using UnityEngine;
using UnityEditor;

namespace NatesJauntyTools
{
	[CustomEditor(typeof(BoolMatrix))]
	public class BoolMatrixEditor : MatrixEditor<bool>
	{
		protected override Vector2 CellSize { get { return new Vector2(20, 20); } }

		protected override void DrawCell(Rect cellArea, ref bool cellData)
		{
			cellData = EditorGUI.Toggle(cellArea, cellData);
		}
	}
}
