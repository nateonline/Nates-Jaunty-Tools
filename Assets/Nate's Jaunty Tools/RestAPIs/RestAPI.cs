using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace NatesJauntyTools.RestAPIs
{
	public abstract class RestAPI : ScriptableObject
	{
		#region JSON Formatting Settings

		public static JsonSerializerSettings FriendlyJSON => new JsonSerializerSettings()
		{
			Formatting = Formatting.Indented,
			NullValueHandling = NullValueHandling.Ignore,
		};

		public static JsonSerializerSettings OptimizedJSON => new JsonSerializerSettings()
		{
			Formatting = Formatting.None,
			NullValueHandling = NullValueHandling.Ignore,
		};

		#endregion


		#region Request Tracking

		static List<UnityWebRequest> requests = new List<UnityWebRequest>();

		static async Task TrackAndSendRequest(UnityWebRequest request)
		{
			requests.Add(request);
			await request.SendWebRequest();
			requests.Remove(request);
		}

		public static void AbortAllRequests()
		{
			foreach (UnityWebRequest request in requests) { request.Abort(); }
		}

		#endregion


		#region Requests

		protected async Task<string> GetRaw(string url, Dictionary<string, string> headers = null)
		{
			UnityWebRequest request = UnityWebRequest.Get(url);
			PopulateHeaders(request, headers);

			await TrackAndSendRequest(request);

			ResponseCode responseCode = request.ResponseCodeEnum();
			if (responseCode.IsSuccess())
			{
				return request.downloadHandler.text;
			}
			else
			{
				LogRestError(request);
				return null;
			}
		}

		protected async Task<T> Get<T>(string url, Dictionary<string, string> headers = null)
		{
			UnityWebRequest request = UnityWebRequest.Get(url);
			PopulateHeaders(request, headers);

			await TrackAndSendRequest(request);

			return ValidateResponseObject<T>(request);
		}

		protected async Task<T> Post<T>(string url, T data, Dictionary<string, string> headers = null)
		{
			UnityWebRequest request = UnityWebRequest.Post(url, JsonConvert.SerializeObject(data));
			PopulateHeaders(request, headers);

			await TrackAndSendRequest(request);

			return ValidateResponseObject<T>(request);
		}

		protected async Task<T> Patch<T>(string url, T data, Dictionary<string, string> headers = null)
		{
			UnityWebRequest request = UnityWebRequest.Put(url, JsonConvert.SerializeObject(data));
			request.method = "PATCH";
			PopulateHeaders(request, headers);

			await TrackAndSendRequest(request);

			return ValidateResponseObject<T>(request);
		}

		#endregion


		#region Request Helpers

		void PopulateHeaders(UnityWebRequest request, Dictionary<string, string> headers)
		{
			if (headers != null)
			{
				foreach (var header in headers) { request.SetRequestHeader(header.Key, header.Value); }
			}
		}

		T ValidateResponseObject<T>(UnityWebRequest request)
		{
			ResponseCode responseCode = request.ResponseCodeEnum();
			if (responseCode.IsSuccess())
			{
				return JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
			}
			else
			{
				LogRestError(request);
				return default(T);
			}
		}

		void LogRestError(UnityWebRequest request)
		{
			StringBuilder str = new StringBuilder();
			str.Append($"Rest API Error! Response Code: {request.ResponseCodeEnum().FullName()}");
			str.Append($"\nMethod: {request.method}");
			str.Append($"\nURL: {request.url}");
			str.Append("\n\nHeaders:\n");
			foreach (var header in request.GetResponseHeaders()) { str.Append($"\t{header.Key} = {header.Value}\n"); }
			str.Append($"\nBody:\n");
			str.Append($"\t{request.downloadHandler.text}");
			str.Append("\n\n");
			Debug.LogError(str.ToString());
		}

		#endregion
	}
}
