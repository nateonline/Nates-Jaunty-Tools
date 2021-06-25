using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;

namespace NatesJauntyTools
{
	[CustomEditor(typeof(Script), true)]
	[CanEditMultipleObjects]
	public class Script_Editor : Editor
	{
		#region General

		Script script;

		List<FieldInfo> defaultInspectorFields = new List<FieldInfo>();

		void LoadData()
		{
			script = (Script)target;

			defaultInspectorFields.Clear();
			defaultInspectorFields.AddRange(script.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public).ToList());
			defaultInspectorFields.AddRange(script.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(f => f.GetCustomAttribute(typeof(SerializeField)) != null));
		}

		public override void OnInspectorGUI()
		{
			LoadData();

			base.OnInspectorGUI();

			DrawInspectorButtons();
		}

		void OnSceneGUI()
		{
			LoadData();

			// DrawPositionHandles();
		}

		string PropertyID(SerializedProperty property)
		{
			return $"{script.GetInstanceID()} {property.name}";
		}

		#endregion


		#region Button

		void DrawInspectorButtons()
		{
			MethodInfo[] inspectorButtonMethods = script.GetType().GetMethods().Where(m => m.GetCustomAttribute(typeof(InspectorButton)) != null).ToArray();

			if (inspectorButtonMethods.Length > 0)
			{
				EditorGUILayout.LabelField("Button Methods", EditorStyles.boldLabel);

				foreach (MethodInfo method in inspectorButtonMethods)
				{
					InspectorButton inspectorButtonAttribute = (InspectorButton)method.GetCustomAttribute(typeof(InspectorButton));
					EditorGUI.BeginDisabledGroup(inspectorButtonAttribute.editMode == false && Application.isPlaying == false);
					if (GUILayout.Button(method.Name.SplitCamelCase()))
					{
						method.Invoke(script, null);
					}
					EditorGUI.EndDisabledGroup();
				}
			}
		}

		#endregion


		#region Vector Handles

		// void DrawPositionHandles()
		// {
		// 	FieldInfo[] positionHandles = script.GetType().GetFields().Where(f => f.GetCustomAttribute(typeof(PositionHandle)) != null).ToArray();

		// 	if (positionHandles.Length > 0)
		// 	{
		// 		SerializedObject scriptObject = new SerializedObject(script);

		// 		Handles.Label(script.transform.position, "Transform");

		// 		foreach (FieldInfo field in positionHandles)
		// 		{
		// 			PositionHandle positionHandle = (PositionHandle)field.GetCustomAttribute(typeof(PositionHandle));
		// 			SerializedProperty property = scriptObject.FindProperty(field.Name);

		// 			if (property.type == "Vector3")
		// 			{
		// 				EditorGUI.BeginChangeCheck();

		// 				Vector3 newPosition;
		// 				if (positionHandle.ignoreObjectPosition)
		// 				{
		// 					newPosition = Handles.PositionHandle((Vector3)field.GetValue(script), Quaternion.identity);
		// 					Handles.Label((Vector3)field.GetValue(script), field.Name);
		// 				}
		// 				else
		// 				{
		// 					newPosition = Handles.PositionHandle(script.transform.position + (Vector3)field.GetValue(script), Quaternion.identity);
		// 					Handles.Label(script.transform.position + (Vector3)field.GetValue(script), field.Name);
		// 				}

		// 				if (EditorGUI.EndChangeCheck())
		// 				{
		// 					field.SetValue(script, newPosition);
		// 				}
		// 			}
		// 		}
		// 	}
		// }

		#endregion
	}
}
