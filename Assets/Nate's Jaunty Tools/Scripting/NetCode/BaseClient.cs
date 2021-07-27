using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;

namespace NatesJauntyTools.NetCode
{
	public abstract class BaseClient : Script
	{
		protected NetworkDriver driver;
		protected NetworkConnection serverConnection;

		
		public virtual void Initialize(string address, ushort port)
		{
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
		}

		public virtual void Shutdown()
		{
			if (driver.IsCreated) driver.Dispose();
			else { Debug.LogWarning("No driver to dispose of"); }
		}

		protected virtual void UpdateClient()
		{
			if (driver.IsCreated)
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

		protected abstract void OnData(DataStreamReader reader);

		public void SendToServer(BaseMessage message)
		{
			DataStreamWriter writer;
			driver.BeginSend(serverConnection, out writer);
			message.Serialize(ref writer);
			driver.EndSend(writer);
		}
	}
}
