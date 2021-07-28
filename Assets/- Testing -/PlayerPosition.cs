using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
using NatesJauntyTools.NetCode;

public class PlayerPosition : BaseMessage
{
	public byte PlayerID { get; set; }
	public float PositionX { get; set; }
	public float PositionY { get; set; }
	public float PositionZ { get; set; }


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
		PositionX = x;
		PositionY = y;
		PositionZ = z;
	}

	public override void Serialize(ref DataStreamWriter writer)
	{
		writer.WriteByte(Code);
		writer.WriteByte(PlayerID);
		writer.WriteFloat(PositionX);
		writer.WriteFloat(PositionY);
		writer.WriteFloat(PositionZ);
	}

	public override void Deserialize(DataStreamReader reader)
	{
		// Already read code byte
		PlayerID = reader.ReadByte();
		PositionX = reader.ReadFloat();
		PositionY = reader.ReadFloat();
		PositionZ = reader.ReadFloat();
	}
}
