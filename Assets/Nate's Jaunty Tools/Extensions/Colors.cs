using UnityEngine;

namespace NatesJauntyTools
{
	public static partial class Tools
	{
		/// <summary> Use in URP to get a material's color, instead of using material.color. </summary>
		/// <remarks> In Universal Render Pipeline, the material.color property doesn't work. This is an issue in default URP shaders that don't properly set the _Color shader property. </remarks>
		public static Color32 ColorURP(this Material material)
		{
			return material.GetColor("_BaseColor");
		}

		/// <summary> Get the hex code (uppercase with leading #) from a color. </summary>
		public static string ToHex(this Color color) { return ((Color32)color).ToHex(); }

		/// <summary> Get the hex code (uppercase with leading #) from a color. </summary>
		public static string ToHex(this Color32 color)
		{
			int[] characterValues = new int[8];

			characterValues[0] = color.r / 16;
			characterValues[1] = color.r % 16;
			characterValues[2] = color.g / 16;
			characterValues[3] = color.g % 16;
			characterValues[4] = color.b / 16;
			characterValues[5] = color.b % 16;
			characterValues[6] = color.a / 16;
			characterValues[7] = color.a % 16;

			string hexCode = "";

			for (int i = 0; i < characterValues.Length; i++)
			{
				if (characterValues[i] < 10)
				{
					hexCode += (char)(characterValues[i] + 48);
				}
				else
				{
					hexCode += (char)(characterValues[i] + 87);
				}
			}

			return $"#{hexCode.ToUpper()}";
		}

		/// <summary> Get the hex code (with leading #) from a material. This won't work in URP. </summary>
		public static string ToHex(this Material material)
		{
			return ToHex(material.color);
		}

		/// <summary> Get the hex code (with leading #) from a material. Use this instead of material.color in URP. </summary>
		public static string ToHexURP(this Material material)
		{
			return ToHex(material.ColorURP());
		}

		/// <summary> Convert a string representing a hex code into a color. The leading # is optional. </summary>
		/// <remarks> Supports string lengths of 1,2,3,4,6,8 not including the leading #. </remarks>
		public static Color32 HexToColor(this string hexCode)
		{
			hexCode = hexCode.TrimStart('#');

			string formattedHexCode = "";

			switch (hexCode.Length)
			{
				case 1:
					formattedHexCode += hexCode[0];
					formattedHexCode += hexCode[0];
					formattedHexCode += hexCode[0];
					formattedHexCode += hexCode[0];
					formattedHexCode += hexCode[0];
					formattedHexCode += hexCode[0];
					formattedHexCode += 'f';
					formattedHexCode += 'f';
					break;

				case 2:
					formattedHexCode += hexCode[0];
					formattedHexCode += hexCode[1];
					formattedHexCode += hexCode[0];
					formattedHexCode += hexCode[1];
					formattedHexCode += hexCode[0];
					formattedHexCode += hexCode[1];
					formattedHexCode += 'f';
					formattedHexCode += 'f';
					break;

				case 3:
					formattedHexCode += hexCode[0];
					formattedHexCode += hexCode[0];
					formattedHexCode += hexCode[1];
					formattedHexCode += hexCode[1];
					formattedHexCode += hexCode[2];
					formattedHexCode += hexCode[2];
					formattedHexCode += 'f';
					formattedHexCode += 'f';
					break;

				case 4:
					formattedHexCode += hexCode[0];
					formattedHexCode += hexCode[0];
					formattedHexCode += hexCode[1];
					formattedHexCode += hexCode[1];
					formattedHexCode += hexCode[2];
					formattedHexCode += hexCode[2];
					formattedHexCode += hexCode[3];
					formattedHexCode += hexCode[3];
					break;

				case 6:
					formattedHexCode += hexCode[0];
					formattedHexCode += hexCode[1];
					formattedHexCode += hexCode[2];
					formattedHexCode += hexCode[3];
					formattedHexCode += hexCode[4];
					formattedHexCode += hexCode[5];
					formattedHexCode += 'f';
					formattedHexCode += 'f';
					break;

				case 8:
					formattedHexCode += hexCode[0];
					formattedHexCode += hexCode[1];
					formattedHexCode += hexCode[2];
					formattedHexCode += hexCode[3];
					formattedHexCode += hexCode[4];
					formattedHexCode += hexCode[5];
					formattedHexCode += hexCode[6];
					formattedHexCode += hexCode[7];
					break;
			}

			formattedHexCode = formattedHexCode.ToLower();

			int[] characterValues = new int[8];

			for (int i = 0; i < formattedHexCode.Length; i++)
			{
				if (System.Char.IsDigit(formattedHexCode[i]))
				{
					characterValues[i] = (int)formattedHexCode[i] - 48;
				}
				else if (System.Char.ToLower(formattedHexCode[i]) >= 'a' && System.Char.ToLower(formattedHexCode[i]) <= 'f')
				{
					characterValues[i] = (int)formattedHexCode[i] - 87;
				}
				else
				{
					characterValues[i] = 15; // Set to f
				}
			}

			byte red = 0, green = 0, blue = 0, alpha = 0;

			for (int i = 0; i < characterValues.Length; i++)
			{
				if (i % 2 == 0) { characterValues[i] *= 16; }

				if (i < 2)
				{
					red += (byte)characterValues[i];
				}
				else if (i >= 2 && i < 4)
				{
					green += (byte)characterValues[i];
				}
				else if (i >= 4 && i < 6)
				{
					blue += (byte)characterValues[i];
				}
				else
				{
					alpha += (byte)characterValues[i];
				}
			}

			return new Color32(red, green, blue, alpha);
		}

