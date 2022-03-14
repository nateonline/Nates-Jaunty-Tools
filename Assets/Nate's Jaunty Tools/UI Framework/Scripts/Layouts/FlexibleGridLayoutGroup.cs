using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NatesJauntyTools;

public class FlexibleGridLayoutGroup : LayoutGroup
{
	public Vector2 spacing;

	public enum FitType { Stretch, FixedCount, FixedSize }

	[Header("Flexible Grid Settings")]
	public FitType rowFit;
	public FitType columnFit;

	public int rows; float RowsF => (float)rows;
	public int columns; float ColumnsF => (float)columns;

	public Vector2 cellSize;

	// Calculate, as opposed to using "cellSize" straight from the inspector
	bool calculateCellWidth;
	bool calculateCellHeight;


	public override void CalculateLayoutInputHorizontal()
	{
		base.CalculateLayoutInputHorizontal();

		if (rowFit == FitType.Stretch && columnFit == FitType.Stretch)
		{
			float sqrRt = Mathf.Sqrt(transform.childCount);
			rows = Mathf.CeilToInt(sqrRt);
			columns = Mathf.CeilToInt(sqrRt);
			calculateCellWidth = true;
			calculateCellHeight = true;
		}
		else
		{
			switch (rowFit)
			{
				case FitType.Stretch:
					calculateCellHeight = true;
					rows = Mathf.CeilToInt(transform.childCount / ColumnsF);
					break;

				case FitType.FixedCount:
					calculateCellHeight = true;
					break;

				case FitType.FixedSize:
					calculateCellHeight = false;
					rows = Mathf.CeilToInt(transform.childCount / ColumnsF);
					rectTransform.SetHeight(
						(cellSize.y * rows)
						+ (spacing.y * (rows - 1))
						+ padding.top
						+ padding.bottom
					);
					break;
			}

			switch (columnFit)
			{
				case FitType.Stretch:
					calculateCellWidth = true;
					columns = Mathf.CeilToInt(transform.childCount / RowsF);
					break;

				case FitType.FixedCount:
					calculateCellWidth = true;
					break;

				case FitType.FixedSize:
					calculateCellWidth = false;
					columns = Mathf.CeilToInt(transform.childCount / RowsF);
					rectTransform.SetWidth(
						(cellSize.x * columns)
						+ (spacing.x * (columns - 1))
						+ padding.left
						+ padding.right
					);
					break;
			}
		}

		Vector2 parentSize = new Vector2(
			rectTransform.rect.width,
 			rectTransform.rect.height
		);

		Vector2 calculatedCellSize = new Vector2(
			(parentSize.x / ColumnsF)
			- ((spacing.x / ColumnsF) * (columns - 1))
			- (padding.left / ColumnsF)
			- (padding.right / ColumnsF),

 			(parentSize.y / RowsF)
			- ((spacing.y / RowsF) * (rows - 1))
			- (padding.top / RowsF)
			- (padding.bottom / RowsF)
		);

		cellSize.x = (calculateCellWidth) ? calculatedCellSize.x : cellSize.x;
		cellSize.y = (calculateCellHeight) ? calculatedCellSize.y : cellSize.y;

		int columnCount = 0;
		int rowCount = 0;

		for (int i = 0; i < rectChildren.Count; i++)
		{
			rowCount = (columns != 0) ? i / columns : 1;
			columnCount = (columns != 0) ? i % columns : 1;

			Vector2 cellPosition = new Vector2(
				(cellSize.x * columnCount)
				+ (spacing.x * columnCount)
				+ padding.left,

				(cellSize.y * rowCount)
				+ (spacing.y * rowCount)
				+ padding.top
			);
			SetChildAlongAxis(rectChildren[i], 0, cellPosition.x, cellSize.x);
			SetChildAlongAxis(rectChildren[i], 1, cellPosition.y, cellSize.y);
		}
	}

	public override void CalculateLayoutInputVertical() { }

	public override void SetLayoutHorizontal() { }

	public override void SetLayoutVertical() { }
}
