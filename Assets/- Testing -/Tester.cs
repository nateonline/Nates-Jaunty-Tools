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
	public async void GetData()
	{
		TestDocument t = await firestore.GetDocumentAsync<TestDocument>("data/nate");
		Debug.Log(t.name);
	}

	[InspectorButton(true)]
	public async void TestDeserializeCollection()
	{
		List<TestDocument> testCollection = await firestore.GetCollectionAsync<TestDocument>("data");
		Debug.Log(testCollection.Count);
	}
}
