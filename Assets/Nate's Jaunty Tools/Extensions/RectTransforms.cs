using UnityEngine;

namespace NatesJauntyTools
{
	public static partial class Tools
	{
		/// <summary> (Currently only works with the old input system) </summary>
		public static bool ContainsMouse(this RectTransform rect)
		{
			Vector2 mouse = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			return RectTransformUtility.RectangleContainsScreenPoint(rect, mouse);
		}
	}
}