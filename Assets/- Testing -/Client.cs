using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using NatesJauntyTools;
using NatesJauntyTools.NetCode;
using TMPro;

public class Client : BaseClient
{
	[Header("Networking")]
	public string address;
	public ushort port = 1414;

	[Header("Other References")]
	[SerializeField] ChatUI chatUI;

	[Header("Info")]
	[ReadOnly] [SerializeField] private byte playerID; public byte PlayerID { get { return playerID; } }
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
			case OpCode.AssignPlayerID: ReceivePlayerID(new AssignPlayerID(reader)); break;
			case OpCode.ChatMessage: ReceiveChatMessage(new ChatMessage(reader)); break;
			case OpCode.PlayerPosition: ReceivePlayerPosition(new PlayerPosition(reader)); break;
			default: Debug.LogWarning($"CLIENT: Didn't understand OpCode {opCode}", gameObject); break;
		}
	}

	void ReceiveChatMessage(ChatMessage chatMessage)
	{
		chatUI.ReceiveChat(chatMessage.PlayerID, chatMessage.Text.ToString());
	}

	void ReceivePlayerID(AssignPlayerID assignPlayerID)
	{
		playerID = assignPlayerID.PlayerID;
	}

	void ReceivePlayerPosition(PlayerPosition playerPosition)
	{
		transform.position = new Vector3(playerPosition.PositionX, playerPosition.PositionY, playerPosition.PositionZ);
	}
}
