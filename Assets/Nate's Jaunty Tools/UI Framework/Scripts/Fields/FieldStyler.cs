using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using NatesJauntyTools;

public class FieldStyler : Script
{
	[Header("General")]
	public string labelText;

	[Header("Label & Field Refs")]
	public RectTransform labelTransform;
	public TMP_Text label;
	public RectTransform fieldArea;

	public string Label
	{
		get { return label.text; }
		set { label.text = value; }
	}

	void OnValidate()
	{
		if (label != null)
		{
			if (!string.IsNullOrEmpty(labelText) && !string.IsNullOrWhiteSpace(labelText))
			{
				label.text = labelText;
			}
			else
			{
				label.text = name;
			}
		}
		else { Debug.LogWarning($"Can't set label for {name} because the TMP wasn't set in the inspector!", gameObject); }
	}
}
