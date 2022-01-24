using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;

namespace NatesJauntyTools.NetCode
{
	public abstract class BaseServer : NetEndpoint
	{
		public event Action<NetworkConnection> OnNewConnection;

		protected NativeList<NetworkConnection> clientConnections;

		public int maxConnections;
		public ushort port = 1414;
		[ReadOnly] public byte connectionCount = 0;


		public override void Startup()
		{
			if (!IsInitialized)
			{
				NetLog("SERVER: Initializing");

				// Initialize Driver
				driver = NetworkDriver.Create();
				NetworkEndPoint endPoint = NetworkEndPoint.AnyIpv4; // Who can connect to the server
				endPoint.Port = port;

				if (driver.Bind(endPoint) != 0)
				{
					NetLog($"SERVER: There was an error binding to port {endPoint.Port}");
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
				NetLog("SERVER: Can't initialize server, it's already initialized");
			}
		}

		public override void Shutdown()
		{
			if (IsInitialized)
			{
				NetLog("SERVER: Shutting down");

				if (driver.IsCreated) { driver.Dispose(); }
				else { NetLog("SERVER: No driver to dispose of"); }

				if (clientConnections.IsCreated) { clientConnections.Dispose(); }
				else { NetLog("SERVER: No client connections to dispose of"); }
			}

			IsInitialized = false;
		}

		public override void Upkeep()
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

		void AcceptNewConnections()
		{
			NetworkConnection newConnection;
			int safetyNetCounter = 100;
			while (safetyNetCounter > 0 && (newConnection = driver.Accept()) != default(NetworkConnection))
			{
				if (--safetyNetCounter == 0) { NetLog("SERVER: Hit max iterations, exiting"); return; }

				clientConnections.Add(newConnection);
				NetLog("SERVER: Accepted a connection");
				HandleNewConnection(newConnection);
				OnNewConnection?.Invoke(newConnection);
			}
		}

		protected virtual void HandleNewConnection(NetworkConnection newConnection) { }

		void UpdateMessagePump()
		{
			DataStreamReader reader;
			for (int i = 0; i < clientConnections.Length; i++)
			{
				NetworkEvent.Type cmd;
				int safetyNetCounter = 1000;
				while (safetyNetCounter > 0 && (cmd = driver.PopEventForConnection(clientConnections[i], out reader)) != NetworkEvent.Type.Empty)
				{
					if (--safetyNetCounter == 0) { NetLog("SERVER: Hit max iterations, exiting"); return; }

					if (cmd == NetworkEvent.Type.Data)
					{
						OnData(reader);
					}
					else if (cmd == NetworkEvent.Type.Disconnect)
					{
						NetLog("SERVER: Client has disconnected from the server");
						clientConnections[i] = default(NetworkConnection);
					}
					else { NetLog($"SERVER: Received {cmd} from client unexpectedly"); }
				}
			}
		}

		public void SendToClient(NetworkConnection clientConnection, NetMsg message)
		{
			if (IsInitialized)
			{
				DataStreamWriter writer;
				driver.BeginSend(clientConnection, out writer);
				message.Serialize(ref writer);
				driver.EndSend(writer);
			}
			else { NetLog("SERVER: Can't send message to client, server isn't initialized"); }
		}

		public void SendToAllClients(NetMsg message)
		{
			if (IsInitialized)
			{
				foreach (NetworkConnection client in clientConnections)
				{
					if (client.IsCreated) { SendToClient(client, message); }
				}
			}
			else { NetLog("SERVER: Can't send message to all clients, server isn't initialized"); }
		}
	}
}
