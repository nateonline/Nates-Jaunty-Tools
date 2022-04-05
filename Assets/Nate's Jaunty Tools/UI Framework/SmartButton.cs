using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NatesJauntyTools;

[ExecuteAlways]
public class SmartButton : Script
{
#if UNITY_EDITOR
	[Tooltip("(Editor ONLY) If true, the label will always match the name of the GameObject")]
	[SerializeField] bool alwaysLabelNameInEditor;
	void Update()
	{
		if (alwaysLabelNameInEditor && !Application.isPlaying && Text != name) Text = name;
	}
#endif


	[SerializeField] TMP_Text label;
	[SerializeField] Button button;

	public string Text
	{
		get => label.text;
		set => label.text = value;
	}

	public bool Interactable
	{
		get => button.interactable;
		set => button.interactable = value;
	}

	public Button.ButtonClickedEvent OnClick => button.onClick;
}
