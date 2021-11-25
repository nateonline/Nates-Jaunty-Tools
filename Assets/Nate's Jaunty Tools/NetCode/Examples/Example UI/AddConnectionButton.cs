using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace NatesJauntyTools.Examples.NetCode
{
	public class AddConnectionButton : Script
	{
		public bool Interactable
		{
			get => button.interactable;
			set
			{
				button.interactable = value;
				label.color = (value) ? normalTextColor : disabledTextColor;
			}
		}

		[SerializeField] Button button;
		[SerializeField] TMP_Text label;
		Color normalTextColor;
		[SerializeField] Color disabledTextColor;


		void Awake()
		{
			normalTextColor = label.color;
			label.color = (button.interactable) ? normalTextColor : disabledTextColor;
		}

		[InspectorButton]
		public void ToggleInteractable() => Interactable = !Interactable;
	}
}
