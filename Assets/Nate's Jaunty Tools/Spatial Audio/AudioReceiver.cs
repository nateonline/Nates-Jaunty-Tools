using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NatesJauntyTools.SpatialAudio
{
	public class AudioReceiver : Script
	{
		void OnEnable() { AudioRelay._.AddReceiver(this); }
		void OnDisable() { AudioRelay._.RemoveReceiver(this); }
	}
}
