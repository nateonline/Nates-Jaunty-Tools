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
		[ReadOnly] [SerializeField] protected byte playerID;
		public byte PlayerID => playerID;


		[InspectorButton]
		public void StartClient() { Startup(); }


		public override void Upkeep()
		{
			base.Upkeep();

			if (IsInitialized && Mathf.RoundToInt(Time.time) % 5 == 0) { SendToServer(new KeepAlive()); }
		}

		protected override void OnData(DataStreamReader reader)
		{
			OpCode opCode = (OpCode)reader.ReadByte();
			switch (opCode)
			{
				case OpCode.KeepAlive:
					break;

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
			playerID = assignPlayerID.PlayerID;
			NetLog($"Connected to {address} as ID {playerID}");
		}

		void ReceiveChatMessage(ChatMessage chatMessage)
		{
			NetLog($"[{chatMessage.PlayerID}]: {chatMessage.Text}");
		}
	}
}
