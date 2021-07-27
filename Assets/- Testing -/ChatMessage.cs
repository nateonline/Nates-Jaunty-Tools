using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
using NatesJauntyTools.NetCode;

public class ChatMessage : BaseMessage
{
    public byte PlayerID { get; set; }
	public FixedString512 Text { get; set; }

	
	public ChatMessage()
	{
		Code = OpCode.ChatMessage.ToByte();
	}

    public ChatMessage(DataStreamReader reader)
	{
		Code = OpCode.ChatMessage.ToByte();
		Deserialize(reader);
	}

	public ChatMessage(byte playerID, FixedString512 text)
	{
		Code = OpCode.ChatMessage.ToByte();
		PlayerID = playerID;
		Text = text;
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte(Code);
		writer.WriteByte(PlayerID);
		writer.WriteFixedString512(Text);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		PlayerID = reader.ReadByte();
		Text = reader.ReadFixedString512();
	}
}
