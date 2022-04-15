using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;
using NatesJauntyTools;
using NatesJauntyTools.Firestore;

public class Tester : Script
{
	public FirestoreLink firestore;
	public string path;

	[InspectorButton]
	public async void GetTestData()
	{
		string json = await firestore.GetRawData(path);
		Debug.Log(json);

		var testDoc = JsonConvert.DeserializeObject<TestDocument>(json);
		Debug.Log(testDoc.ID);
		Debug.Log(testDoc.intField);
	}
}
