using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace NatesJauntyTools
{
	public class ConsoleLogFilter : MonoBehaviour
	{
		public Console.LogType logType;
		[SerializeField] Toggle toggle;
		[SerializeField] TMP_Text label;

		public bool IsOn
		{
			get => toggle.isOn;
			set => toggle.isOn = value;
		}

		public string Text
		{
			get => label.text;
			set => label.text = value;
		}

		public Toggle.ToggleEvent OnValueChanged => toggle.onValueChanged;


		public void SetData(Console.LogType logType)
		{
			this.logType = logType;
			IsOn = true;
			Text = logType.ToString();
		}
	}
}
