using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using NatesJauntyTools;
using NatesJauntyTools.NetCode;

namespace NatesJauntyTools.Examples.NetCode
{
	public class Server : BaseServer
	{
		[InspectorButton]
		public void StartServer() { Startup(); }


		protected override void OnData(DataStreamReader reader)
		{
			OpCode opCode = (OpCode)reader.ReadByte();
			switch (opCode)
			{
				case OpCode.ChatMessage:
					SendToAllClients(new ChatMessage(reader));
					break;

				default:
					NetLog($"Didn't understand OpCode {opCode}");
					break;
			}
		}

		protected override void HandleNewConnection(NetworkConnection connection)
		{
			connectionCount++;
			SendToClient(connection, new AssignPlayerID(connectionCount));
			Debug.Log($"SERVER: Assigned player ID {connectionCount}");
		}
	}
}
