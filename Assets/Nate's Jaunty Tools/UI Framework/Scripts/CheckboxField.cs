using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public enum DisplayType { Unselected, Selected, MixedSelection }

public class CheckboxField : InputField
{
	[SerializeField] protected TMP_Dropdown checkbox;
	[SerializeField] List<string> defaultOptions = new List<string>();
	public System.Type optionType = null;
	public List<object> options = new List<object>();

	public UnityEvent<string> OnSubmit;

	public override string Value
	{
		get { return options[checkbox.value].ToString(); }
		set { checkbox.value = options.IndexOf(value); }
	}

	public override void Submit()
	{
		OnSubmit.Invoke(Value);
	}
}
