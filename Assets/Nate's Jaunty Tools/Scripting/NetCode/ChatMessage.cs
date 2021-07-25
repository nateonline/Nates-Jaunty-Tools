using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
using NatesJauntyTools.NetCode;

public class ChatMessage : BaseMessage
{
    FixedString128 MessageText { get; set; }

	
	public ChatMessage()
	{
		Code = OpCode.ChatMessage.ToByte();
	}

	public ChatMessage(DataStreamReader reader)
	{
		Code = OpCode.ChatMessage.ToByte();
		Deserialize(reader);
	}
	
	public ChatMessage(string messageText)
	{
		Code = OpCode.ChatMessage.ToByte();
		MessageText = messageText;
	}

	
	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte(Code);
		writer.WriteFixedString128(MessageText);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		// Already read first byte / op code
		MessageText = reader.ReadFixedString128();
	}

	public override void ReceivedOnClient() { Debug.Log($"CLIENT: {MessageText}"); }
	public override void ReceivedOnServer(BaseServer server) { Debug.Log($"SERVER: {MessageText}"); }
}
