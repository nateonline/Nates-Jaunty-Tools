using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace NatesJauntyTools.Examples.NetCode
{
	public class ConnectionTile : MonoBehaviour
	{
		public NetCodeUI ui;
		public MonoBehaviour connection;
		public TMP_Text title;
		[SerializeField] TMP_Text logs;

		public void StartConnection()
		{
			switch (connection)
			{
				case Server server:
					server.StartServer();
					break;

				case Host host:
					break;

				case Client client:
					client.StartClient();
					break;
			}
		}

		public void RemoveConnection() => ui.RemoveConnection(this);

		public void Log(string message) => logs.text += "\n" + message;
	}
}
