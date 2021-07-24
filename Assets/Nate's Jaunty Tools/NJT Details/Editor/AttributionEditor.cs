using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NatesJauntyTools.Details
{
	[CustomEditor(typeof(AttributionAsset))]
	public class AttributionEditor : Editor
	{
		AttributionAsset attribution;

		public override void OnInspectorGUI()
		{
			attribution = (AttributionAsset)target;
			attribution.ApplyData();

			DrawTitle();

			foreach (AttributionItem item in attribution.attributionItems)
			{
				DrawAttributionItem(item);
				EditorGUILayout.Space(10);
			}
		}

		void DrawTitle()
		{
			GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
			titleStyle.fontSize = 20;
			titleStyle.fontStyle = FontStyle.Bold;
			titleStyle.alignment = TextAnchor.MiddleCenter;

			EditorGUILayout.Space(10);
			EditorGUILayout.LabelField("Attribution", titleStyle);
			EditorGUILayout.Space(10);
		}
		
		public void DrawAttributionItem(AttributionItem item)
		{
			GUIStyle boldStyle = new GUIStyle(GUI.skin.label);

			EditorGUILayout.LabelField(item.name, EditorStyles.boldLabel);
			if (!System.String.IsNullOrEmpty(item.description))
			{
				GUIStyle wrapStyle = new GUIStyle(GUI.skin.label);
				wrapStyle.wordWrap = true;
				EditorGUILayout.LabelField(item.description, wrapStyle);
			}
			
			EditorGUILayout.BeginHorizontal();

			if (System.String.IsNullOrEmpty(item.sourceURL)) { EditorGUILayout.LabelField("No Source"); }
			else { DrawHyperlink("Source", item.sourceURL); }

			if (System.String.IsNullOrEmpty(item.authorURL)) { EditorGUILayout.LabelField(item.authorName); }
			else { DrawHyperlink(item.authorName, item.authorURL); }

			if (System.String.IsNullOrEmpty(item.licenseURL)) { EditorGUILayout.LabelField(item.licenseName); }
			else { DrawHyperlink(item.licenseName, item.licenseURL); }

			EditorGUILayout.EndHorizontal();
		}

		void DrawHyperlink(string text, string url)
		{
			var hyperlinkStyle = new GUIStyle(GUI.skin.button);
			hyperlinkStyle.normal.textColor = "#00CCFF".HexToColor();
			
			bool button;
			if (text == "Source")
			{
				button = GUILayout.Button(text, hyperlinkStyle, GUILayout.Width(70));
			}
			else
			{
				button = GUILayout.Button(text, hyperlinkStyle);
			}

			if (button) { Application.OpenURL(url); }
		}
	}
}
