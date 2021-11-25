using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NatesJauntyTools.Examples.NetCode
{
	public class NetCodeUI : MonoBehaviour
	{
		[SerializeField] AddConnectionButton serverButton;
		[SerializeField] AddConnectionButton hostButton;
		[SerializeField] AddConnectionButton clientButton;

		[SerializeField] GameObject connectionPrefab;
		[SerializeField] RectTransform connectionParent;

		List<ConnectionTile> connections = new List<ConnectionTile>();


		public void AddConnection(string connectionType)
		{
			GameObject newConnectionObject = Instantiate(connectionPrefab, connectionParent);
			ConnectionTile newConnectionTile = newConnectionObject.GetComponent<ConnectionTile>();

			connections.Add(newConnectionTile);
			newConnectionObject.name = connectionType;
			newConnectionTile.title.text = connectionType;
			newConnectionTile.ui = this;

			switch (connectionType)
			{
				case "Server":
					Server newServer = newConnectionObject.AddComponent<Server>();
					newServer.connectionTile = newConnectionTile;
					newConnectionTile.connection = newServer;
					break;

				case "Host":
					Host newHost = newConnectionObject.AddComponent<Host>();
					newHost.connectionTile = newConnectionTile;
					newConnectionTile.connection = newHost;
					break;

				case "Client":
					Client newClient = newConnectionObject.AddComponent<Client>();
					newClient.connectionTile = newConnectionTile;
					newConnectionTile.connection = newClient;
					newClient.address = "localhost";
					break;
			}

			ValidateButtons();
		}

		public void RemoveConnection(ConnectionTile tile)
		{
			connections.Remove(tile);
			Destroy(tile.gameObject);
			ValidateButtons();
		}

		void ValidateButtons()
		{
			bool serverOrHostExists = false;
			foreach (ConnectionTile tile in connections)
			{
				switch (tile.connection)
				{
					case Server server:
					case Host host:
						serverOrHostExists = true;
						break;
				}
			}

			serverButton.Interactable = !serverOrHostExists;
			hostButton.Interactable = !serverOrHostExists;
		}
	}
}
