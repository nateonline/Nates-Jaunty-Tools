using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using NatesJauntyTools;

public enum SelectionType { Unselected, Selected, Mixed }

public class CheckboxField : InputField
{
	[SerializeField] protected Button button;
	[SerializeField] protected Image image;
	[SerializeField] protected Sprite falseIcon;
	[SerializeField] protected Sprite trueIcon;
	[SerializeField] protected Sprite mixedIcon;
	[SerializeField] SelectionType selectionValue; public SelectionType SelectionValue
	{
		get { return selectionValue; }
		set { selectionValue = value; UpdateSprite(); }
	}

	public UnityEvent<SelectionType> OnSubmit;

	public override string Value
	{
		get { return selectionValue.ToString(); }
		set
		{
			if (SelectionType.TryParse(value, true, out SelectionType parsedSelectionType)) {SelectionValue = parsedSelectionType;}
			else { Debug.LogWarning($"CheckboxField didn't understand \"{value}\" when trying to set its value"); }
		}
	}

	void OnValidate()
	{
		Debug.Log("OnValidate");
		
		SelectionValue = selectionValue;
	}

	public override void Submit()
	{
		OnSubmit.Invoke(SelectionValue);
	}

	public void OnButtonClick()
	{
		switch (SelectionValue)
		{
			case SelectionType.Unselected:
			case SelectionType.Mixed:
				SelectionValue = SelectionType.Selected;
				break;

			case SelectionType.Selected:
				SelectionValue = SelectionType.Unselected;
				break;
		}
	}

	void UpdateSprite()
	{
		Sprite iconToDisplay = null;

		switch (selectionValue)
		{
			case SelectionType.Unselected: iconToDisplay = falseIcon; break;
			case SelectionType.Selected: iconToDisplay = trueIcon; break;
			case SelectionType.Mixed: iconToDisplay = mixedIcon; break;
		}

		image.enabled = (iconToDisplay != null);
		image.sprite = iconToDisplay;
	}
}
