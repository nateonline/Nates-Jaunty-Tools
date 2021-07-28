using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;

namespace NatesJauntyTools.NetCode
{
	public abstract class BaseClient : Script
	{
		[ReadOnly] public bool IsInitialized;
		protected NetworkDriver driver;
		protected NetworkConnection serverConnection;


		public virtual void InitializeClient(string address, ushort port)
		{
			if (!IsInitialized)
			{
				Debug.Log("CLIENT: Initializing");

				driver = NetworkDriver.Create();
				serverConnection = default(NetworkConnection);

				NetworkEndPoint endPoint;
				if (address == "localhost")
				{
					endPoint = NetworkEndPoint.LoopbackIpv4;
					endPoint.Port = port;
				}
				else
				{
					endPoint = NetworkEndPoint.Parse(address, port);
				}

				serverConnection = driver.Connect(endPoint);
				IsInitialized = true;
			}
			else { Debug.LogWarning("CLIENT: Can't initialize client, it's already initialized"); }
		}

		public virtual void ShutdownClient()
		{
			if (IsInitialized)
			{
				Debug.Log("CLIENT: Shutting down");

				if (driver.IsCreated) driver.Dispose();
				else { Debug.LogWarning("CLIENT: No driver to dispose of"); }
			}

			IsInitialized = false;
		}

		protected virtual void UpdateClient()
		{
			if (IsInitialized)
			{
				driver.ScheduleUpdate().Complete();
				CheckAlive();
				UpdateMessagePump();
			}
		}

		void CheckAlive()
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
					Debug.Log($"CLIENT: Connected to the server");
				}
				else if (cmd == NetworkEvent.Type.Data)
				{
					OnData(reader);
				}
				else if (cmd == NetworkEvent.Type.Disconnect)
				{
					Debug.Log("CLIENT: Disconnected from the server");
					serverConnection = default(NetworkConnection);
				}
				else { Debug.Log($"CLIENT: Received {cmd} from server unexpectedly"); }
			}
		}

		protected abstract void OnData(DataStreamReader reader);

		public void SendToServer(BaseMessage message)
		{
			if (IsInitialized)
			{
				DataStreamWriter writer;
				driver.BeginSend(serverConnection, out writer);
				message.Serialize(ref writer);
				driver.EndSend(writer);
			}
			else { Debug.LogWarning("CLIENT: Can't send message to server, client isn't initialized"); }
		}
	}
}
