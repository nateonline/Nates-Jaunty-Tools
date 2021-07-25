using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;

namespace NatesJauntyTools.NetCode
{
	public class BaseClient : MonoBehaviour
	{
		public NetworkDriver driver;
		protected NetworkConnection serverConnection;


		#if UNITY_EDITOR
		private void Start() { Initialize(); }
		private void OnDestroy() { Shutdown(); }
		private void Update() { UpdateClient(); }
		#endif

		
		public virtual void Initialize()
		{
			driver = NetworkDriver.Create();
			serverConnection = default(NetworkConnection);

			NetworkEndPoint endPoint = NetworkEndPoint.LoopbackIpv4;
			endPoint.Port = 1414;
			serverConnection = driver.Connect(endPoint);
		}

		public virtual void Shutdown()
		{
			if (driver.IsCreated) driver.Dispose();
		}

		public virtual void UpdateClient()
		{
			driver.ScheduleUpdate().Complete();
			CheckAlive();
			UpdateMessagePump();
		}

		private void CheckAlive()
		{
			if (!serverConnection.IsCreated) { Debug.Log("CLIENT: Something went wrong, lost connection to server!"); }
		}

		protected void UpdateMessagePump()
		{
			DataStreamReader reader;
			
			NetworkEvent.Type cmd;
			int safetyNetCounter = 1000;
			while (safetyNetCounter > 0 && (cmd = serverConnection.PopEvent(driver, out reader)) != NetworkEvent.Type.Empty)
			{
				if (--safetyNetCounter == 0) { Debug.LogError("CLIENT: Hit max iterations, exiting", gameObject); return; }

				if (cmd == NetworkEvent.Type.Connect)
				{
					Debug.Log($"CLIENT: We are now connected to the server!");
				}
				else if (cmd == NetworkEvent.Type.Data)
				{
					OnData(reader);
				}
				else if (cmd == NetworkEvent.Type.Disconnect)
				{
					Debug.Log("CLIENT: Client has disconnected from the server");
					serverConnection = default(NetworkConnection);
				}
				else { Debug.Log($"CLIENT: Received {cmd} from client unexpectedly"); }
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

			message.ReceivedOnClient();
		}

		public virtual void SendToServer(BaseMessage message)
		{
			DataStreamWriter writer;
			driver.BeginSend(serverConnection, out writer);
			message.Serialize(ref writer);
			driver.EndSend(writer);
		}
	}
}
