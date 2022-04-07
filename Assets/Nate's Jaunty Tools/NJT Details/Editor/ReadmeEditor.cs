using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NatesJauntyTools.Details
{
	[CustomEditor(typeof(ReadmeAsset), true)]
	public class ReadmeEditor : Editor
	{
		ReadmeAsset readme;

		public override void OnInspectorGUI()
		{
			readme = (ReadmeAsset)target;
			readme.ApplyData();

			DrawTitle();

			DrawVersions();
			EditorGUILayout.Space(30);
			DrawChangeLog();
		}

		void DrawTitle()
		{
			GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
			titleStyle.fontSize = 20;
			titleStyle.fontStyle = FontStyle.Bold;
			titleStyle.alignment = TextAnchor.MiddleCenter;

			EditorGUILayout.Space(10);
			EditorGUILayout.LabelField("Read Me", titleStyle);
			EditorGUILayout.Space(10);
		}

		void DrawVersions()
		{
			GUIStyle greyText = new GUIStyle(GUI.skin.label);
			greyText.normal.textColor = new Color32(96, 100, 103, 255);
			
			EditorGUILayout.LabelField("Version", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(readme.versionNumber);
			EditorGUILayout.LabelField($"Built in Unity {readme.unityVersion}", greyText);
			EditorGUILayout.EndHorizontal();
		}

		void DrawChangeLog()
		{
			EditorGUILayout.LabelField("Change Log", EditorStyles.boldLabel);
			EditorGUILayout.Space(5);
			foreach (ChangeLogItem item in readme.changeLog)
			{
				DrawChangeLogItem(item);
				EditorGUILayout.Space(5);
			}
		}

		public void DrawChangeLogItem(ChangeLogItem item)
		{
			GUIStyle style = new GUIStyle(GUI.skin.label);
			style.wordWrap = true;

			switch (item.type)
			{
				case ChangeType.Added:
					style.normal.textColor = "#0AC81E".HexToColor();
					EditorGUILayout.LabelField($"+  {item.description}", style);
					break;

				case ChangeType.Deleted:
					style.normal.textColor = "#DC1414".HexToColor();
					EditorGUILayout.LabelField($"-  {item.description}", style);
					break;

				case ChangeType.Modified:
					style.normal.textColor = "#148CE6".HexToColor();
					EditorGUILayout.LabelField($"*  {item.description}", style);
					break;
				
				case ChangeType.Spacer:
					EditorGUILayout.Space(5);
					break;

				default:
					break;
			}
		}
	}
}
