using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;

namespace NatesJauntyTools.NetCode
{
	public class BaseMessage
	{
		public byte Code { get; set; }

		public virtual void Serialize(ref DataStreamWriter writer) {}
		public virtual void Deserialize(DataStreamReader reader) {}

		public virtual void ReceivedOnClient() {}
		public virtual void ReceivedOnServer() {}
	}
}
