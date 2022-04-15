using System.Collections;
using System.Collections.Generic;
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

		public async Task<T> GetData<T>(string path) where T : Document
		{
			UnityWebRequest request = UnityWebRequest.Get(URL(path));
			await request.SendWebRequest();
			if (request.ResponseCodeEnum().IsSuccess())
			{
				Debug.Log($"Firestore deserializing data");
				return JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
			}
			else return null;
		}
	}
}
