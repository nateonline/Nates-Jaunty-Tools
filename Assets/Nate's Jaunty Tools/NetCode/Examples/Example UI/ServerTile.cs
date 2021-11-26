using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using TMPro;
using NatesJauntyTools;

namespace NatesJauntyTools.Examples.NetCode
{
	public class ServerTile : ConnectionTile
	{
		public Server server;

		void Awake() => server.OnNetLog += Log;
		void OnDestroy() => server.OnNetLog -= Log;


		protected override void OnSendChat(string message)
		{
			Log($"[0]: {message}");
			server.SendToAllClients(new ChatMessage(0, new FixedString512(message)));
		}

		protected override void OnCommand(string command, string[] args)
		{
			command = command.ToLower();

			switch (command)
			{
				case "start":
					server.StartServer();
					Log($"Server started!");
					break;

				case "clear":
					logs.text = "";
					break;

				case "ip":
					Log(Tools.GetLocalIPAddress());
					break;

				default:
					Log($"Command \"{command}\" not recignized");
					break;
			}
		}
	}
}
