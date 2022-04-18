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
	public Status status;
	public TextField username;
	public TextField message;


	[InspectorButton]
	public void GetData()
	{
		status.Set("Getting data...");
		firestore.GetDocument<TestDocument>(path, OnGetData);


		void OnGetData(TestDocument updatedDocument)
		{
			username.Value = updatedDocument.username;
			message.Value = updatedDocument.message;
			status.Clear();
		}
	}

	[InspectorButton]
	public void SetData()
	{
		TestDocument newDoc = new TestDocument();
		newDoc.username = username.Value;
		newDoc.message = message.Value;

		status.Set("Setting data...");
		firestore.SetDocument(path, newDoc, OnSetData);


		void OnSetData(TestDocument updatedDocument)
		{
			status.Clear();
		}
	}
}
