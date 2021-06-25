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
	}
}
