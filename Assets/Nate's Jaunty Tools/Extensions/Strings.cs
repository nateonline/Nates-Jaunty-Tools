using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace NatesJauntyTools
{
	public static partial class Tools
	{
		/// <summary> Converts a camel case string into a string that has spaces between capitalized words. </summary>
		/// <returns> A new string with whitespace between words </returns>
		public static string SplitCamelCase(this string input)
		{
			string output = "";

			for (int i = 0; i < input.Length; i++)
			{
				if (i == 0 || i == input.Length - 1)
				{
					if (i == 0)
					{
						output += Char.ToUpper(input[i]);
					}
					else if (i == input.Length - 1)
					{
						if (Char.IsLetter(input[i]))
						{
							output += input[i];
						}
						else if (Char.IsNumber(input[i]))
						{
							if (Char.IsLetter(input[i - 1]))
							{
								output += $" {input[i]}";
							}
							else
							{
								output += input[i];
							}
						}
					}
				}
				else
				{
					if (Char.IsLower(input[i - 1]) && Char.IsUpper(input[i]))
					{
						output += $" {input[i]}";
					}
					else if (Char.IsUpper(input[i - 1]) && Char.IsUpper(input[i]) && Char.IsLower(input[i + 1]))
					{
						output += $" {input[i]}";
					}
					else if (Char.IsLetter(input[i - 1]) && Char.IsNumber(input[i]))
					{
						output += $" {input[i]}";
					}
					else if (Char.IsNumber(input[i - 1]) && Char.IsLetter(input[i]))
					{
						output += $" {Char.ToUpper(input[i])}";
					}
					else
					{
						output += input[i];
					}
				}
			}

			return output;
		}


		/// <summary> Uses Levenshtein Distance to find the difference between the extended string and a passed string. </summary>
		/// <returns> An integer representing the amount of steps needed to change the extended string into the passed string </returns>
		/// https://www.youtube.com/watch?v=We3YDTzNXEk&ab_channel=TusharRoy-CodingMadeSimple
		public static int Difference(this string extended, string passed)
		{
			int[,] diff = new int[extended.Length + 1, passed.Length + 1];

			// Verify arguments
			if (extended.Length == 0) { return passed.Length; }
			if (passed.Length == 0) { return extended.Length; }

			// Initialize arrays
			for (int i = 0; i <= extended.Length; i++) { diff[i, 0] = i; }
			for (int j = 0; j <= passed.Length; j++) { diff[0, j] = j; }

			// Begin looping
			for (int row = 1; row <= extended.Length; row++)
			{
				for (int col = 1; col <= passed.Length; col++)
				{
					// Compute cost
					int cost = (passed[col - 1] == extended[row - 1]) ? 0 : 1;
					diff[row, col] = MinOf3(
						diff[row - 1, col] + 1,
						diff[row, col - 1] + 1,
						diff[row - 1, col - 1] + cost
					);
				}
			}

			/* Log Each */ /**
			string header = "\t"; foreach (char c in passed) { header += $"<b>{c}</b>\t"; } Debug.Log(header);
			for (int row = 1; row <= extended.Length; row++)
			{
				string line = $"<b>{extended[row - 1]}</b>\t";

				for (int col = 1; col <= passed.Length; col++)
				{
					line += $"{diff[row, col]}";
					if (col != passed.Length) { line += "\t"; }
				}

				Debug.Log(line);
			}
			/**/

			// Return cost
			return diff[extended.Length, passed.Length];

			// Function to find minimum of 3 numbers
			int MinOf3(int n1, int n2, int n3)
			{
				return Math.Min(Math.Min(n1, n2), n3);
			}
		}


		public static string RemoveParentheses(this string input)
		{

			input = Regex.Replace(input, @"\(.*?\)", "");
			input = Regex.Replace(input, @"\s{2,}", " ");
			return input;
		}

		public static bool IsNumber(this string input) => Regex.IsMatch(input, @"/^[-+]?\d+$/");

		public static bool IsInteger(this string input) => Regex.IsMatch(input, @"^[-+]?[0-9]\d*(\.\d+)?$");

		public static bool RepresentsTrue(this string arg) { return (arg.ToLower() == "t" || arg.ToLower() == "true" || arg.ToLower() == "y" || arg.ToLower() == "yes"); }

		public static bool RepresentsFalse(this string arg) { return (arg.ToLower() == "f" || arg.ToLower() == "false" || arg.ToLower() == "n" || arg.ToLower() == "no"); }


		public enum CharacterSet { AlphaNumeric_All, AlphaNumeric_Upper, AlphaNumeric_Lower, Alpha_All, Alpha_Upper, Alpha_Lower, Numeric }

		public static string GetCharacters(this CharacterSet characterSet)
		{
			switch (characterSet)
			{
				case CharacterSet.AlphaNumeric_All: return "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
				case CharacterSet.AlphaNumeric_Upper: return "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
				case CharacterSet.AlphaNumeric_Lower: return "abcdefghijklmnopqrstuvwxyz0123456789";
				case CharacterSet.Alpha_All: return "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
				case CharacterSet.Alpha_Upper: return "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
				case CharacterSet.Alpha_Lower: return "abcdefghijklmnopqrstuvwxyz";
				case CharacterSet.Numeric: return "0123456789";

				default: throw new NotImplementedException($"CharacterSet {characterSet} doesn't have a defined set of characters!");
			}
		}


		public static string SubstringAfterLast(this string value, string search)
		{
			int pos = value.LastIndexOf(search) + 1;
			return value.Substring(pos, value.Length - pos);
		}
	}
}
