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

		[SerializeField] RectTransform connectionParent;
		[SerializeField] GameObject serverTilePrefab;
		[SerializeField] GameObject hostTilePrefab;
		[SerializeField] GameObject clientTilePrefab;

		List<ConnectionTile> connections = new List<ConnectionTile>();


		public void AddConnection(string connectionType)
		{
			// GameObject newConnectionObject = Instantiate(connectionPrefab, connectionParent);
			// ConnectionTile newConnectionTile = newConnectionObject.GetComponent<ConnectionTile>();

			// connections.Add(newConnectionTile);
			// newConnectionObject.name = connectionType;
			// newConnectionTile.title.text = connectionType;
			// newConnectionTile.ui = this;


			ConnectionTile newConnection = null;
			switch (connectionType)
			{
				case "Server":
					newConnection = Instantiate(serverTilePrefab, connectionParent).GetComponent<ServerTile>();
					break;

				case "Host":
					break;

				case "Client":
					newConnection = Instantiate(clientTilePrefab, connectionParent).GetComponent<ClientTile>();
					break;
			}
			newConnection.name = connectionType;
			newConnection.title.text = connectionType;
			newConnection.ui = this;
			connections.Add(newConnection);

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
			bool serverOrHostExists = connections.Any(c => c is ServerTile);

			serverButton.Interactable = !serverOrHostExists;
			hostButton.Interactable = !serverOrHostExists;
		}
	}
}
