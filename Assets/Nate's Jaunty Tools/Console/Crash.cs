using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Crash : MonoBehaviour
{
	public TMP_Text message;
	public TMP_Text stackTrace;

	public void SetData(string message, string stackTrace)
	{
		this.message.text = message;
		this.stackTrace.text = stackTrace;
	}
}
