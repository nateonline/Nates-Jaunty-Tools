using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using NatesJauntyTools;
using NatesJauntyTools.NetCode;

public class Server : BaseServer
{
    public int maxConnections = 8;
	public ushort port;
	

	void Update() { UpdateServer(); }
	void OnDestroy() { Shutdown(); }
	

	[InspectorButton]
	public void StartServer() { Initialize(maxConnections, port); }


    protected override void OnData(DataStreamReader reader)
	{
		OpCode opCode = reader.ReadByte().ToOpCode();
		switch (opCode)
		{
			case OpCode.ChatMessage: ReceiveChatMessage(new ChatMessage(reader)); break;
			default: Debug.LogWarning($"SERVER: Didn't understand OpCode {opCode}", gameObject); break;
		}
	}

	void ReceiveChatMessage(ChatMessage chatMessage)
	{
		SendToAllClients(chatMessage);
	}
}
