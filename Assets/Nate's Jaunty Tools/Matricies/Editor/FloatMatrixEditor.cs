using UnityEngine;
using UnityEditor;

namespace NatesJauntyTools
{
	[CustomEditor(typeof(FloatMatrix))]
	public class FloatMatrixEditor : MatrixEditor<float>
	{
		protected override Vector2 CellSize { get { return new Vector2(40, 20); } }

		protected override void DrawCell(Rect cellArea, ref float cellData)
		{
			cellData = EditorGUI.FloatField(cellArea, cellData);
		}
	}
}
