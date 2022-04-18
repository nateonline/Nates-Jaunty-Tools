using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NatesJauntyTools.GoogleSheets
{
	// public class GoogleSheetImporter : EditorWindow
	// {
	// 	GoogleSheetLink gsLink;
	// 	ScriptableObject soExample;
	// 	string path;

	// 	[MenuItem("Nate's Jaunty Tools/Import/Scriptable Objects from Google Sheet")]
	// 	private static void ShowWindow()
	// 	{
	// 		var window = GetWindow<GoogleSheetImporter>();
	// 		window.titleContent = new GUIContent("Google Sheet Importer");
	// 		window.Show();
	// 	}

	// 	private void OnGUI()
	// 	{
	// 		gsLink = (GoogleSheetLink)EditorGUILayout.ObjectField("Google Sheet Link", gsLink, typeof(GoogleSheetLink), allowSceneObjects: false);
	// 		soExample = (ScriptableObject)EditorGUILayout.ObjectField("SO Type", soExample, typeof(ScriptableObject), allowSceneObjects: false);
	// 		path = (string)EditorGUILayout.TextField("Import Path", path);

	// 		if (GUILayout.Button("Test Log"))
	// 		{
	// 			Debug.Log($"GS Link initialized = {gsLink.IsSetup}");
	// 			Debug.Log($"SO Type = {soExample.GetType()}");
	// 			Debug.Log($"Path = {path}");
	// 		}
	// 	}
	// }
}
