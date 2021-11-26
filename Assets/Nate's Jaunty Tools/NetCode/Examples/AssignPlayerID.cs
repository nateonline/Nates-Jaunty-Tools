using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
using NatesJauntyTools.NetCode;

namespace NatesJauntyTools.Examples.NetCode
{
	public class AssignPlayerID : NetMessage
	{
		public byte PlayerID { get; set; }


		public AssignPlayerID()
		{
			Code = (byte)OpCode.AssignPlayerID;
		}

		public AssignPlayerID(DataStreamReader reader)
		{
			Code = (byte)OpCode.AssignPlayerID;
			Deserialize(reader);
		}

		public AssignPlayerID(byte playerID)
		{
			Code = (byte)OpCode.AssignPlayerID;
			PlayerID = playerID;
		}

		public override void Serialize(ref DataStreamWriter writer)
		{
			writer.WriteByte(Code);
			writer.WriteByte(PlayerID);
		}

		public override void Deserialize(DataStreamReader reader)
		{
			// Already read code byte
			PlayerID = reader.ReadByte();
		}
	}
}
