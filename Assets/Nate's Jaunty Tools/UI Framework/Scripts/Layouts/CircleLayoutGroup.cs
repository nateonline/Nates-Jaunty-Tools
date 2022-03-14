using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NatesJauntyTools;

public class CircleLayoutGroup : LayoutGroup
{
	[Header("Circle Settings")]
	public Vector2 cellSize;
	public bool clockwise = true;
	[Range(0, 360)] public float originAngle;
	[Range(0, 360)] public float endAngle = 360;
	public float radiusInset;
	public bool controlChildRotation;


	public override void CalculateLayoutInputHorizontal()
	{
		base.CalculateLayoutInputHorizontal();

		Vector2 parentSize = new Vector2(
			rectTransform.rect.width,
 			rectTransform.rect.height
		);

		float radius = Mathf.Min(parentSize.x, parentSize.y) / 2f;
		float angleStep = endAngle / (float)rectChildren.Count;
		const float ANGLE_TO_RADIANS = Mathf.PI / 180f;

		if (clockwise)
		{
			for (int i = 0; i < rectChildren.Count; i++) { SetPosition(i); }
		}
		else
		{
			for (int i = 0; i < rectChildren.Count; i++) { SetPosition(rectChildren.Count - 1 - i); }
		}

		void SetPosition(int childIndex)
		{
			float childAngle = (angleStep * childIndex) + (originAngle + 180);

			Vector2 cellPosition = new Vector2(
				(clockwise ? -1f : 1f) * (Mathf.Sin(childAngle * ANGLE_TO_RADIANS) * (radius - radiusInset))
				+ (parentSize.x / 2f)
				- (cellSize.x / 2f)
				,
				(Mathf.Cos(childAngle * ANGLE_TO_RADIANS) * (radius - radiusInset))
				+ (parentSize.y / 2f)
				- (cellSize.y / 2f)
			);
			SetChildAlongAxis(rectChildren[childIndex], 0, cellPosition.x, cellSize.x);
			SetChildAlongAxis(rectChildren[childIndex], 1, cellPosition.y, cellSize.y);
			if (controlChildRotation)
			{
				Vector3 rotation = rectChildren[childIndex].eulerAngles;
				rotation.z = ((angleStep * childIndex) + originAngle) * -1;
				rectChildren[childIndex].eulerAngles = rotation;
			}
		}
	}

	public override void CalculateLayoutInputVertical() { }

	public override void SetLayoutHorizontal() { }

	public override void SetLayoutVertical() { }
}
