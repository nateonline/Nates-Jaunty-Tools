using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NatesJauntyTools.Firebase;

[Newtonsoft.Json.JsonConverter(typeof(DocumentConverter<TestDocument>)), System.Serializable]
public class TestDocument : Document
{
	public string name;
	public string location;
	public string message;
	public string description;
	public string customColor;
}

[Newtonsoft.Json.JsonConverter(typeof(CollectionConverter<TestDocument>)), System.Serializable]
public class TestDocumentCollection : Collection<TestDocument> { }
