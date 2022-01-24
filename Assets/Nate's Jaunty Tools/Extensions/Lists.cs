using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace NatesJauntyTools
{
	public static partial class Tools
	{
		/// <summary> Randomizes the order of items in the list </summary>
		public static void Shuffle<T>(this IList<T> list, bool increasedRandomness = false)
		{
			if (increasedRandomness)
			{
				RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();

				int n = list.Count;
				while (n > 1)
				{
					byte[] box = new byte[1];
					do provider.GetBytes(box);
					while (!(box[0] < n * (Byte.MaxValue / n)));
					int k = (box[0] % n);
					n--;
					T value = list[k];
					list[k] = list[n];
					list[n] = value;
				}
			}
			else
			{
				System.Random rng = new System.Random();

				int n = list.Count;
				while (n > 1)
				{
					n--;
					int k = rng.Next(n + 1);
					T value = list[k];
					list[k] = list[n];
					list[n] = value;
				}
			}
		}

		/// <summary> Gets a random element from the list </summary>
		public static T RandomItem<T>(this IEnumerable<T> list)
		{
			return (T)list.ElementAt(UnityEngine.Random.Range(0, list.Count()));
		}

		/// <summary> Runs Debug.Log on each item in the list </summary>
		/// <param name="multiLine"> Controls if the output should display on multiple lines </param>
		public static void LogEach<T>(this IList<T> list, string headerMessage = "", bool multiLine = false)
		{
			bool logHeader = !string.IsNullOrEmpty(headerMessage);

			if (multiLine)
			{
				if (logHeader) { Debug.Log($"--- {headerMessage} ---"); }
				foreach (T item in list)
				{
					Debug.Log(item.ToString());
				}
				string endingMessage = "----";
				foreach (Char c in headerMessage) { endingMessage += "-"; }
				endingMessage += "----";
				if (logHeader) { Debug.Log(endingMessage); Debug.Log(""); }
			}
			else
			{
				string output = (logHeader) ? $"{headerMessage}: " : "";

				for (int i = 0; i < list.Count; i++)
				{
					output += (i != list.Count - 1) ? $"{list[i].ToString()}, " : $" {list[i].ToString()}";
				}

				Debug.Log(output);
			}
		}

		/// <summary> Returns the last index of the list </summary>
		public static int LastIndex<T>(this IList<T> list) { return list.Count - 1; }

		/// <summary> Returns the last item in the list </summary>
		public static T LastItem<T>(this IList<T> list) { return list[list.LastIndex()]; }

		/// <summary> Uses ToString to order the elements, then joins them together with a delimiter </summary>
		public static string SortAndStringify<T>(this IEnumerable<T> list, string delimiter = ", ") => string.Join(delimiter, list.OrderBy(i => i.ToString()));

		/// <summary> Checks if two lists have the same elements, regardless of order </summary>
		public static bool Matches<T>(this IEnumerable<T> list1, IEnumerable<T> list2) => list1.Except(list2).Count() + list2.Except(list1).Count() == 0;

		/// <summary> Gets the item in the list by wrapping the index. Allows negative index. </summary>
		/// <remarks> Ex: In a list with numbers 1 through 5, AtWrap(0) = 1, AtWrap(5) = 1, AtWrap(-1) = 5 </remarks>
		public static T AtWrap<T>(this IList<T> list, int index)
		{
			int moddedIndex = index % list.Count;
			int resultIndex = (moddedIndex >= 0) ? moddedIndex : list.Count - Math.Abs(moddedIndex);
			return list[resultIndex];
		}
	}
}
