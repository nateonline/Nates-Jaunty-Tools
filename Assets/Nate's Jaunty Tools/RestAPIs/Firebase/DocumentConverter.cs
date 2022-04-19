using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NatesJauntyTools;

namespace NatesJauntyTools.Firebase
{
	public class DocumentConverter<T> : JsonConverter where T : Document, new()
	{
		public override bool CanConvert(Type objectType) => typeof(T).IsAssignableFrom(objectType);

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject json = JObject.Load(reader);
			T newDoc = new T();
			newDoc.InitializeFromJson(json["name"], json["createTime"], json["updateTime"]);

			JObject jsonFields = (JObject)json["fields"];

			foreach (var docField in typeof(T).GetFields())
			{
				switch (docField.Name)
				{
					case nameof(newDoc.Path):
					case nameof(newDoc.ID):
					case nameof(newDoc.CreatedTimestamp):
					case nameof(newDoc.UpdatedTimestamp):
						// Ignore these fields
						break;

					default: // All other fields
						if (jsonFields[docField.Name] != null)
						{
							docField.SetValue(newDoc, GetValue((JObject)jsonFields[docField.Name]));
						}
						break;
				}


				object GetValue(JObject fieldObject)
				{
					object value = null;

					try
					{
						if (fieldObject["stringValue"] != null) { value = fieldObject["stringValue"].ToObject(typeof(string)); }
						if (fieldObject["booleanValue"] != null) { value = fieldObject["booleanValue"].ToObject(typeof(bool)); }
						if (fieldObject["integerValue"] != null) { value = fieldObject["integerValue"].ToObject(typeof(int)); }
						if (fieldObject["doubleValue"] != null) { value = fieldObject["doubleValue"].ToObject(typeof(double)); }
						if (fieldObject["timestampValue"] != null) { value = fieldObject["timestampValue"].ToObject(typeof(DateTime)); }
					}
					catch
					{
						Debug.LogError($"DocumentConverter: Ran into an error when setting field \"{docField.Name}\"");
					}

					return value;
				}
			}

			return newDoc;
		}

		public override void WriteJson(JsonWriter writer, object data, JsonSerializer serializer)
		{
			T docData = (T)data;
			writer.WriteStartObject();

			writer.WritePropertyName("name");
			serializer.Serialize(writer, docData.Path);

			writer.WritePropertyName("fields"); // Start of fields
			writer.WriteStartObject();
			foreach (var docField in typeof(T).GetFields())
			{
				writer.WritePropertyName(docField.Name);
				if (docField.FieldType.IsEquivalentTo(typeof(string)))
				{
					writer.WriteStartObject();
					writer.WritePropertyName("stringValue");
					serializer.Serialize(writer, docField.GetValue(docData));
					writer.WriteEndObject();
				}
				else if (docField.FieldType.IsEquivalentTo(typeof(bool)))
				{
					writer.WriteStartObject();
					writer.WritePropertyName("booleanValue");
					serializer.Serialize(writer, docField.GetValue(docData));
					writer.WriteEndObject();
				}
				else if (docField.FieldType.IsEquivalentTo(typeof(int)))
				{
					writer.WriteStartObject();
					writer.WritePropertyName("integerValue");
					serializer.Serialize(writer, docField.GetValue(docData));
					writer.WriteEndObject();
				}
				else if (docField.FieldType.IsEquivalentTo(typeof(double)))
				{
					writer.WriteStartObject();
					writer.WritePropertyName("doubleValue");
					serializer.Serialize(writer, docField.GetValue(docData));
					writer.WriteEndObject();
				}
				else if (docField.FieldType.IsEquivalentTo(typeof(DateTime)))
				{
					writer.WriteStartObject();
					writer.WritePropertyName("timestampValue");
					serializer.Serialize(writer, docField.GetValue(docData));
					writer.WriteEndObject();
				}
				else
				{
					serializer.Serialize(writer, null);
				}
			}
			writer.WriteEndObject(); // End of fields

			writer.WriteEndObject();
		}
	}
}
