using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TextField : InputField
{
	[SerializeField] protected TMP_InputField tmpInputField;

	public string Placeholder
	{
		get { return tmpInputField.placeholder.GetComponent<TMP_Text>().text; }
		set { tmpInputField.placeholder.GetComponent<TMP_Text>().text = value; }
	}

	public UnityEvent<string> OnSubmit;

	public override string Value
	{
		get { return tmpInputField.text; }
		set { tmpInputField.text = value; }
	}

	public override void Submit()
	{
		if (string.IsNullOrEmpty(Value) || string.IsNullOrWhiteSpace(Value)) { Value = ""; }

		OnSubmit.Invoke(Value);
	}
}
