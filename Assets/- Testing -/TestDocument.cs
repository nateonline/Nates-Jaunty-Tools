using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NatesJauntyTools.Firestore;

[Newtonsoft.Json.JsonConverter(typeof(DocumentConverter<TestDocument>)), System.Serializable]
public class TestDocument : Document
{
	public string message;
	public int intField;
}