		public static Color32 RandomAnalogousColor(this Color32 startingColor, float degreeShift = 30f, int svCloseness = 1)
		{
			float degrees = degreeShift / 360f;

			float h, s, v;
			Color.RGBToHSV(startingColor, out h, out s, out v);

			h += Random.Range(-degrees, degrees);
			h %= 1;

			if (svCloseness <= 0) { svCloseness = 1; }

			float tempS = 0f;
			for (int i = 0; i < svCloseness; i++)
			{
				tempS += Random.Range(-0.5f, 0.5f);
			}
			s += tempS / (float)svCloseness;

			float tempV = 0f;
			for (int i = 0; i < svCloseness; i++)
			{
				tempV += Random.Range(-0.5f, 0.5f);
			}
			v += tempV / (float)svCloseness;

			return Color.HSVToRGB(h, s, v);
		}

		public static Color32 RandomColor(bool randomAlpha = false)
		{
			if (randomAlpha)
			{
				return new Color32(
					(byte)UnityEngine.Random.Range(0, 256),
					(byte)UnityEngine.Random.Range(0, 256),
					(byte)UnityEngine.Random.Range(0, 256),
					(byte)UnityEngine.Random.Range(0, 256)
				);
			}
			else
			{
				return new Color32(
					(byte)UnityEngine.Random.Range(0, 256),
					(byte)UnityEngine.Random.Range(0, 256),
					(byte)UnityEngine.Random.Range(0, 256),
					255
				);
			}
		}

		public static float GrayscaleValue(this Color32 color)
		{
			return Mathf.Sqrt(
				Mathf.Pow(color.r, 2) * 0.299f +
				Mathf.Pow(color.g, 2) * 0.587f +
				Mathf.Pow(color.b, 2) * 0.114f);
		}

		public static Color32 PreferredTextColor(this Color32 backgroundColor, float midpoint = 0.5f)
		{
			float luminosity = Mathf.Sqrt(
				Mathf.Pow(backgroundColor.r, 2) * 0.299f +
				Mathf.Pow(backgroundColor.g, 2) * 0.587f +
				Mathf.Pow(backgroundColor.b, 2) * 0.114f);

			return (luminosity < 255f * Mathf.Pow(midpoint, 1f / 2.2f)) ? Color.white : Color.black;
		}

		public static Color32 PreferredTextColor(this Color32 backgroundColor, Color32 darkTextColor, Color32 lightTextColor, float midpoint = 0.5f)
		{
			float luminosity = Mathf.Sqrt(
				Mathf.Pow(backgroundColor.r, 2) * 0.299f +
				Mathf.Pow(backgroundColor.g, 2) * 0.587f +
				Mathf.Pow(backgroundColor.b, 2) * 0.114f);

			return (luminosity < 255f * Mathf.Pow(midpoint, 1f / 2.2f)) ? lightTextColor : darkTextColor;
		}
	}
}
