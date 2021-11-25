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
		[Header("Networking")]
		public string address;
		public ushort port = 1414;

		[Header("Display")]
		public ConnectionTile connectionTile;

		[Header("Info")]
		[ReadOnly] [SerializeField] private byte playerID; public byte PlayerID => playerID;
		[SerializeField] GameObject playerPrefab;


		void Update() { UpdateClient(); }
		void OnDestroy() { ShutdownClient(); }


		[InspectorButton]
		public void StartClient() { InitializeClient(address, port); }


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
					connectionTile.Log($"Didn't understand OpCode {opCode}");
					break;
			}
		}

		void ReceivePlayerID(AssignPlayerID assignPlayerID)
		{
			playerID = assignPlayerID.PlayerID;
			connectionTile.Log($"Connected to {address} as ID {playerID}");
		}

		void ReceiveChatMessage(ChatMessage chatMessage)
		{
			connectionTile.Log($"[{chatMessage.PlayerID}]: {chatMessage.Text}");
		}
	}
}
