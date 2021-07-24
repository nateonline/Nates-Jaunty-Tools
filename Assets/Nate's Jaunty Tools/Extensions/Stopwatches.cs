using System.Diagnostics;
using UnityEngine;

namespace NatesJauntyTools
{
	public static partial class Tools
	{
		/// <summary> Gets the ellapsed time as a float </summary>
		public static float GetEllapsedTime(this Stopwatch stopwatch)
		{
			return ((float)stopwatch.Elapsed.Seconds) + ((float)stopwatch.Elapsed.Milliseconds / 1000f);
		}
	}
}