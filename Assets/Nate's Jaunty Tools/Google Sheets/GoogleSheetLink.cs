using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace NatesJauntyTools.GoogleSheets
{
	[CreateAssetMenu(menuName = "Nate's Jaunty Tools/Google Integrations/Google Sheet Link", fileName = "New Google Sheet Link")]
	public class GoogleSheetLink : ScriptableObject
	{
		#region Global Variables

		readonly string[] scopes = { SheetsService.Scope.Spreadsheets };

		SheetsService service;
		public bool IsInitialized => service != null;

		[Tooltip("False = Initialize when used for the first time\nTrue = Initialize on ScriptableObject Awake()")]
		[SerializeField] bool initializeOnAwake;

		[Tooltip("Copied from the URL bar when viewing the Google Sheet (in between the '/d/' and '/edit')")]
		[SerializeField] string spreadsheetID = "";

		[Tooltip("Copied from the client_secrets.json file generated from your service account")]
		[SerializeField, TextArea] string clientSecretsJSON;

		#endregion


		#region Setup

		void Awake() { if (initializeOnAwake) VerifyService(); }

		void VerifyService()
		{
			if (!IsInitialized)
			{
				service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
				{
					HttpClientInitializer = GoogleCredential.FromJson(clientSecretsJSON).CreateScoped(scopes),
					ApplicationName = name,
				});

				if (!IsInitialized) // Still isn't initialized?
				{
					throw new Exception($"{name} initialization failed: Service wasn't created correctly");
				}
			}
		}

		#endregion


		#region Data Functions

		/// <param name="range"> In "'MySheet'!A1" format </param>
		public object GetCell(string range)
		{
			VerifyService();

			if (range.Contains(":"))
			{
				throw new ArgumentException($"Can't use GetCell with \"{range}\", use GetRange instead");
			}

			try
			{
				var request = service.Spreadsheets.Values.Get(spreadsheetID, range);
				var response = request.Execute();
				var values = response.Values;
				return (values != null && values.Count > 0) ? values[0][0] : null;
			}
			catch
			{
				throw new Exception($"Error reading range \"{range}\"! Check to make sure the range is spelled correctly");
			}
		}

		/// <param name="range"> In "'MySheet'!A1" format </param>
		public void SetCell(string range, object value)
		{
			VerifyService();

			if (range.Contains(":"))
			{
				throw new ArgumentException($"Can't use SetCell with \"{range}\", use SetRange instead");
			}

			try
			{
				var valueRange = new ValueRange();
				valueRange.Values = new List<IList<object>> { new List<object>() { value } };

				var request = service.Spreadsheets.Values.Update(valueRange, spreadsheetID, range);
				request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

				var response = request.Execute();
			}
			catch
			{
				throw new Exception($"Error reading range \"{range}\"! Check to make sure the range is spelled correctly");
			}
		}

		/// <param name="range"> In "'MySheet'!A1:B2" format </param>
		public List<List<object>> GetRange(string range)
		{
			VerifyService();

			var request = service.Spreadsheets.Values.Get(spreadsheetID, range);

			try
			{
				var response = request.Execute();
				var values = response.Values;
				return (values != null && values.Count > 0) ? values as List<List<object>> : null;
			}
			catch
			{
				throw new Exception($"Error reading range \"{range}\"! Check to make sure the range is spelled correctly");
			}
		}

		/// <param name="range"> In "'MySheet'!A1:B2" format </param>
		public void SetRange(string range, List<List<object>> values)
		{
			VerifyService();

			try
			{
				var valueRange = new ValueRange();
				valueRange.Values = values as IList<IList<object>>;

				var request = service.Spreadsheets.Values.Update(valueRange, spreadsheetID, range);
				request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

				var response = request.Execute();
			}
			catch
			{
				throw new Exception($"Error reading range \"{range}\"! Check to make sure the range is spelled correctly");
			}
		}

		/// <param name="range"> Only the sheet name and column letters are required (ex: 'MySheet'!A:F) </param>
		public void AppendRow(string range, List<object> values)
		{
			VerifyService();

			try
			{
				var valueRange = new ValueRange();
				valueRange.Values = new List<IList<object>> { values };

				var request = service.Spreadsheets.Values.Append(valueRange, spreadsheetID, range);
				request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

				var appendResponse = request.Execute();
			}
			catch
			{
				throw new Exception($"Error reading range \"{range}\"! Check to make sure the range is spelled correctly");
			}
		}

		/// <param name="range"> Only the sheet name and column letters are required (ex: 'MySheet'!A:F) </param>
		public Task AppendRowAsync(string range, List<object> values)
		{
			return Task.Factory.StartNew(() => AppendRow(range, values));
		}

		#endregion
	}
}
