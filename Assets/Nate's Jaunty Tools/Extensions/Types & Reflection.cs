using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

namespace NatesJauntyTools
{
	public static partial class Tools
	{
		/// <summary> Returns a deep copy of the reference type object (I think this only does 1 layer deep currently) </summary>
		public static T DeepCopy<T>(this T obj)
		{
			using (var ms = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(ms, obj);
				ms.Position = 0;

				return (T) formatter.Deserialize(ms);
			}
		}

		public static Dictionary<string, string> GetFields(this object subject)
		{
			Dictionary<string, string> fields = new Dictionary<string, string>();

			foreach (FieldInfo field in subject.GetType().GetFields())
			{
				if (field.GetValue(subject) == null)
				{
					fields.Add(field.Name, "");
				}
				// else if (field.FieldType.GetInterface(nameof(IEnumerable)) != null)
				// {
				// 	Array array = field.GetValue(subject) as Array;
					
				// 	if (array != null)
				// 	{
				// 		List<string> values = new List<string>();
				// 		foreach (var element in array)
				// 		{
				// 			if (element != null) { values.Add(element.ToString()); }
				// 			else { values.Add("null"); }
				// 		}
				// 		fields.Add(field.Name, String.Join(", ", values));
				// 	}
				// }
				else
				{
					fields.Add(field.Name, field.GetValue(subject).ToString());
				}
			}

			return fields;
		}
	}
}