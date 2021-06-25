using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NatesJauntyTools.SpatialAudio
{
	[CustomEditor(typeof(AudioTransmitter)), CanEditMultipleObjects]
	public class AudioTransmitterEditor : Editor
	{
		AudioTransmitter targetTransmitter;


		void OnSceneGUI()
		{
			targetTransmitter = (target as AudioTransmitter);

			EditorGUI.BeginChangeCheck();

			Handles.color = SpatialAudioColors.VolumeScaled;
			float volumeStart = Handles.RadiusHandle(Quaternion.identity, targetTransmitter.transform.position, targetTransmitter.volumeStart);

			Handles.color = SpatialAudioColors.VolumeMax;
			float volumePeak = Handles.RadiusHandle(Quaternion.identity, targetTransmitter.transform.position, targetTransmitter.volumePeak);

			Handles.color = SpatialAudioColors.SurroundScaled;
			float surroundStart = Handles.RadiusHandle(Quaternion.identity, targetTransmitter.transform.position, targetTransmitter.surroundStart);

			Handles.color = SpatialAudioColors.SurroundMax;
			float surroundPeak = Handles.RadiusHandle(Quaternion.identity, targetTransmitter.transform.position, targetTransmitter.surroundPeak);

			targetTransmitter.ValidateDistances();

			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(target, "Changed Distances");
				targetTransmitter.volumeStart = volumeStart;
				targetTransmitter.volumePeak = volumePeak;
				targetTransmitter.surroundStart = surroundStart;
				targetTransmitter.surroundPeak = surroundPeak;
			}

			DrawPositionalRange(targetTransmitter.volumeStart, targetTransmitter.volumePeak, 150, false, SpatialAudioColors.VolumeScaled, "Volume");
			DrawPositionalRange(targetTransmitter.surroundStart, targetTransmitter.surroundPeak, 120, false, SpatialAudioColors.SurroundScaled, "Surround");
		}

		void DrawPositionalRange(float radius1, float radius2, float offset) { DrawPositionalRange(radius1, radius2, offset, false, Color.white, ""); }

		void DrawPositionalRange(float radius1, float radius2, float offset, bool rotateWithView, Color32 color, string label)
		{
			Vector3 radius1pos = targetTransmitter.transform.position + new Vector3(radius1, 0, 0);
			Vector3 radius2pos = targetTransmitter.transform.position + new Vector3(radius2, 0, 0);

			Handles.color = color;

			if (rotateWithView)
			{
				float sceneRotationY = SceneView.currentDrawingSceneView.rotation.eulerAngles.y;
				radius1pos.RotateAroundPivot(targetTransmitter.transform.position, new Vector3(0, sceneRotationY + offset, 0));
				radius2pos.RotateAroundPivot(targetTransmitter.transform.position, new Vector3(0, sceneRotationY + offset, 0));
			}
			else
			{
				radius1pos.RotateAroundPivot(targetTransmitter.transform.position, new Vector3(0, offset, 0));
				radius2pos.RotateAroundPivot(targetTransmitter.transform.position, new Vector3(0, offset, 0));
			}

			Handles.DrawLine(radius1pos, radius2pos);

			Handles.Label((radius1pos + radius2pos) / 2, label);
		}
	}
}
