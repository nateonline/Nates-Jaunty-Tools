using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using NatesJauntyTools;
using NatesJauntyTools.NetCode;

public class Client : BaseClient
{
    [Header("Networking")]
	public string address;
	public ushort port;

	[Header("Other References")]
	[SerializeField] ChatUI chatUI;
	

	void Update() { UpdateClient(); }
	void OnDestroy() { Shutdown(); }

	
	[InspectorButton]
	public void StartClient() { Initialize(address, port); }

	
	protected override void OnData(DataStreamReader reader)
	{
		OpCode opCode = reader.ReadByte().ToOpCode();
		switch (opCode)
		{
			case OpCode.ChatMessage: ReceiveChatMessage(new ChatMessage(reader)); break;
			default: Debug.LogWarning($"CLIENT: Didn't understand OpCode {opCode}", gameObject); break;
		}
	}

	public void ReceiveChatMessage(ChatMessage chatMessage)
	{
		chatUI.ReceiveChat(chatMessage.PlayerID, chatMessage.Text.ToString());
	}
}
