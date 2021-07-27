using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NatesJauntyTools.NetCode;
using TMPro;

public class ChatUI : MonoBehaviour
{
    [SerializeField] Client client;
	
	[SerializeField] TMP_InputField inputField;
	[SerializeField] TMP_Text chatMessageDisplay;
	
	
	public void SendChat()
	{
		ChatMessage chatMessage = new ChatMessage(1, inputField.text);
		client.SendToServer(chatMessage);
	}

	public void ReceiveChat(int playerID, string text)
	{
		chatMessageDisplay.text += $"\n[{playerID}]: {text}";
	}
}