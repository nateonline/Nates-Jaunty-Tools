using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;

namespace NatesJauntyTools.NetCode
{
	public abstract class BaseServer : Script
	{
		protected NetworkDriver driver;
		protected NativeList<NetworkConnection> clientConnections;


		public virtual void Initialize(int maxConnections, ushort port)
		{
			// Initialize Driver
			driver = NetworkDriver.Create();
			NetworkEndPoint endPoint = NetworkEndPoint.AnyIpv4; // Who can connect to the server
			endPoint.Port = port;
			if (driver.Bind(endPoint) != 0) { Debug.LogWarning($"SERVER: There was an error binding to port {endPoint.Port}"); }
			else { driver.Listen(); }

			// Initialize Connection List
			clientConnections = new NativeList<NetworkConnection>(maxConnections, Allocator.Persistent);
		}

		public virtual void Shutdown()
		{
			if (driver.IsCreated) { driver.Dispose(); }
			else { Debug.LogWarning("No driver to dispose of"); }

			if (clientConnections.IsCreated) { clientConnections.Dispose(); }
			else { Debug.LogWarning("No client connections to dispose of"); }
		}

		protected void UpdateServer()
		{
			if (driver.IsCreated)
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

		void AcceptNewConnections()
		{
			NetworkConnection c;
			int safetyNetCounter = 100;
			while (safetyNetCounter > 0 && (c = driver.Accept()) != default(NetworkConnection))
			{
				if (--safetyNetCounter == 0) { Debug.LogError("SERVER: Hit max iterations, exiting", gameObject); return; }

				clientConnections.Add(c);
				Debug.Log("SERVER: Accepted a connection");
			}
		}

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
			DataStreamWriter writer;
			driver.BeginSend(clientConnection, out writer);
			message.Serialize(ref writer);
			driver.EndSend(writer);
		}

		public void SendToAllClients(BaseMessage message)
		{
			foreach (NetworkConnection client in clientConnections)
			{
				if (client.IsCreated) { SendToClient(client, message); }
			}
		}
	}
}
