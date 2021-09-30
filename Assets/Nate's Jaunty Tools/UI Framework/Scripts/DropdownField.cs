using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;
using TMPro;
using NatesJauntyTools;

public  class DropdownField : InputField
{
	[SerializeField] TMP_Dropdown tmpDropdown;
	[SerializeField] List<string> defaultOptions = new List<string>();
	public System.Type optionType = null;
	public List<object> options = new List<object>();
	public UnityEvent<string> OnSubmit;

	[InspectorButton]
	public void DebugOptions() { options.LogEach(); }

	public override string Value
	{
		get { return options[tmpDropdown.value].ToString(); }
		set
		{
			if (options.Contains(value))
			{
				tmpDropdown.value = options.IndexOf(value);
			}
			else
			{
				// Debug.LogWarning($"Couldn't find {value} in options", gameObject);
				tmpDropdown.value = 0; // Index 0 should always be the empty string option
			}
		}
	}

	public int Index
	{
		get { return tmpDropdown.value; }
		set
		{
			if (0 <= value && value <= options.LastIndex())
			{
				tmpDropdown.value = value;
			}
			else { Debug.LogWarning($"Can't set index to {value} because it would be out of range"); }
		}
	}

	public override void Submit()
	{
		OnSubmit.Invoke(Value);
	}


	#region Mono

	void Awake()
	{
		if (options.Count == 0) { SetOptions(defaultOptions); }
	}

	#endregion


	#region Options

	public void SetOptions<T>(List<T> newOptions)
	{
		options = newOptions.Cast<object>().ToList();
		optionType = typeof(T);

		tmpDropdown.ClearOptions();

		List<string> optionsToAdd = new List<string>();
		foreach (object option in options)
		{
			optionsToAdd.Add(option.ToString());
		}

		tmpDropdown.AddOptions(optionsToAdd);
	}

	public void SetOptions<T>(T enumType) where T : System.Enum
	{
		List<T> listOfEnumValues = System.Enum.GetValues(typeof(T)).Cast<T>().ToList();
		SetOptions(listOfEnumValues);
	}

	#endregion
}
