using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;

namespace NatesJauntyTools.NetCode
{
	public abstract class BaseMessage
	{
		public byte Code { get; set; }
		
		public abstract void Serialize(ref DataStreamWriter writer);
		public abstract void Deserialize(DataStreamReader reader);
	}
}
