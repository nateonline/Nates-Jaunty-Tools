using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

namespace NatesJauntyTools
{
	public static partial class Tools
	{
		/// <summary> Uses binary serialization to deeply copy the object </summary>
		public static T DeepCopy_Binary<T>(this T original)
		{
			using (var ms = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(ms, original);
				ms.Position = 0;

				return (T)formatter.Deserialize(ms);
			}
		}

		/// <summary> Uses reflection to deeply copy all the fields of the object </summary>
		public static T DeepCopy<T>(this T original) where T : new()
		{
			return typeof(T).GetFields().Aggregate(new T(), (newCopy, field) =>
			{
				field.SetValue(newCopy, field.GetValue(original));
				return newCopy;
			});
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