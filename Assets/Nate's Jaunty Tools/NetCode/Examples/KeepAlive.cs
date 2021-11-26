using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;
using NatesJauntyTools.NetCode;

namespace NatesJauntyTools.Examples.NetCode
{
	public class KeepAlive : NetMessage
	{
		public KeepAlive()
		{
			Code = (byte)OpCode.KeepAlive;
		}

		public KeepAlive(DataStreamReader reader)
		{
			Code = (byte)OpCode.KeepAlive;
			Deserialize(reader);
		}

		public override void Serialize(ref DataStreamWriter writer)
		{
			writer.WriteByte(Code);
		}

		public override void Deserialize(DataStreamReader reader)
		{
			// Already read code byte
		}
	}
}
