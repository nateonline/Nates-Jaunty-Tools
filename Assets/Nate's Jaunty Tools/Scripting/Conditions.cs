using System;
using UnityEngine;

namespace NatesJauntyTools
{
	/// <summary> Used for organizing many boolean conditions into a single output. </summary>
	public class Conditions
	{
		const char trueCharacter = 'Y';
		const char falseCharacter = 'N';

		string[] names;
		bool[] values;


		public Conditions()
		{
			Clear();
		}

		int GrowArray()
		{
			string[] newNames = new string[names.Length + 1];
			Array.Copy(names, newNames, names.Length);
			names = newNames;

			bool[] newValues = new bool[values.Length + 1];
			Array.Copy(values, newValues, values.Length);
			values = newValues;

			return names.Length - 1;
		}

		public void Track(bool condition, string name = "")
		{
			int newItemIndex = GrowArray();

			names[newItemIndex] = name;
			values[newItemIndex] = condition;
		}

		public void Clear()
		{
			names = new string[0];
			values = new bool[0];
		}

		public bool AND()
		{
			for (int i = 0; i < values.Length; i++)
			{
				if (values[i] == false) { return false; }
			}

			return true;
		}

		public bool OR()
		{
			for (int i = 0; i < values.Length; i++)
			{
				if (values[i] == true) { return true; }
			}

			return false;
		}

		public void DebugLog(bool hideNames = false)
		{
			string logString = "";

			for (int i = 0; i < values.Length; i++)
			{
				logString += MessageForIndex(i, hideNames);
			}

			Debug.Log(logString);
		}

		string MessageForIndex(int i, bool hideNames)
		{
			if (names[i] == "" || hideNames)
			{
				if (i < values.Length)
				{
					return (values[i]) ? $"[{trueCharacter}] " : $"[{falseCharacter}] ";
				}
				else
				{
					return (values[i]) ? $"[{trueCharacter}]" : $"[{falseCharacter}]";
				}
			}
			else
			{
				if (i < values.Length)
				{
					return (values[i]) ? $"[{names[i]} : {trueCharacter}] " : $"[{names[i]} : {falseCharacter}] ";
				}
				else
				{
					return (values[i]) ? $"[{names[i]} : {trueCharacter}]" : $"[{names[i]} : {falseCharacter}]";
				}
			}
		}
	}
}
