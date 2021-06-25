using System.Collections.Generic;
using UnityEngine;

namespace NatesJauntyTools
{
	public enum MatrixType { Separated, Reflected }

	public abstract class Matrix<T> : ScriptableObject
	{
		public MatrixType type;
		
		[SerializeField] List<string> rowTitles = new List<string>();
		[SerializeField] List<string> columnTitles = new List<string>();
		[SerializeField] List<string> reflectedListTitles = new List<string>();

		public List<string> RowTitles
		{
			get
			{
				return (type == MatrixType.Separated) ? rowTitles : reflectedListTitles;
			}
		}

		public List<string> ColumnTitles
		{
			get
			{
				return (type == MatrixType.Separated) ? columnTitles : reflectedListTitles;
			}
		}

		public bool CellIsAvailable(int rowIndex, int columnIndex)
		{
			return type == MatrixType.Separated || columnIndex + rowIndex < data[0].cells.Length;
		}

		[HideInInspector] public MatrixRow<T>[] data;


		public T GetValue(int rowIndex, int columnIndex)
		{
			if (CellIsAvailable(rowIndex, columnIndex))
			{
				return data[rowIndex].cells[columnIndex];
			}
			else
			{
				Debug.LogError("That cell isn't available!");
				return default(T);
			}
		}

		public T GetValue(string rowTitle, string columnTitle)
		{
			for (int r = 0; r < rowTitles.Count; r++)
			{
				for (int c = 0; c < columnTitles.Count; c++)
				{
					if (rowTitles[r] == rowTitle && columnTitles[c] == columnTitle)
					{
						return GetValue(r, c);
					}
				}
			}

			Debug.LogWarning($"Didn't find a match for row {rowTitle} and column {columnTitle}");
			return default(T);
		}

		public override string ToString()
		{
			int[] columnCharacterCounts = new int[ColumnTitles.Count + 1];

			for (int c = 0; c < columnCharacterCounts.Length; c++)
			{
				if (c == 0)
				{
					foreach (string title in rowTitles)
					{
						columnCharacterCounts[c] = Mathf.Max(columnCharacterCounts[c], title.Length);
					}
				}
				else
				{
					for (int r = 0; r < rowTitles.Count; r++)
					{
						columnCharacterCounts[c] = Mathf.Max(columnCharacterCounts[c], data[r].cells[c - 1].ToString().Length);
					}
				}
			}

			string output = "";
			
			for (int y = -1; y < RowTitles.Count; y++)
			{
				for (int x = -1; x < ColumnTitles.Count; x++)
				{
					if (x == -1 && y == -1)
					{
						output += "".PadRight(columnCharacterCounts[x + 1]);
					}
					else if (y == -1)
					{
						output += ColumnTitles[x].PadRight(columnCharacterCounts[x + 1]);
					}
					else if (x == -1)
					{
						output += RowTitles[y].PadRight(columnCharacterCounts[x + 1]);
					}
					else
					{
						output += data[y].cells[x].ToString();
					}

					output += " "; // Add space between cells in a row
				}

				output += "\n";
			}

			return output;
		}
	}

	[System.Serializable]
	public struct MatrixRow<T>
	{
		public T[] cells;

		public MatrixRow(int count) { cells = new T[count]; }
	}
}
