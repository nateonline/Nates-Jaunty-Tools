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
	public TestDocument testDocument;

	[InspectorButton]
	public async void GetTestData()
	{
		string json = await firestore.GetRawData(path);
		Debug.Log(json);

		testDocument = JsonConvert.DeserializeObject<TestDocument>(json);
		Debug.Log(testDocument.ID);
		Debug.Log(testDocument.ID);
		Debug.Log(testDocument.intField);
	}

	[InspectorButton]
	public void DebugJSON()
	{
		Debug.Log(JsonConvert.SerializeObject(testDocument));
	}

	[InspectorButton]
	public void SetTestData()
	{
		firestore.SetData(path, testDocument, OnSetData);


		void OnSetData(TestDocument updatedDocument)
		{
			Debug.Log(updatedDocument.JSON);
		}
	}
}
