using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace NatesJauntyTools
{
	public partial class Tools
	{
		public static int RandomInRange_Secure(int min, int max)
		{
			if (min > max) Debug.LogError("min is greater than max");
			if (min == max) return min;

			using (var rng = new RNGCryptoServiceProvider())
			{
				var data = new byte[4];
				rng.GetBytes(data);

				int generatedValue = Mathf.Abs(BitConverter.ToInt32(data, startIndex: 0));

				int diff = max - min;
				int mod = generatedValue % diff;
				int normalizedNumber = min + mod;

				return normalizedNumber;
			}
		}

		public static string RandomString_Secure(int length, string characterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")
		{
			var stringChars = new char[length];
			
			for (int i = 0; i < stringChars.Length; i++)
			{
				stringChars[i] = characterSet[RandomInRange_Secure(0, characterSet.Length)];
			}

			return new String(stringChars);
		}
	}
}
