using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

class UIPrefabCreator
{
	[MenuItem("GameObject/UI/NJT Smart Button")]
	static void CreateSmartButton()
	{
		GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Nate's Jaunty Tools/UI Framework/Smart Button.prefab");
		GameObject newInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, Selection.activeTransform);
		Undo.RegisterCreatedObjectUndo(newInstance, "Created Smart Button");
	}
}
