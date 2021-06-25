using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NatesJauntyTools
{
	[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
	public class ReadOnlyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			EditorGUI.BeginDisabledGroup(true);
			EditorGUI.PropertyField(position, prop);
			EditorGUI.EndDisabledGroup();
		}
	}
}
