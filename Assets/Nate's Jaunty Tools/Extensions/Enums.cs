using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NatesJauntyTools
{
	public static partial class Tools
	{
		public static byte ToByte(this System.Enum e) => System.Convert.ToByte(e);

		public static List<T> GetEnumValues<T>() where T : struct
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("GetValues<T> can only be called for types derived from System.Enum", "T");
			}
			return new List<T>((T[])Enum.GetValues(typeof(T)));
		}
	}
}
