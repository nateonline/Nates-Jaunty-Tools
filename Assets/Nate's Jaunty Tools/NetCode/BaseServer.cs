using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;

namespace NatesJauntyTools.NetCode
{
	public abstract class BaseServer : Script
	{
		[ReadOnly] public bool IsInitialized;
		protected NetworkDriver driver;
		protected NativeList<NetworkConnection> clientConnections;


		public virtual void InitializeServer(int maxConnections, ushort port)
		{
			if (!IsInitialized)
			{
				Debug.Log("SERVER: Initializing");

				// Initialize Driver
				driver = NetworkDriver.Create();
				NetworkEndPoint endPoint = NetworkEndPoint.AnyIpv4; // Who can connect to the server
				endPoint.Port = port;

				if (driver.Bind(endPoint) != 0)
				{
					Debug.LogWarning($"SERVER: There was an error binding to port {endPoint.Port}");
				}
				else
				{
					driver.Listen();
					IsInitialized = true;

					// Initialize Connection List
					clientConnections = new NativeList<NetworkConnection>(maxConnections, Allocator.Persistent);
				}
			}
			else
			{
				Debug.LogWarning("SERVER: Can't initialize server, it's already initialized");
			}
		}

		public virtual void ShutdownServer()
		{
			if (IsInitialized)
			{
				Debug.Log("SERVER: Shutting down");

				if (driver.IsCreated) { driver.Dispose(); }
				else { Debug.LogWarning("SERVER: No driver to dispose of"); }

				if (clientConnections.IsCreated) { clientConnections.Dispose(); }
				else { Debug.LogWarning("SERVER: No client connections to dispose of"); }
			}

			IsInitialized = false;
		}

		protected void UpdateServer()
		{
			if (IsInitialized)
			{
				driver.ScheduleUpdate().Complete();
				CleanupConnections();
				AcceptNewConnections();
				UpdateMessagePump();
			}
		}

		void CleanupConnections()
		{
			for (int i = 0; i < clientConnections.Length; i++)
			{
				if (!clientConnections[i].IsCreated)
				{
					clientConnections.RemoveAtSwapBack(i);
					i--;
				}
			}
		}

		protected virtual void AcceptNewConnections()
		{
			NetworkConnection c;
			int safetyNetCounter = 100;
			while (safetyNetCounter > 0 && (c = driver.Accept()) != default(NetworkConnection))
			{
				if (--safetyNetCounter == 0) { Debug.LogError("SERVER: Hit max iterations, exiting", gameObject); return; }

				clientConnections.Add(c);
				Debug.Log("SERVER: Accepted a connection");
				OnNewConnection(c);
			}
		}

		protected virtual void OnNewConnection(NetworkConnection connection) { }

		protected void UpdateMessagePump()
		{
			DataStreamReader reader;
			for (int i = 0; i < clientConnections.Length; i++)
			{
				NetworkEvent.Type cmd;
				int safetyNetCounter = 1000;
				while (safetyNetCounter > 0 && (cmd = driver.PopEventForConnection(clientConnections[i], out reader)) != NetworkEvent.Type.Empty)
				{
					if (--safetyNetCounter == 0) { Debug.LogError("SERVER: Hit max iterations, exiting", gameObject); return; }

					if (cmd == NetworkEvent.Type.Data)
					{
						OnData(reader);
					}
					else if (cmd == NetworkEvent.Type.Disconnect)
					{
						Debug.Log("SERVER: Client has disconnected from the server");
						clientConnections[i] = default(NetworkConnection);
					}
					else { Debug.Log($"SERVER: Received {cmd} from client unexpectedly"); }
				}
			}
		}

		protected abstract void OnData(DataStreamReader reader);

		public void SendToClient(NetworkConnection clientConnection, BaseMessage message)
		{
			if (IsInitialized)
			{
				DataStreamWriter writer;
				driver.BeginSend(clientConnection, out writer);
				message.Serialize(ref writer);
				driver.EndSend(writer);
			}
			else { Debug.LogWarning("SERVER: Can't send message to client, server isn't initialized"); }
		}

		public void SendToAllClients(BaseMessage message)
		{
			if (IsInitialized)
			{
				foreach (NetworkConnection client in clientConnections)
				{
					if (client.IsCreated) { SendToClient(client, message); }
				}
			}
			else { Debug.LogWarning("SERVER: Can't send message to all clients, server isn't initialized"); }
		}
	}
}
