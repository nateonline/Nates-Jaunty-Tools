using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using NatesJauntyTools.REST;

namespace NatesJauntyTools.Firestore
{
	// https://firebase.google.com/docs/firestore/use-rest-api
	// https://firebase.google.com/docs/firestore/reference/rest/v1/projects.databases.documents

	[CreateAssetMenu(menuName = "Nate's Jaunty Tools/Firestore/Firestore Link")]
	public class FirestoreLink : ScriptableObject
	{
		public string projectID;

		string URL(string path) => $"https://firestore.googleapis.com/v1/projects/{projectID}/databases/(default)/documents/{path}";

		public async Task<string> GetRawData(string path)
		{
			UnityWebRequest request = UnityWebRequest.Get(URL(path));
			await request.SendWebRequest();
			return request.downloadHandler.text;
		}

		public async void GetData<T>(string path, Action<T> callback) where T : Document
		{
			T newDoc = await GetDataAsync<T>(path);
			callback?.Invoke(newDoc);
		}

		public async Task<T> GetDataAsync<T>(string path) where T : Document
		{
			UnityWebRequest request = UnityWebRequest.Get(URL(path));
			await request.SendWebRequest();
			return (request.ResponseCodeEnum().IsSuccess()) ? JsonConvert.DeserializeObject<T>(request.downloadHandler.text) : null;
		}

		public async void SetData<T>(string path, T data, Action<T> callback = null) where T : Document
		{
			T updatedDoc = await SetDataAsync<T>(path, data);
			callback?.Invoke(updatedDoc);
		}

		public async Task<T> SetDataAsync<T>(string path, T data) where T : Document
		{
			UnityWebRequest request = UnityWebRequest.Post(URL(path), data.JSON);
			await request.SendWebRequest();
			return (request.ResponseCodeEnum().IsSuccess()) ? JsonConvert.DeserializeObject<T>(request.downloadHandler.text) : null;
		}
	}
}
