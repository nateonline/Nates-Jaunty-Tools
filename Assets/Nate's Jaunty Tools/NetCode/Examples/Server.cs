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
		public int maxConnections = 8;
		public ushort port = 1414;
		public byte playerCount = 0;

		[Header("Display")]
		public ConnectionTile connectionTile;


		[InspectorButton]
		public void StartServer() { InitializeServer(maxConnections, port); }

		void Update() { UpdateServer(); }
		void OnDestroy() { ShutdownServer(); }


		protected override void OnData(DataStreamReader reader)
		{
			OpCode opCode = (OpCode)reader.ReadByte();
			switch (opCode)
			{
				case OpCode.ChatMessage:
					SendToAllClients(new ChatMessage(reader));
					break;

				default:
					connectionTile.Log($"Didn't understand OpCode {opCode}");
					break;
			}
		}

		protected override void OnNewConnection(NetworkConnection connection)
		{
			playerCount++;
			SendToClient(connection, new AssignPlayerID(playerCount));
			Debug.Log($"SERVER: Assigned player ID {playerCount}");
		}
	}
}
