using UnityEngine;
using UnityEditor;

namespace NatesJauntyTools
{
	[CustomEditor(typeof(IntMatrix))]
	public class IntMatrixEditor : MatrixEditor<int>
	{
		protected override Vector2 CellSize { get { return new Vector2(40, 20); } }

		protected override void DrawCell(Rect cellArea, ref int cellData)
		{
			cellData = EditorGUI.IntField(cellArea, cellData);
		}
	}
}
