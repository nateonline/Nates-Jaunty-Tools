using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using TMPro;

namespace NatesJauntyTools.Examples.NetCode
{
	public class ClientTile : ConnectionTile
	{
		public Client client;

		void Awake() => client.OnNetLog += Log;
		void OnDestroy() => client.OnNetLog -= Log;


		protected override void OnSendChat(string message)
		{
			client.SendToServer(new ChatMessage(client, message));
		}

		protected override void OnCommand(string command, string[] args)
		{
			command = command.ToLower();

			switch (command)
			{
				case "start":
					client.StartClient();
					Log("Client started!");
					break;

				case "clear":
					logs.text = "";
					break;

				case "address":
					if (args.Length < 1) { Log($"No address specified"); }
					else
					{
						client.address = args[0];
						Log($"Set address to {args[0]}");
					}
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
