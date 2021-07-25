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
		Code = OpCode.PlayerPosition.ToByte();
	}

	public PlayerPosition(DataStreamReader reader)
	{
		Code = OpCode.PlayerPosition.ToByte();
		Deserialize(reader);
	}
	
	public PlayerPosition(byte playerID, float x, float y, float z)
	{
		Code = OpCode.PlayerPosition.ToByte();
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
		// Already read first byte / op code
		PlayerID = reader.ReadByte();
		PositionX = reader.ReadFloat();
		PositionY = reader.ReadFloat();
		PositionZ = reader.ReadFloat();
	}

	public override void ReceivedOnClient() { Debug.Log($"CLIENT: pID={PlayerID} x={PositionX} y={PositionY} z={PositionZ}"); }
	public override void ReceivedOnServer() { Debug.Log($"SERVER: pID={PlayerID} x={PositionX} y={PositionY} z={PositionZ}"); }
}
