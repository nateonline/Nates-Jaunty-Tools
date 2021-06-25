using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NatesJauntyTools.SpatialAudio
{
	public class AudioTransmitter : Script
	{
		void OnEnable() { AudioRelay._.AddTransmitter(this); }
		void OnDisable() { AudioRelay._.RemoveTransmitter(this); }

		void OnValidate() { ValidateDistances(); }


		public float volumeStart = 20f;
		public float volumePeak = 15f;
		public float surroundStart = 10f;
		public float surroundPeak = 5f;

		public AudioClip clip;
		[Range(0, 1)] public float volume = 0.5f;
		public bool loop;
		public bool playOnAwake;


		public void ValidateDistances()
		{
			if (volumePeak > volumeStart) { volumePeak = volumeStart; }
			if (surroundPeak > surroundStart) { surroundPeak = surroundStart; }
		}

		public float DistanceRatio()
		{
			return volumePeak / volumeStart;
		}
	}
}
