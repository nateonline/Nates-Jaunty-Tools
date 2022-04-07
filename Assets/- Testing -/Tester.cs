using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NatesJauntyTools;
using NatesJauntyTools.GoogleSheets;

public class Tester : Script
{
	[SerializeField] TMP_Text timer;
	void Update() => timer.text = Time.time.ToString("n2");


	[SerializeField] GoogleSheetLink googleSheetLink;
	[SerializeField] string getRange, getCell, setRange, setCell, appendRow;


	[InspectorButton]
	public void GetRange()
	{
		object cellValue = googleSheetLink.GetRange(getRange);
		Debug.Log(cellValue.ToString());
		googleSheetLink.Shutdown();
	}

	[InspectorButton]
	public void GetRange_Generic()
	{
		List<TestRow> testRows = googleSheetLink.GetRange<TestRow>(getRange);
		int sum = 0;
		foreach (var row in testRows)
		{
			sum += row.a;
			sum += row.b;
			sum += row.c;
		}
		Debug.Log($"Sum = {sum}");
		googleSheetLink.Shutdown();
	}

	[InspectorButton]
	public async void GetRange_Async()
	{
		object cellValue = await googleSheetLink.GetRange_Async(getRange);
		Debug.Log(cellValue.ToString());
		googleSheetLink.Shutdown();
	}

	[InspectorButton]
	public async void GetRange_Async_Generic()
	{
		List<TestRow> testRows = await googleSheetLink.GetRange_Async<TestRow>(getRange);
		int sum = 0;
		foreach (var row in testRows)
		{
			sum += row.a;
			sum += row.b;
			sum += row.c;
		}
		Debug.Log($"Sum = {sum}");
		googleSheetLink.Shutdown();
	}

	[InspectorButton]
	public void GetCell()
	{
		object cellValue = googleSheetLink.GetCell(getCell);
		Debug.Log(cellValue.ToString());
		googleSheetLink.Shutdown();
	}

	[InspectorButton]
	public void GetCell_Generic()
	{
		string cellValue = googleSheetLink.GetCell<string>(getCell);
		Debug.Log(cellValue);
		googleSheetLink.Shutdown();
	}

	[InspectorButton]
	public async void GetCell_Async()
	{
		object cellValue = await googleSheetLink.GetCell_Async(getCell);
		Debug.Log(cellValue.ToString());
		googleSheetLink.Shutdown();
	}

	[InspectorButton]
	public async void GetCell_Async_Generic()
	{
		string cellValue = await googleSheetLink.GetCell_Async<string>(getCell);
		Debug.Log(cellValue);
		googleSheetLink.Shutdown();
	}

	[InspectorButton]
	public void GetCell_Callback()
	{
		googleSheetLink.GetCell_Callback(getCell, OnGetCell);


		void OnGetCell(object cellValue)
		{
			Debug.Log(cellValue.ToString());
			googleSheetLink.Shutdown();
		}
	}

	[InspectorButton]
	public void GetCell_Callback_Generic()
	{
		googleSheetLink.GetCell_Callback<string>(getCell, OnGetCell);


		void OnGetCell(string cellValue)
		{
			Debug.Log(cellValue);
			googleSheetLink.Shutdown();
		}
	}


	List<TestRow> rows = new List<TestRow>()
	{
		new TestRow()
		{
			a = 2,
			b = 2,
			c = 2,
		},
		new TestRow()
		{
			a = 2,
			b = 2,
			c = 2,
		},
		new TestRow()
		{
			a = 2,
			b = 2,
			c = 2,
		},
	};

	[InspectorButton]
	public void SetRange()
	{
		googleSheetLink.SetRange(setRange, rows);
	}

	[InspectorButton]
	public void SetCell()
	{
		googleSheetLink.SetCell(setCell, "asdf");
	}
}

public class TestRow : GoogleSheetRow
{
	public int a, b, c;

	public override void Deserialize(List<object> rowValues)
	{
		a = rowValues[0].CellTo<int>();
		b = rowValues[1].CellTo<int>();
		c = rowValues[2].CellTo<int>();
	}

	public override List<object> Serialize() => new List<object>()
	{
		a,
		b,
		c
	};
}
