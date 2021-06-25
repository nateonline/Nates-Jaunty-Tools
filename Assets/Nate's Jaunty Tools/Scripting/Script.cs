using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NatesJauntyTools
{
	/// <summary> Adds most of Siege Engine's scripting functionality. </summary> 
	/// <remarks> Includes things like inspector buttons and the InitializeComponent() function. </remarks>
	public class Script : MonoBehaviour
	{
		#region Initialization

		public T InitializeComponent<T>()
		{
			T component = GetComponent<T>();
			if (component.ToString() == "null") { component = default(T); }

			if (Equals(component, default(T)))
			{
				Debug.LogWarning($"No <b><color=#c99700>{typeof(T).ToString()}</color></b> component found", gameObject);
				return default(T);
			}
			else
			{
				return component;
			}
		}

		public T ForceInitializeComponent<T>() where T : Component
		{
			T component = GetComponent<T>();
			if (component.ToString() == "null") { component = gameObject.AddComponent<T>(); }
			return component;
		}

		public Component InitializeComponentInChildren<Component>()
		{
			Component component = GetComponentInChildren<Component>();
			if (component.ToString() == "null") { component = default(Component); }

			if (Equals(component, default(Component)))
			{
				Debug.LogWarning($"No <b><color=#c99700>{typeof(Component).ToString()}</color></b> component found in children", gameObject);
				return default(Component);
			}
			else
			{
				return component;
			}
		}

		public Component InitializeComponentInParent<Component>()
		{
			Component component = GetComponentInParent<Component>();
			if (component.ToString() == "null") { component = default(Component); }

			if (Equals(component, default(Component)))
			{
				Debug.LogWarning($"No <b><color=#c99700>{typeof(Component).ToString()}</color></b> component found in parent", gameObject);
				return default(Component);
			}
			else
			{
				return component;
			}
		}

		#endregion
	}
}
