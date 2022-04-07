using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NatesJauntyTools.GoogleSheets
{
	public static class GoogleSheetsExtensions
	{
		public static T CellTo<T>(this object cellValue)
		{
			if (cellValue is T) return (T)cellValue;
			try { return (T)Convert.ChangeType(cellValue, typeof(T)); }
			catch (InvalidCastException) { return default(T); }
		}

		// public float CurrencyToFloat(object cellValue)
		// {
		// 	NumberFormatInfo currencyFormatter = new NumberFormatInfo()
		// 	{
		// 		CurrencyDecimalDigits = 2,
		// 		CurrencyDecimalSeparator = ".",
		// 		CurrencySymbol = "$"
		// 	};
		// 	return (float.TryParse((string)cellValue, NumberStyles.Currency, currencyFormatter, out float result)) ? result : 0;
		// }
	}
}
