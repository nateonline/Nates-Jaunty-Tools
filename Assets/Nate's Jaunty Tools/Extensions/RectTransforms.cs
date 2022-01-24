using UnityEngine;
using UnityEngine.UI;

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

		public static void ScrollToTop(this ScrollRect scrollRect)
		{
			Canvas.ForceUpdateCanvases();
			scrollRect.normalizedPosition = new Vector2(0, 1);
		}

		public static void ScrollToBottom(this ScrollRect scrollRect)
		{
			Canvas.ForceUpdateCanvases();
			scrollRect.normalizedPosition = new Vector2(0, 0);
		}

		public static void SetLeft(this RectTransform rt, float left) { rt.offsetMin = new Vector2(left, rt.offsetMin.y); }

		public static void SetRight(this RectTransform rt, float right) { rt.offsetMax = new Vector2(-right, rt.offsetMax.y); }

		public static void SetTop(this RectTransform rt, float top) { rt.offsetMax = new Vector2(rt.offsetMax.x, -top); }

		public static void SetBottom(this RectTransform rt, float bottom) { rt.offsetMin = new Vector2(rt.offsetMin.x, bottom); }

		public static void SetWidth(this RectTransform rt, float width) { rt.sizeDelta = new Vector2(width, rt.sizeDelta.y); }

		public static void SetHeight(this RectTransform rt, float height) { rt.sizeDelta = new Vector2(rt.sizeDelta.x, height); }
	}
}