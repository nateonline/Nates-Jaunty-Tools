using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace NatesJauntyTools
{
	public static partial class Tools
	{
		/// <sumary>
		/// String must be in this exact format: 9/13/2020 11:03:39 AM
		/// </summary>
		public static DateTime DateTimeFromString(this string input)
		{
			string[] allSegments = input.Split(' ');
			string[] dateSegments = allSegments[0].Split('/');
			string[] timeSegments = allSegments[1].Split(':');
			string timeAMPM = allSegments[2];

			int year, month, day, hour, minute, second;

			year = int.Parse(dateSegments[2]);
			month = int.Parse(dateSegments[0]);
			day = int.Parse(dateSegments[1]);

			if (timeAMPM == "AM") { hour = int.Parse(timeSegments[0]); }
			else if (timeAMPM == "PM") { hour = int.Parse(timeSegments[0]) + 12; }
			else
			{
				Debug.LogError("DateTime time was not in AM or PM!");
				return new DateTime();
			}

			minute = int.Parse(timeSegments[1]);
			second = int.Parse(timeSegments[2]);

			return new DateTime(year, month, day, hour, minute, second);
		}

		public static string ToString_HumanTime(this DateTime input)
		{
			return input.ToShortTimeString().ToLower().Replace(":00", "").Replace(" ", "");
		}

		public static string ToString_NoSecondsDateTime(this DateTime input)
		{
			Regex rx = new Regex(@"/:..\s/");
			string shortTime = rx.Replace(input.ToShortTimeString(), "");
			return $"{input.ToShortDateString()} {shortTime}";
		}

		public static double ConvertToUnixTimestamp(DateTime date)
		{
			DateTime unixOrigin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			TimeSpan diff = date.ToUniversalTime() - unixOrigin;
			return Math.Floor(diff.TotalSeconds);
		}

		public static string FriendlyNowComparison(this DateTime target)
		{
			DateTime now = DateTime.Now;
			TimeSpan difference;
			bool isPastTarget;
			string valueString;

			if (target < now)
			{
				difference = now - target;
				isPastTarget = true;
			}
			else
			{
				difference = target - now;
				isPastTarget = false;
			}

			if (difference.Days >= 2)
			{
				valueString = $"{(int)difference.Days} days";
			}
			else if (difference.TotalHours >= 2)
			{
				valueString = $"{(int)difference.TotalHours} hours";
			}
			else if (difference.TotalMinutes >= 2)
			{
				valueString = $"{(int)difference.TotalMinutes} minutes";
			}
			else if (difference.TotalSeconds >= 2)
			{
				valueString = $"{(int)difference.TotalSeconds} seconds";
			}
			else return "now";

			return (isPastTarget) ? $"{valueString} ago" : $"{valueString} from now";
		}
	}
}
