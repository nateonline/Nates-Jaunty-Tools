using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using NatesJauntyTools;
using NatesJauntyTools.NetCode;

namespace NatesJauntyTools.Examples.NetCode
{
	public class Client : BaseClient
	{
		[InspectorButton]
		public void StartClient() { Startup(); }


		protected override void OnData(DataStreamReader reader)
		{
			OpCode opCode = (OpCode)reader.ReadByte();
			switch (opCode)
			{
				case OpCode.AssignPlayerID:
					ReceivePlayerID(new AssignPlayerID(reader));
					break;

				case OpCode.ChatMessage:
					ReceiveChatMessage(new ChatMessage(reader));
					break;

				default:
					NetLog($"Didn't understand OpCode {opCode}");
					break;
			}
		}

		void ReceivePlayerID(AssignPlayerID assignPlayerID)
		{
			clientID = assignPlayerID.PlayerID;
			NetLog($"Connected to {address} as ID {clientID}");
		}

		void ReceiveChatMessage(ChatMessage chatMessage)
		{
			NetLog($"[{chatMessage.PlayerID}]: {chatMessage.Text}");
		}
	}
}
