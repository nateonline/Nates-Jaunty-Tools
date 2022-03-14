using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
using NatesJauntyTools.NetCode;

namespace NatesJauntyTools.Examples.NetCode
{
	public class ChatMessage : NetMsg
	{
		public byte PlayerID { get; set; }
		public FixedString512 Text { get; set; }


		public ChatMessage()
		{
			Code = (byte)OpCode.ChatMessage;
		}

		public ChatMessage(DataStreamReader reader)
		{
			Code = (byte)OpCode.ChatMessage;
			Deserialize(reader);
		}

		public ChatMessage(byte playerID, FixedString512 text)
		{
			Code = (byte)OpCode.ChatMessage;
			PlayerID = playerID;
			Text = text;
		}

		public ChatMessage(Client client, string text)
		{
			Code = (byte)OpCode.ChatMessage;
			PlayerID = client.PlayerID;
			Text = new FixedString512(text);
		}

		public override void Serialize(ref DataStreamWriter writer)
		{
			writer.WriteByte(Code);
			writer.WriteByte(PlayerID);
			writer.WriteFixedString512(Text);
		}

		public override void Deserialize(DataStreamReader reader)
		{
			// Already read code byte
			PlayerID = reader.ReadByte();
			Text = reader.ReadFixedString512();
		}
	}
}
