using UnityEngine;

namespace NatesJauntyTools
{
	public static partial class Tools
	{
		public static byte ToByte(this System.Enum e)
		{
			return System.Convert.ToByte(e);
		}
	}
}
