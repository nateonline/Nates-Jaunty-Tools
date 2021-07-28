using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using NatesJauntyTools;
using NatesJauntyTools.NetCode;

public class Server : BaseServer
{
	public int maxConnections = 8;
	public ushort port = 1414;
	public byte playerCount = 0;


	[InspectorButton]
	public void StartServer() { InitializeServer(maxConnections, port); }

	void Update() { UpdateServer(); }
	void OnDestroy() { ShutdownServer(); }


	protected override void OnData(DataStreamReader reader)
	{
		OpCode opCode = (OpCode)reader.ReadByte();
		switch (opCode)
		{
			case OpCode.ChatMessage: SendToAllClients(new ChatMessage(reader)); break;
			case OpCode.PlayerPosition: SendToAllClients(new PlayerPosition(reader)); break;
			default: Debug.LogWarning($"SERVER: Didn't understand OpCode {opCode}", gameObject); break;
		}
	}

	protected override void OnNewConnection(NetworkConnection connection)
	{
		playerCount++;
		SendToClient(connection, new AssignPlayerID(playerCount));
		Debug.Log($"SERVER: Assigned player ID {playerCount}");
	}
}
