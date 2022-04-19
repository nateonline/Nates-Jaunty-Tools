using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace NatesJauntyTools.Firebase
{
	// [JsonConverter(typeof(CollectionConverter<T>)), System.Serializable]
	public class Collection<T> where T : Document, new()
	{
		// [JsonProperty(ItemConverterType = typeof(T))]
		List<T> documents = new List<T>();

		public T this[int index]
		{
			get => documents[index];
			set => documents[index] = value;
		}

		public int Count => documents.Count;

		public void Add(T newDocument) => documents.Add(newDocument);

		public void Clear() => documents.Clear();
	}
}
