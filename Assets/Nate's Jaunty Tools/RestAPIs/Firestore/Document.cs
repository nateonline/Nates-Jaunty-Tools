using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NatesJauntyTools;

namespace NatesJauntyTools.Firestore
{
	[JsonConverter(typeof(DocumentConverter<Document>)), Serializable]
	public class Document
	{
		public string Path { get; private set; }
		public string ID => Path.SubstringAfterLast("/");
		public DateTime CreatedTimestamp { get; private set; }
		public DateTime UpdatedTimestamp { get; private set; }

		public string JSON => JsonConvert.SerializeObject(this);


		public void InitializeFromJson(JToken id, JToken created, JToken updated)
		{
			Path = id.ToString();
			CreatedTimestamp = DateTime.Parse(created.ToString());
			UpdatedTimestamp = DateTime.Parse(updated.ToString());
		}
	}
}
