using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NatesJauntyTools;

public class Tester : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		Console.Log("Info", ConsoleLogType.Info);
		Console.Log("Debug", ConsoleLogType.Debug);
		Console.Log("Network", ConsoleLogType.Network);
		Console.Log("Database", ConsoleLogType.Database);
		Console.Log("Assert", ConsoleLogType.Assert);
		Console.Log("Warning", ConsoleLogType.Warning);
		Console.Log("Error", ConsoleLogType.Error);
		Console.Log("Exception", ConsoleLogType.Exception);

		Debug.Log("Debug.Log");
		Debug.LogWarning("Debug.LogWarning");
		Debug.LogError("Debug.LogError");
	}

	// Update is called once per frame
	void Update()
	{

	}
}
