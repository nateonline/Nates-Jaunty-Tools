using System.Collections.Generic;
using UnityEngine;

namespace NatesJauntyTools
{
	public static partial class Tools
	{
		/// <summary> Change layer of the given transform and all of its children. </summary>
		/// <param name="layer">layer in Integer form</param>
		public static void ChangeLayerDeep(this Transform root, int layer)
		{
			root.gameObject.layer = layer;
			foreach (Transform child in root)
			{
				ChangeLayerDeep(child, layer);
			}
		}

		public static Component FindComponent<T>(this Transform root, string search, string previousTabs = "") where T : Component
		{
			// Debug.Log($"{previousTabs}{root.name}");

			Component thisComponent = root.GetComponent<T>();
			if (root.name == search && thisComponent)
			{
				return thisComponent;
			}
			else if (root.childCount > 0)
			{
				foreach (Transform child in root)
				{
					Component x = child.FindComponent<T>(search, previousTabs + "\t");
					if (x) return x;
				}
			}

			return default(T);
		}

		public static List<Transform> GetChildren(this Transform parent)
		{
			List<Transform> children = new List<Transform>();

			foreach (Transform child in parent)
			{
				children.Add(child.transform);
			}

			return children;
		}

		public static T GetComponentInChildrenOnly<T>(this Transform parent) where T : Component
		{
			foreach (Transform child in parent.GetChildren())
			{
				if (child.TryGetComponent<T>(out T component)) return (component);
			}

			return null;
		}

		public static List<T> GetComponentsInChildrenOnly<T>(this Transform parent) where T : Component
		{
			List<T> componentsToReturn = new List<T>();

			foreach (Transform child in parent.GetChildren())
			{
				if (child.TryGetComponent<T>(out T component)) componentsToReturn.Add(component);
			}

			return componentsToReturn;
		}
	}
}
