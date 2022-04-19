using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;
using NatesJauntyTools;
using NatesJauntyTools.Firebase;
using NatesJauntyTools.RiotGames;

public class Tester : Script
{
	[Header("Firebase")]
	public FirestoreAPI firestore;
	public string path;

	[InspectorButton(true)]
	public async void TestDeserializeCollection()
	{
		string json = "{\"documents\":[{\"name\":\"projects/office-status-320718/databases/(default)/documents/data/nate\",\"fields\":{\"location\":{\"stringValue\":\"IT Office\"},\"description\":{\"stringValue\":\"\"},\"customColor\":{\"stringValue\":\"\"},\"name\":{\"stringValue\":\"Nate\"},\"message\":{\"stringValue\":\"Come In!\"}},\"createTime\":\"2022-04-19T20:59:13.427804Z\",\"updateTime\":\"2022-04-19T21:04:44.726569Z\"}]}";

		string modifiedJson = "[{\"name\":\"projects/office-status-320718/databases/(default)/documents/data/nate\",\"fields\":{\"location\":{\"stringValue\":\"IT Office\"},\"description\":{\"stringValue\":\"\"},\"customColor\":{\"stringValue\":\"\"},\"name\":{\"stringValue\":\"Nate\"},\"message\":{\"stringValue\":\"Come In!\"}},\"createTime\":\"2022-04-19T20:59:13.427804Z\",\"updateTime\":\"2022-04-19T21:04:44.726569Z\"}]";

		List<TestDocument> testCollection = await firestore.GetCollectionAsync<TestDocument>("data");
		Debug.Log(testCollection.Count);
	}
}
