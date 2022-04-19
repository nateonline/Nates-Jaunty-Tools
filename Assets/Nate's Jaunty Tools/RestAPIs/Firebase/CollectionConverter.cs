using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NatesJauntyTools.Firebase
{
	public class CollectionConverter<T> : JsonConverter where T : Document, new()
	{
		public override bool CanConvert(Type objectType) => typeof(T).IsAssignableFrom(objectType);

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject json = JObject.Load(reader);
			Collection<T> newCollection = new Collection<T>();

			foreach (JObject doc in (JArray)json["documents"])
			{
				newCollection.Add(doc.ToObject<T>());
			}

			return newCollection;
		}

		public override void WriteJson(JsonWriter writer, object data, JsonSerializer serializer)
		{
			T docData = (T)data;
			writer.WriteStartObject();

			writer.WriteEndObject();
		}
	}
}
