using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NatesJauntyTools.SpatialAudio
{
	[CustomEditor(typeof(AudioRelay)), CanEditMultipleObjects]
	public class AudioRelayEditor : Editor
	{
		float dottedLineSize = 3f;
		float dividerScale = 0.5f;


		void OnSceneGUI()
		{
			AudioRelay thisAudioRelay = (target as AudioRelay);

			Handles.color = SpatialAudioColors.Inaudible;
			Handles.RadiusHandle(Quaternion.identity, thisAudioRelay.transform.position, 2f);

			Handles.color = SpatialAudioColors.VolumeScaled;
			Handles.RadiusHandle(Quaternion.identity, thisAudioRelay.transform.position, 1f);

			foreach (AudioConnection connection in thisAudioRelay.connections)
			{
				Vector3 relay_position = thisAudioRelay.transform.position;
				Vector3 relay_target = connection.source.transform.position;
				Vector3 relay_volumeStart = LerpByDistance(relay_position, relay_target, connection.source.maxDistance);
				Vector3 relay_volumePeak = LerpByDistance(relay_position, relay_target, connection.source.minDistance);

				Vector3 transmit_position = connection.transmitter.transform.position;
				Vector3 transmit_target = connection.receiver.transform.position;
				float transmit_distance = Vector3.Distance(transmit_position, transmit_target);
				Vector3 transmit_volumeStart = LerpByDistance(transmit_position, transmit_target, connection.transmitter.volumeStart);
				Vector3 transmit_volumePeak = LerpByDistance(transmit_position, transmit_target, connection.transmitter.volumePeak);
				Vector3 transmit_surroundStart = LerpByDistance(transmit_position, transmit_target, connection.transmitter.surroundStart);
				Vector3 transmit_surroundPeak = LerpByDistance(transmit_position, transmit_target, connection.transmitter.surroundPeak);

				if ((connection.VolumeProgress() <= 0 && thisAudioRelay.DebugInaudible) || connection.VolumeProgress() > 0)
				{
					Handles.color = SpatialAudioColors.Inaudible;
					if (transmit_distance > connection.transmitter.volumeStart)
					{
						Handles.DrawDottedLine(relay_target, relay_volumeStart, dottedLineSize);
						if (thisAudioRelay.DebugInWorld) { Handles.DrawDottedLine(transmit_target, transmit_volumeStart, dottedLineSize); }
					}

					Handles.color = SpatialAudioColors.VolumeScaled;
					if (transmit_distance > connection.transmitter.volumeStart)
					{
						Handles.DrawLine(relay_volumeStart, relay_volumePeak);
						if (thisAudioRelay.DebugInWorld)
						{
							Handles.DrawLine(transmit_volumeStart, transmit_volumePeak);
							DrawPerpendicularDivider(transmit_volumeStart, transmit_position);
						}
					}
					else if (transmit_distance > connection.transmitter.volumePeak)
					{
						Handles.DrawLine(relay_target, relay_volumePeak);
						if (thisAudioRelay.DebugInWorld) { Handles.DrawLine(transmit_target, transmit_volumePeak); }
					}

					Handles.color = SpatialAudioColors.VolumeMax;
					if (transmit_distance > connection.transmitter.volumePeak)
					{
						Handles.DrawLine(relay_volumePeak, relay_position);
						DrawPerpendicularDivider(relay_volumePeak, relay_position);
						if (thisAudioRelay.DebugInWorld)
						{
							Handles.DrawLine(transmit_volumePeak, transmit_position);
							DrawPerpendicularDivider(transmit_volumePeak, transmit_position);
						}
					}
					else
					{
						Handles.DrawLine(relay_target, relay_position);
						if (thisAudioRelay.DebugInWorld) { Handles.DrawLine(transmit_target, transmit_position); }
					}
				}

				if ((connection.SurroundProgress() <= 0 && thisAudioRelay.DebugInaudible) || connection.SurroundProgress() > 0)
				{
					if (thisAudioRelay.DebugInWorld)
					{
						Handles.color = SpatialAudioColors.SurroundScaled;
						if (transmit_distance > connection.transmitter.surroundStart)
						{
							DrawPerpendicularDivider(transmit_surroundStart, transmit_position);
						}

						Handles.color = SpatialAudioColors.SurroundMax;
						if (transmit_distance > connection.transmitter.surroundPeak)
						{
							DrawPerpendicularDivider(transmit_surroundPeak, transmit_position);
						}
					}
				}
			}
		}

		Vector3 LerpByDistance(Vector3 A, Vector3 B, float x)
		{
			Vector3 P = x * Vector3.Normalize(B - A) + A;
			return P;
		}

		void DrawPerpendicularDivider(Vector3 position, Vector3 center)
		{
			Vector3 direction = position - center;
			Vector3 normal = position + direction.normalized * dividerScale * (SceneView.currentDrawingSceneView.size * 0.1f);
			Vector3 left = normal.RotateAroundPivot(position, 90);
			Vector3 right = normal.RotateAroundPivot(position, 180); // I have no idea man. This should be -90 or 270, but neither works and 180 works so...

			Handles.DrawLine(position, left);
			Handles.DrawLine(position, right);
		}
	}
}
