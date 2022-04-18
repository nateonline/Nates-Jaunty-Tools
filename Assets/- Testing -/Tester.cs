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
	public FirestoreAPI firestore;
	public string path;
	public TestDocument testDocument;

	[InspectorButton]
	public async void GetTestData()
	{
		testDocument = await firestore.GetDocumentAsync<TestDocument>(path);
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
		firestore.SetDocument(path, testDocument, OnSetData);


		void OnSetData(TestDocument updatedDocument)
		{
			Debug.Log(updatedDocument.JSON);
		}
	}
}
