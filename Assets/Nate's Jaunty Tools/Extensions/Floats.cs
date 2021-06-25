using UnityEngine;

namespace NatesJauntyTools
{
	public static partial class Tools
	{
		public static float Wrap(this float value, float min, float max)
		{
			float rangeSize = max - min;

			while (value > max) { value -= rangeSize; }
			while (value < min) { value += rangeSize; }

			return value;
		}
	}
}