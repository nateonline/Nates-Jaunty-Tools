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


		public override void Upkeep()
		{
			base.Upkeep();

			if (IsInitialized && Mathf.RoundToInt(Time.time) % 5 == 0) { SendToAllClients(new KeepAlive()); }
		}

		protected override void OnData(DataStreamReader reader)
		{
			OpCode opCode = (OpCode)reader.ReadByte();
			switch (opCode)
			{
				case OpCode.KeepAlive:
					break;

				case OpCode.ChatMessage:
					ReceiveChatMessage(new ChatMessage(reader));
					break;

				default:
					NetLog($"Didn't understand OpCode {opCode}");
					break;
			}
		}

		void ReceiveChatMessage(ChatMessage chatMessage)
		{
			NetLog($"[{chatMessage.PlayerID}]: {chatMessage.Text}");
			SendToAllClients(chatMessage);
		}

		protected override void HandleNewConnection(NetworkConnection connection)
		{
			connectionCount++;
			SendToClient(connection, new AssignPlayerID(connectionCount));
			Debug.Log($"SERVER: Assigned player ID {connectionCount}");
		}
	}
}
