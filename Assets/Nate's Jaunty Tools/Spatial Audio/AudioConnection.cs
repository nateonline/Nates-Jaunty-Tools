using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NatesJauntyTools.SpatialAudio
{
	public class AudioConnection : Script
	{
		public AudioTransmitter transmitter;
		public AudioReceiver receiver;
		public AudioSource source;

		void Update()
		{
			UpdateSourceData();
			UpdateSourceTransform();
		}

		public void Initialize(AudioTransmitter t, AudioReceiver r, AudioSource s)
		{
			transmitter = t;
			receiver = r;
			source = s;

			name = $"{receiver.name} <= {transmitter.name}";

			source.dopplerLevel = 0f;

			UpdateSourceData();

			if (source.playOnAwake) { source.Play(); }
		}

		void UpdateSourceData()
		{
			if (source != null)
			{
				source.clip = transmitter.clip;
				source.volume = transmitter.volume;
				source.loop = transmitter.loop;
				source.maxDistance = 1f;
				source.minDistance = transmitter.DistanceRatio();
				source.playOnAwake = transmitter.playOnAwake;
				source.spatialBlend = 1f - SurroundProgress(); // because spatial blend should be 0 when surround is 1
			}
		}

		void UpdateSourceTransform()
		{
			transform.rotation = Quaternion.Inverse(receiver.transform.localRotation);

			Vector3 audioSourcePosition = (transmitter.transform.position - receiver.transform.position) / transmitter.volumeStart;

			if (audioSourcePosition.magnitude > 2)
			{
				audioSourcePosition = audioSourcePosition.normalized * 2f;
			}

			source.transform.localPosition = audioSourcePosition;
		}

		AudibleSegment GetAudibleSegment()
		{
			float currentDistance = Vector3.Distance(transmitter.transform.position, receiver.transform.position);

			if (currentDistance <= transmitter.surroundPeak) { return AudibleSegment.SurroundMax; }
			else if (currentDistance <= transmitter.surroundStart) { return AudibleSegment.SurroundScaled; }
			else if (currentDistance <= transmitter.volumePeak) { return AudibleSegment.VolumeMax; }
			else if (currentDistance <= transmitter.volumeStart) { return AudibleSegment.VolumeScaled; }
			else { return AudibleSegment.Inaudible; }
		}

		public float VolumeProgress()
		{
			return 1f - (Vector3.Distance(receiver.transform.position, transmitter.transform.position) - transmitter.volumePeak) / (transmitter.volumeStart - transmitter.volumePeak);
		}

		public float SurroundProgress()
		{
			return 1f - (Vector3.Distance(receiver.transform.position, transmitter.transform.position) - transmitter.surroundPeak) / (transmitter.surroundStart - transmitter.surroundPeak);
		}
	}
}
