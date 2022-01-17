using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;
using NatesJauntyTools.NetCode;

namespace NatesJauntyTools.Examples.NetCode
{
	public class PlayerPosition : NetMessage
	{
		public byte PlayerID { get; set; }
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }


		public PlayerPosition()
		{
			Code = (byte)OpCode.PlayerPosition;
		}

		public PlayerPosition(DataStreamReader reader)
		{
			Code = (byte)OpCode.PlayerPosition;
			Deserialize(reader);
		}

		public PlayerPosition(byte playerID, float x, float y, float z)
		{
			Code = (byte)OpCode.PlayerPosition;
			PlayerID = playerID;
			X = x;
			Y = y;
			Z = z;
		}

		public PlayerPosition(Client client, float x, float y, float z)
		{
			Code = (byte)OpCode.PlayerPosition;
			PlayerID = client.PlayerID;
			X = x;
			Y = y;
			Z = z;
		}

		public override void Serialize(ref DataStreamWriter writer)
		{
			writer.WriteByte(Code);
			writer.WriteByte(PlayerID);
			writer.WriteFloat(X);
			writer.WriteFloat(Y);
			writer.WriteFloat(Z);
		}

		public override void Deserialize(DataStreamReader reader)
		{
			// Already read code byte
			PlayerID = reader.ReadByte();
			X = reader.ReadFloat();
			Y = reader.ReadFloat();
			Z = reader.ReadFloat();
		}
	}
}
