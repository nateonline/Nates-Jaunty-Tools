using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using NatesJauntyTools.RestAPIs;

namespace NatesJauntyTools.Firebase
{
	// API KEY = AIzaSyBczzeyjR6iXU09s6SHwS7VxxVNx84Luuo
	// https://firebase.google.com/docs/firestore/use-rest-api
	// Use Google Identity OAuth 2.0 and service account


	[CreateAssetMenu(menuName = "Nate's Jaunty Tools/Firestore/Firestore API")]
	public class FirestoreAPI : RestAPI
	{
		[SerializeField] string projectID;

		string BaseURL => $"https://firestore.googleapis.com/v1/projects/{projectID}/databases/(default)/documents/";

		Dictionary<string, string> Headers => new Dictionary<string, string>()
		{ };


		public async void GetDocument<T>(string path, Action<T> callback) where T : Document
		{
			callback?.Invoke(await GetDocumentAsync<T>(path));
		}

		public async Task<T> GetDocumentAsync<T>(string path) where T : Document
		{
			return await Get<T>(BaseURL + path, Headers);
		}

		public async void SetDocument<T>(string path, T document, Action<T> callback = null) where T : Document
		{
			callback?.Invoke(await SetDocumentAsync<T>(path, document));
		}

		public async Task<T> SetDocumentAsync<T>(string path, T document) where T : Document
		{
			return await Patch<T>(BaseURL + path, document);
		}

		public async void GetCollection<T>(string path, Action<List<T>> callback) where T : Document
		{
			callback?.Invoke(await GetCollectionAsync<T>(path));
		}

		public async Task<List<T>> GetCollectionAsync<T>(string path) where T : Document
		{
			string json = await GetRaw(BaseURL + path);
			if (json == null) return new List<T>();

			int trim = 17; // Removes "documents" prefix
			string modifiedJson = json.Substring(trim, json.Length - (trim + 3)); // Removes last line and last "}"

			return JsonConvert.DeserializeObject<List<T>>(modifiedJson);
		}
	}
}
