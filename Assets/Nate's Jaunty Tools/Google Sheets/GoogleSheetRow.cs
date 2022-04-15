using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;
using UnityEngine;

namespace NatesJauntyTools.GoogleSheets
{
	public abstract class GoogleSheetRow
	{
		public abstract void Deserialize(List<object> rowValues);

		public abstract List<object> Serialize();
	}

	public static class GoogleSheetRowExtensions
	{
		public static List<List<object>> ToTable<T>(this List<T> listOfRows) where T : GoogleSheetRow
		{
			List<List<object>> tableValues = new List<List<object>>();
			foreach (T row in listOfRows) { tableValues.Add(row.Serialize()); }
			return tableValues;
		}
	}
}
