using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NatesJauntyTools.NetCode;
using TMPro;

public class Chat : MonoBehaviour
{
    public TMP_InputField chatInput;

	public void SendChatMessage()
	{
		FindObjectOfType<BaseClient>().SendToServer(new ChatMessage(chatInput.text));
	}
}
