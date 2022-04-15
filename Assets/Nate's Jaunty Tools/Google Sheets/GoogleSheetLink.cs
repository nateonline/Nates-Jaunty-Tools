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
		public bool IsSetup => service != null;

		[Tooltip("Copied from the URL bar when viewing the Google Sheet (in between the '/d/' and '/edit')")]
		[SerializeField] string spreadsheetID = "";

		[Tooltip("Copied from the client_secrets.json file generated from your service account")]
		[SerializeField, TextArea] string clientSecretsJSON;

		#endregion


		#region Setup

		public void Setup() => Setup(name);

		public void Setup(string objectName)
		{
			service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
			{
				HttpClientInitializer = GoogleCredential.FromJson(clientSecretsJSON).CreateScoped(scopes),
				ApplicationName = objectName,
			});

			if (!IsSetup) // Still isn't setup?
			{
				throw new Exception($"{objectName} Setup Failed: Service wasn't created correctly");
			}
		}

		public void Shutdown() { service = null; }

		Task Setup_Async()
		{
			string objectName = name;
			return Task.Factory.StartNew(() => Setup(objectName));
		}

		#endregion


		#region Get Range

		/// <param name="range"> Example: 'My Sheet'!A1:B2 </param>
		public List<List<object>> GetRange(string range)
		{
			Setup();

			var request = service.Spreadsheets.Values.Get(spreadsheetID, range);

			try
			{
				var response = request.Execute();
				var rowValues = response.Values;
				List<List<object>> values = new List<List<object>>();
				foreach (var row in rowValues) { values.Add(row as List<object>); }
				return (values != null && values.Count > 0) ? values : null;
			}
			catch
			{
				throw new Exception($"Error reading range \"{range}\"! Check to make sure the range is spelled correctly");
			}
		}

		/// <param name="range"> Example: 'My Sheet'!A1:B2 </param>
		public List<T> GetRange<T>(string range) where T : GoogleSheetRow, new()
		{
			List<T> rows = new List<T>();
			foreach (var rowValues in GetRange(range))
			{
				T newRow = new T();
				newRow.Deserialize(rowValues);
				rows.Add(newRow);
			}
			return rows;
		}

		/// <param name="range"> Example: 'My Sheet'!A1:B2 </param>
		public async Task<List<List<object>>> GetRange_Async(string range)
		{
			await Setup_Async();

			var request = service.Spreadsheets.Values.Get(spreadsheetID, range);

			try
			{
				var response = await request.ExecuteAsync();
				var rowValues = response.Values;
				List<List<object>> values = new List<List<object>>();
				foreach (var row in rowValues) { values.Add(row as List<object>); }
				return (values != null && values.Count > 0) ? values : null;
			}
			catch
			{
				throw new Exception($"Error reading range \"{range}\"! Check to make sure the range is spelled correctly");
			}
		}

		/// <param name="range"> Example: 'My Sheet'!A1:B2 </param>
		public async Task<List<T>> GetRange_Async<T>(string range) where T : GoogleSheetRow, new()
		{
			List<T> rows = new List<T>();
			List<List<object>> tableValues = await GetRange_Async(range);
			foreach (var rowValues in tableValues)
			{
				T newRow = new T();
				newRow.Deserialize(rowValues);
				rows.Add(newRow);
			}
			return rows;
		}

		/// <param name="range"> Example: 'My Sheet'!A1:B2 </param>
		public async void GetRange_Callback(string range, Action<List<List<object>>> callback)
		{
			List<List<object>> tableValues = await GetRange_Async(range);
			callback(tableValues);
		}

		/// <param name="range"> Example: 'My Sheet'!A1:B2 </param>
		public async void GetRange_Callback<T>(string range, Action<List<T>> callback) where T : GoogleSheetRow, new()
		{
			List<T> rows = await GetRange_Async<T>(range);
			callback(rows);
		}

		#endregion


		#region Set Range

		/// <param name="range"> Example: 'My Sheet'!A1:B2 </param>
		public void SetRange(string range, List<List<object>> values)
		{
			Setup();

			try
			{
				var valueRange = new ValueRange();
				List<IList<object>> tableValues = new List<IList<object>>();
				foreach (var rowValues in values) { tableValues.Add(rowValues); }
				valueRange.Values = tableValues;

				var request = service.Spreadsheets.Values.Update(valueRange, spreadsheetID, range);
				request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

				var response = request.Execute();
			}
			catch
			{
				throw new Exception($"Error writing to range \"{range}\"! Check to make sure the range is spelled correctly");
			}
		}

		/// <param name="range"> Example: 'My Sheet'!A1:B2 </param>
		public void SetRange<T>(string range, List<T> rows) where T : GoogleSheetRow
		{
			SetRange(range, rows.ToTable());
		}

		/// <param name="range"> Example: 'My Sheet'!A1:B2 </param>
		public async Task SetRange_Async(string range, List<List<object>> values)
		{
			await Setup_Async();

			try
			{
				var valueRange = new ValueRange();
				List<IList<object>> tableValues = new List<IList<object>>();
				foreach (var rowValues in values) { tableValues.Add(rowValues); }
				valueRange.Values = tableValues;

				var request = service.Spreadsheets.Values.Update(valueRange, spreadsheetID, range);
				request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

				Debug.Log(request.Range);
				var response = await request.ExecuteAsync();
				Debug.Log(response.UpdatedCells);
			}
			catch
			{
				throw new Exception($"Error writing to range \"{range}\"! Check to make sure the range is spelled correctly");
			}
		}

		/// <param name="range"> Example: 'My Sheet'!A1:B2 </param>
		public async Task SetRange_Async<T>(string range, List<T> rows) where T : GoogleSheetRow
		{
			List<List<object>> tableValues = new List<List<object>>();
			foreach (T row in rows) { tableValues.Add(row.Serialize()); }
			await SetRange_Async(range, tableValues);
		}

		/// <param name="range"> Example: 'My Sheet'!A1:B2 </param>
		public async void SetRange_Callback(string range, List<List<object>> values, Action callback)
		{
			await SetRange_Async(range, values);
			callback();
		}

		/// <param name="range"> Example: 'My Sheet'!A1:B2 </param>
		public async void SetRange_Callback<T>(string range, List<T> rows, Action callback) where T : GoogleSheetRow, new()
		{
			await SetRange_Async<T>(range, rows);
			callback();
		}

		#endregion


		void VerifyCellAddress(string range)
		{
			if (range.Contains(":"))
			{
				throw new ArgumentException($"Can't use GetCell/SetCell with \"{range}\", use GetRange/SetRange instead");
			}
		}


		#region Get Cell

		/// <param name="range"> Example: 'My Sheet'!A1 </param>
		public object GetCell(string range)
		{
			VerifyCellAddress(range);
			var values = GetRange(range);
			return (values != null && values.Count > 0) ? values[0][0] : null;
		}

		/// <param name="range"> Example: 'My Sheet'!A1 </param>
		public T GetCell<T>(string range)
		{
			return GetCell(range).CellTo<T>();
		}

		/// <param name="range"> Example: 'My Sheet'!A1 </param>
		public async Task<object> GetCell_Async(string range)
		{
			VerifyCellAddress(range);

			var values = await GetRange_Async(range);
			return (values != null && values.Count > 0) ? values[0][0] : null;
		}

		/// <param name="range"> Example: 'My Sheet'!A1 </param>
		public async Task<T> GetCell_Async<T>(string range)
		{
			object cellValue = await GetCell_Async(range);
			return cellValue.CellTo<T>();
		}

		/// <param name="range"> Example: 'My Sheet'!A1 </param>
		public async void GetCell_Callback(string range, Action<object> callback)
		{
			object cellValue = await GetCell_Async(range);
			callback(cellValue);
		}

		/// <param name="range"> Example: 'My Sheet'!A1 </param>
		public async void GetCell_Callback<T>(string range, Action<T> callback)
		{
			T cellValue = await GetCell_Async<T>(range);
			callback(cellValue);
		}

		#endregion


		#region Set Cell

		List<List<object>> ObjectToTable(object value) => new List<List<object>>() { new List<object>() { value } };

		/// <param name="range"> Example: 'My Sheet'!A1 </param>
		public void SetCell(string range, object value)
		{
			VerifyCellAddress(range);
			SetRange(range, ObjectToTable(value));
		}

		/// <param name="range"> Example: 'My Sheet'!A1 </param>
		public async Task SetCell_Async(string range, object value)
		{
			VerifyCellAddress(range);
			await SetRange_Async(range, ObjectToTable(value));
		}

		/// <param name="range"> Example: 'My Sheet'!A1 </param>
		public async void SetCell_Callback(string range, object value, Action callback)
		{
			VerifyCellAddress(range);

			Debug.Log($"Value to set: {value}");
			var table = ObjectToTable(value);
			Debug.Log($"Value in table: {table[0][0]}");
			await SetRange_Async(range, table);
			callback();
		}

		#endregion


		#region Append Row

		/// <summary> Only the sheet name and column letters are required (Example: 'My Sheet'!A:F) </summary>
		/// <param name="range"> Example: 'My Sheet'!A:F (only column letters are required) </param>
		public void AppendRow(string range, List<object> values)
		{
			Setup();

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

		/// <summary> Only the sheet name and column letters are required (Example: 'My Sheet'!A:F) </summary>
		/// <param name="range"> Example: 'My Sheet'!A:F (only column letters are required) </param>
		public void AppendRow<T>(string range, T row) where T : GoogleSheetRow
		{
			AppendRow(range, row.Serialize());
		}

		/// <summary> Only the sheet name and column letters are required (Example: 'My Sheet'!A:F) </summary>
		/// <param name="range"> Example: 'My Sheet'!A:F (only column letters are required) </param>
		public async Task AppendRow_Async(string range, List<object> values)
		{
			await Setup_Async();

			try
			{
				var valueRange = new ValueRange();
				valueRange.Values = new List<IList<object>> { values };

				var request = service.Spreadsheets.Values.Append(valueRange, spreadsheetID, range);
				request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

				var appendResponse = await request.ExecuteAsync();
			}
			catch
			{
				throw new Exception($"Error reading range \"{range}\"! Check to make sure the range is spelled correctly");
			}
		}

		/// <summary> Only the sheet name and column letters are required (Example: 'My Sheet'!A:F) </summary>
		/// <param name="range"> Example: 'My Sheet'!A:F (only column letters are required) </param>
		public async Task AppendRow_Async<T>(string range, T row) where T : GoogleSheetRow
		{
			await AppendRow_Async(range, row.Serialize());
		}

		/// <summary> Only the sheet name and column letters are required (Example: 'My Sheet'!A:F) </summary>
		/// <param name="range"> Example: 'My Sheet'!A:F (only column letters are required) </param>
		public async void AppendRow_Callback(string range, List<object> values, Action callback)
		{
			await AppendRow_Async(range, values);
			callback();
		}

		/// <summary> Only the sheet name and column letters are required (Example: 'My Sheet'!A:F) </summary>
		/// <param name="range"> Example: 'My Sheet'!A:F (only column letters are required) </param>
		public async void AppendRow_Callback<T>(string range, T row, Action callback) where T : GoogleSheetRow
		{
			await AppendRow_Async(range, row);
			callback();
		}

		#endregion
	}
}
