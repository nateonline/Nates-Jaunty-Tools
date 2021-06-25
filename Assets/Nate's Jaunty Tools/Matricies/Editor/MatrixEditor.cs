using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NatesJauntyTools
{
	public abstract class MatrixEditor<T> : Editor
	{
		Matrix<T> matrix;
		const float HEADER_SPACER = 10f;
		protected abstract Vector2 CellSize { get; }
		
		public override void OnInspectorGUI()
		{
			matrix = (Matrix<T>)target;

			matrix.type = (MatrixType)EditorGUILayout.EnumPopup("Matrix Type", matrix.type);
			if (matrix.type == MatrixType.Separated)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("rowTitles"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("columnTitles"));
			}
			else if (matrix.type == MatrixType.Reflected)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("reflectedListTitles"));
			}
			else
			{
				EditorGUILayout.HelpBox("Matrix type not handled!", MessageType.Error);
			}

			ShowDuplicateWarnings();

			var oldMatrix = ReinitializeMatrix();

			if (matrix.ColumnTitles.Count > 0 && matrix.RowTitles.Count > 0)
			{
				FillValues(oldMatrix, ref matrix);
				
				Rect matrixArea = GUILayoutUtility.GetRect(10, 10000, 200, 10000);
				GUI.BeginClip(matrixArea);

				DrawColumns();
				DrawRows();
				DrawCells();

				GUI.EndClip();
			}
			else
			{
				EditorGUILayout.HelpBox("There are no cells because there aren't enough rows/columns.", MessageType.Info);
			}

			if(GUI.changed) { EditorUtility.SetDirty(matrix); }
		}

		Matrix<T> ReinitializeMatrix()
		{
			var oldMatrix = Instantiate(matrix);
			matrix.data = new MatrixRow<T>[matrix.RowTitles.Count];
			for (int i = 0; i < matrix.RowTitles.Count; i++)
			{
				matrix.data[i] = new MatrixRow<T>(matrix.ColumnTitles.Count);
			}
			return oldMatrix;
		}

		void FillValues(Matrix<T> fillFrom, ref Matrix<T> fillTo)
		{
			Vector2Int newSize = new Vector2Int(
				Mathf.Min(fillFrom.ColumnTitles.Count, fillTo.ColumnTitles.Count),
				Mathf.Min(fillFrom.RowTitles.Count, fillTo.RowTitles.Count)
			);
			
			for (int col = 0; col < newSize.x; col++)
			{
				for (int row = 0; row < newSize.y; row++)
				{
					fillTo.data[row].cells[col] = fillFrom.data[row].cells[col];
				}
			}
		}

		void ShowDuplicateWarnings()
		{
			List<string> rowDuplicates = Duplicates(matrix.RowTitles);
			if (rowDuplicates != null)
			{
				string message = (rowDuplicates.Count == 1) ? "There is a duplicate row title" : "There are duplicate row titles";

				EditorGUILayout.HelpBox($"{message}: {String.Join(", ", rowDuplicates)}\nAny text searches will return the first match.", MessageType.Warning);
			}

			List<string> columnDuplicates = Duplicates(matrix.ColumnTitles);
			if (columnDuplicates != null)
			{
				string message = (columnDuplicates.Count == 1) ? "There is a duplicate column title" : "There are duplicate column titles";

				EditorGUILayout.HelpBox($"{message}: {String.Join(", ", columnDuplicates)}\nAny text searches will return the first match.", MessageType.Warning);
			}
		}
		
		float RowHeaderWidth
		{
			get
			{
				float width = 0;
				foreach (var rowTitle in matrix.RowTitles)
				{
					width = Mathf.Max(width, EditorStyles.label.CalcSize(new GUIContent(rowTitle)).x + 1f);
				}
				return width;
			}
		}
		
		float ColumnHeaderHeight
		{
			get
			{
				float height = 0;
				foreach (var columnTitle in matrix.ColumnTitles)
				{
					height = Mathf.Max(height, EditorStyles.label.CalcSize(new GUIContent(columnTitle)).x + 1f);
				}
				return height;
			}
		}

		void DrawColumns()
		{
			const float ANGLE = 90f;
			
			float xMove = (RowHeaderWidth + HEADER_SPACER) - (ColumnHeaderHeight - (8f));
			foreach (var title in matrix.ColumnTitles)
			{
				Rect columnLabelRect = new Rect(xMove, ColumnHeaderHeight, ColumnHeaderHeight, CellSize.y);
				Vector2 rotatePoint = new Vector2(
					columnLabelRect.x + columnLabelRect.size.x,
					columnLabelRect.center.y
				);
				// EditorGUI.DrawRect(new Rect(rotatePoint, new Vector2(1, 1)), Color.white);
				
				var centeredStyle = GUI.skin.GetStyle("Label");
				centeredStyle.alignment = TextAnchor.MiddleRight;
				EditorGUIUtility.RotateAroundPivot(ANGLE, rotatePoint);
				EditorGUI.LabelField(columnLabelRect, title, centeredStyle);
				EditorGUIUtility.RotateAroundPivot(-ANGLE, rotatePoint);

				xMove += CellSize.x;
			}
		}

		void DrawRows()
		{
			float yMove = ColumnHeaderHeight + HEADER_SPACER;
			foreach (var title in matrix.RowTitles)
			{
				var centeredStyle = GUI.skin.GetStyle("Label");
				centeredStyle.alignment = TextAnchor.MiddleRight;
				EditorGUI.LabelField(new Rect(0, yMove, RowHeaderWidth, CellSize.y), title, centeredStyle);
				yMove += CellSize.y;
			}
		}

		void DrawCells()
		{
			for (int col = 0; col < matrix.data[0].cells.Length; col++)
			{
				for (int row = 0; row < matrix.data.Length; row++)
				{
					if (matrix.CellIsAvailable(row, col))
					{
						Rect cellArea = new Rect(
							RowHeaderWidth + HEADER_SPACER + (col * CellSize.x), 
							ColumnHeaderHeight + HEADER_SPACER + (row * CellSize.y), 
							CellSize.x,
							CellSize.y
						);
						DrawCell(cellArea, ref matrix.data[row].cells[col]);
					}
				}
			}
		}

		protected abstract void DrawCell(Rect cellArea, ref T cellData);

		List<string> Duplicates(List<string> strings)
		{
			List<string> duplicates = new List<string>();
			
			for (int a = 0; a < strings.Count; a++)
			{
				for (int b = 0; b < strings.Count; b++)
				{
					if (a != b && strings[a] == strings[b])
					{
						if (!duplicates.Contains(strings[a])) { duplicates.Add(strings[a]); }
					}
				}
			}

			return (duplicates.Count > 0) ? duplicates : null;
		}
	}
}
