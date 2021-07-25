using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;

namespace NatesJauntyTools.NetCode
{
	public class BaseServer : MonoBehaviour
	{
		public NetworkDriver driver;
		protected NativeList<NetworkConnection> clientConnections;

		public const int MAX_CONNECTIONS = 4;
		public ushort port = 1414;


		#if UNITY_EDITOR
		private void Start() { Initialize(); }
		private void OnDestroy() { Shutdown(); }
		private void Update() { UpdateServer(); }
		#endif


		public virtual void Initialize()
		{
			// Initialize Driver
			driver = NetworkDriver.Create();
			NetworkEndPoint endPoint = NetworkEndPoint.AnyIpv4; // Who can connect to us
			endPoint.Port = port;
			if (driver.Bind(endPoint) != 0) { Debug.LogWarning($"SERVER: There was an error binding to port {endPoint.Port}"); }
			else { driver.Listen(); }

			// Initialize Connection List
			clientConnections = new NativeList<NetworkConnection>(MAX_CONNECTIONS, Allocator.Persistent);
		}

		public virtual void Shutdown()
		{
			if (driver.IsCreated) { driver.Dispose(); }
			else { Debug.LogWarning("No driver to dispose of"); }

			if (clientConnections.IsCreated) { clientConnections.Dispose(); }
			else { Debug.LogWarning("No client connections to dispose of"); }
		}

		public virtual void UpdateServer()
		{
			driver.ScheduleUpdate().Complete();
			CleanupConnections();
			AcceptNewConnections();
			UpdateMessagePump();
		}

		private void CleanupConnections()
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

		private void AcceptNewConnections()
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

		public virtual void OnData(DataStreamReader reader)
		{
			BaseMessage message = null;
			OpCode opCode = reader.ReadByte().ToOpCode();

			switch (opCode)
			{
				case OpCode.ChatMessage: message = new ChatMessage(reader); break;
				case OpCode.PlayerPosition: message = new PlayerPosition(reader); break;
				default: Debug.LogWarning($"SERVER: Didn't understand OpCode {opCode}"); break;
			}

			message.ReceivedOnServer();
		}
	}
}
