using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NatesJauntyTools;
using NatesJauntyTools.GoogleSheets;

public class Tester : Script
{
	[SerializeField] GoogleSheetLink googleSheetLink;

	[InspectorButton]
	public void GetA1()
	{
		Debug.Log(googleSheetLink.GetCell("'Sheet1'!A1").ToString());
	}

	[InspectorButton]
	public void LogEnums()
	{
		foreach (var logType in Tools.GetEnumValues<Console.LogType>())
		{
			Debug.Log(logType);
		}
	}
}
