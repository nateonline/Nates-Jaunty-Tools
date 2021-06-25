using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace NatesJauntyTools
{
	public static partial class Tools
	{
		/// <summary> Randomizes the order of items in a list. </summary>
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

		/// <summary> Gets a random element from a list. </summary>
		public static T RandomItem<T>(this IEnumerable<T> list)
		{
			return (T)list.ElementAt(UnityEngine.Random.Range(0, list.Count()));
		}

		/// <summary> Debug logs each item in a list. </summary>
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
	}
}
