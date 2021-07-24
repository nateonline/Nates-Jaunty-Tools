using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace NatesJauntyTools.GoogleSheets
{
	[CreateAssetMenu(menuName = "Nate's Jaunty Tools/Google Sheets/Google Sheet Link", fileName = "New Google Sheet Link")]
	public class GoogleSheetLink : ScriptableObject
	{
		#region Global Variables

		string[] Scopes = { SheetsService.Scope.Spreadsheets };
		SheetsService service;

		[Tooltip("Copied from the URL bar when viewing the google sheet (in between the '/d/' and '/edit')")]
		[SerializeField] string spreadsheetID = "";

		[Tooltip("The name of the json asset. Includes .json and must be in the StreamingAssets folder")]
		[SerializeField] string secretsPath = "client_secrets.json";

		#endregion


		#region Setup

		public void Initialize()
		{
			GoogleCredential credential = null;
			string streamedPath = $"{Application.streamingAssetsPath}/{secretsPath}";
			try
			{
				using (var stream = new FileStream(streamedPath, FileMode.Open, FileAccess.Read))
				{
					credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
				}
			}
			catch (Exception e)
			{
				Debug.LogWarning($"{name} initialization failed: [{e.Message}] when using secrets.json path [{streamedPath}]");
				return;
			}

			service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = name,
			});

			if (service == null) { Debug.LogWarning($"{name} initialization failed: Service wasn't created correctly"); }
		}

		bool IsInitializedForOperation(string operationName)
		{
			if (service == null)
			{
				Debug.LogWarning($"{name} can't perform {operationName} operation because it isn't initialized!");
				return false;
			}
			else return true;
		}

		#endregion


		#region Data Functions

		public object GetCell(string range)
		{
			if (range.Contains(":"))
			{
				Debug.LogWarning($"Can't get cell with {range}, use GetRange instead");
				return null;
			}
			else
			{
				var request = service.Spreadsheets.Values.Get(spreadsheetID, range);

				var response = request.Execute();
				var values = response.Values;

				if (values == null || values.Count == 0)
				{
					return null;
				}
				else if (values.Count > 1)
				{
					Debug.LogWarning($"Can't get multiple values with GetCell, use GetRange instead");
					return null;
				}
				else return values[0][0];
			}
		}

		public void SetCell(string range, object value)
		{
			if (range.Contains(":"))
			{
				Debug.LogWarning($"Can't get cell with {range}, use GetRange instead");
			}
			else
			{
				var valueRange = new ValueRange();
				valueRange.Values = new List<IList<object>> { new List<object>() { value } };

				var request = service.Spreadsheets.Values.Update(valueRange, spreadsheetID, range);
				request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

				var response = request.Execute();
			}
		}

		public IList<IList<object>> GetRange(string range)
		{
			if (IsInitializedForOperation("GetRange"))
			{
				var request = service.Spreadsheets.Values.Get(spreadsheetID, range);

				try
				{
					var response = request.Execute();
					var values = response.Values;
					if (values != null && values.Count > 0) { return values; }
				}
				catch
				{
					Debug.LogWarning($"Error reading range {range} - Check to make sure the range is spelled correctly.");
				}
			}

			return null;
		}

		public System.Threading.Tasks.Task AppendRow(string range, List<object> values)
		{
			return System.Threading.Tasks.Task.Factory.StartNew(() =>
			{
				if (IsInitializedForOperation("AppendRow"))
				{
					var valueRange = new ValueRange();
					valueRange.Values = new List<IList<object>> { values };

					var request = service.Spreadsheets.Values.Append(valueRange, spreadsheetID, range);
					request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

					var appendResponse = request.Execute();
				}
			});
		}

		#endregion


		#region Helper Functions

		public static string RowA1Notation(List<object> rowData)
		{
			char start = 'A';
			char end = (char)((rowData.Count - 1) + 'A');
			return $"{start}:{end}";
		}

		#endregion
	}
}
