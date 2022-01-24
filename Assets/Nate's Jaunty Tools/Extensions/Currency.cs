using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NatesJauntyTools
{
	public static partial class Tools
	{
		public static string FormatAsCurrency(this string arg, bool includeSymbol = false)
		{
			if (decimal.TryParse(arg, out decimal decimalArg))
			{
				return FormatAsCurrency(decimalArg, includeSymbol);
			}
			return arg;
		}

		public static string FormatAsCurrency(this double arg, bool includeSymbol = false)
		{
			return FormatAsCurrency(Convert.ToDecimal(arg), includeSymbol);
		}

		public static string FormatAsCurrency(this decimal arg, bool includeSymbol = false)
		{
			string formattedString = String.Format("{0:n}", arg);
			if (includeSymbol) formattedString = "$ " + formattedString;
			return formattedString;
		}
	}
}
