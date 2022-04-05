using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NatesJauntyTools;

public class CrashHandler : Script
{
	[SerializeField] GameObject body;
	[SerializeField] RectTransform scrollView;
	[SerializeField] int maxCrashes = 10;
	[SerializeField] GameObject crashPrefab;
	[SerializeField] Transform crashParent;
	List<Crash> crashes = new List<Crash>();

	[SerializeField] SmartButton copyButton;

	const int CLIPBOARD_SEPARATOR_LENGTH = 25;
	const char CLIPBOARD_SEPARATOR_CHAR = '—';


	void Awake()
	{
		Application.logMessageReceived += OnLogMessage;
		body.SetActive(false);
	}

	void OnDestroy()
	{
		Application.logMessageReceived -= OnLogMessage;
	}

	[InspectorButton]
	public void TestCrash() => Debug.LogError("Testing Crash Handler");

	void OnLogMessage(string message, string stackTrace, LogType logType)
	{
		switch (logType)
		{
			case LogType.Error:
			case LogType.Exception:
			case LogType.Assert:
				body.SetActive(true);
				if (crashes.Count < maxCrashes)
				{
					Crash newCrash = Instantiate(crashPrefab, crashParent).GetComponent<Crash>();
					newCrash.SetData(message, stackTrace);
					crashes.Add(newCrash);
					LayoutRebuilder.ForceRebuildLayoutImmediate(scrollView.GetComponent<RectTransform>());
				}
				break;
		}
	}

	public void CopyErrorsToClipboard()
	{
		StringBuilder stringBuilder = new StringBuilder();

		foreach (Crash crash in crashes)
		{
			stringBuilder.Append(crash.message.text);
			stringBuilder.Append("\n\n");
			stringBuilder.Append(crash.stackTrace.text);
			stringBuilder.Append("\n");

			for (int i = 0; i < CLIPBOARD_SEPARATOR_LENGTH; i++) { stringBuilder.Append(CLIPBOARD_SEPARATOR_CHAR); }
		}

		Runtime.CopyToClipboard(stringBuilder.ToString());

		copyButton.Text = "Text Copied!";
		copyButton.Interactable = false;
	}

	public void Quit() => Runtime.Quit();
}
