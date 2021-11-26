using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using NatesJauntyTools;

namespace NatesJauntyTools.NetCode
{
	public abstract class NetPoint : Script
	{
		[ReadOnly] public bool IsInitialized;
		protected NetworkDriver driver;

		[SerializeField] protected bool includeNetLogsInConsole = true;

		protected void Update() => Upkeep();
		protected void OnDestroy() => Shutdown();

		public abstract void Startup();
		public abstract void Shutdown();
		public abstract void Upkeep();

		protected abstract void OnData(DataStreamReader reader);

		protected void NetLog(string message)
		{
			if (includeNetLogsInConsole) { Debug.Log(message, this); }
			OnNetLog?.Invoke(message);
		}

		public event Action<string> OnNetLog;
	}
}
