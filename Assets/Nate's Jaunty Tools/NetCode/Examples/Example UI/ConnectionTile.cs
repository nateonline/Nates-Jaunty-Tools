using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace NatesJauntyTools.Examples.NetCode
{
	public abstract class ConnectionTile : MonoBehaviour
	{
		public NetCodeUI ui;
		public TMP_Text title;
		[SerializeField] protected TMP_Text logs;
		[SerializeField] protected TMP_InputField commandLine;


		public void RemoveConnection() => ui.RemoveConnection(this);

		public void Log(string message) => logs.text += "\n" + message;

		public void OnSubmitText(string text)
		{
			if (Input.GetKeyDown(KeyCode.Return) && text.Length > 0)
			{
				commandLine.text = "";
				commandLine.ActivateInputField();
				commandLine.Select();

				if (text[0] == '/')
				{
					text = text.Remove(0, 1);
					string[] allParts = text.Split(' ');
					OnCommand(allParts[0], allParts.Skip(1).ToArray());
				}
				else
				{
					Debug.Log($"Chat? {text}");
				}
			}
		}

		protected abstract void OnCommand(string command, string[] args);
	}
}
