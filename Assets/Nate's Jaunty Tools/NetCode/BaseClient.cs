using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;

namespace NatesJauntyTools.NetCode
{
	public abstract class BaseClient : NetPoint
	{
		protected NetworkConnection serverConnection;

		public string address = "localhost";
		public ushort port = 1414;


		public override void Startup()
		{
			if (!IsInitialized)
			{
				NetLog("CLIENT: Initializing");

				driver = NetworkDriver.Create();
				serverConnection = default(NetworkConnection);

				NetworkEndPoint endPoint;
				if (address == "localhost")
				{
					endPoint = NetworkEndPoint.LoopbackIpv4;
					endPoint.Port = port;
				}
				else if (NetworkEndPoint.TryParse(address, port, out NetworkEndPoint newEndPoint))
				{
					endPoint = newEndPoint;
				}
				else
				{
					NetLog($"CLIENT: Can't find address \"{address}\"");
					return;
				}

				serverConnection = driver.Connect(endPoint);
				IsInitialized = true;
			}
			else { NetLog("CLIENT: Can't initialize client, it's already initialized"); }
		}

		public override void Shutdown()
		{
			if (IsInitialized)
			{
				NetLog("CLIENT: Shutting down");

				if (driver.IsCreated) { driver.Dispose(); }
				else { NetLog("CLIENT: No driver to dispose of"); }
			}

			IsInitialized = false;
		}

		public override void Upkeep()
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
			if (!serverConnection.IsCreated) { NetLog("CLIENT: Something went wrong, lost connection to server!"); }
		}

		void UpdateMessagePump()
		{
			DataStreamReader reader;

			NetworkEvent.Type cmd;
			int safetyNetCounter = 1000;
			while (safetyNetCounter > 0 && (cmd = serverConnection.PopEvent(driver, out reader)) != NetworkEvent.Type.Empty)
			{
				if (--safetyNetCounter == 0)
				{
					NetLog("CLIENT: Hit max iterations, exiting");
					return;
				}

				switch (cmd)
				{
					case NetworkEvent.Type.Connect:
						NetLog($"CLIENT: Connected to the server");
						break;

					case NetworkEvent.Type.Disconnect:
						NetLog("CLIENT: Disconnected from the server");
						serverConnection = default(NetworkConnection);
						break;

					case NetworkEvent.Type.Data:
						OnData(reader);
						break;

					default:
						NetLog($"CLIENT: Received {cmd} from server unexpectedly");
						break;
				}
			}
		}

		public void SendToServer(NetMessage message)
		{
			if (IsInitialized)
			{
				DataStreamWriter writer;
				driver.BeginSend(serverConnection, out writer);
				message.Serialize(ref writer);
				driver.EndSend(writer);
			}
			else { NetLog("CLIENT: Can't send message to server, client isn't initialized"); }
		}
	}
}
