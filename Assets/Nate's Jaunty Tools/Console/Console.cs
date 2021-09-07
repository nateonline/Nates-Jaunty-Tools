using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace NatesJauntyTools
{
	public enum ConsoleTimestampMode { None, RunTime, RealTime, DateTime, DateTimeZone }
	public enum ConsoleLogType { Info, Debug, Network, Database, Assert, Warning, Error, Exception }

	public class Console : Singleton<Console>
	{
		static List<Log> logs = new List<Log>();
		static List<string> commandHistory = new List<string>();
		static int commandHistoryIndex = -1;
		static List<Command> commands = new List<Command>();

		[Header("Scene Refs")]
		public GameObject window;
		public ScrollRect scrollRect;
		public TMP_Text logsText;
		public TMP_InputField commandField;

		[Header("Key Binding")]
		public List<KeyCode> keyBinding = new List<KeyCode>();

		[Header("Log Settings")]
		[SerializeField] Toggle logTimestampNone;
		[SerializeField] Toggle logTimestampRunTime;
		[SerializeField] Toggle logTimestampRealTime;
		[SerializeField] Toggle logTimestampDateTime;
		[SerializeField] Toggle logTimestampDateTimeZone;
		public ConsoleTimestampMode CurrentTimestampMode
		{
			get
			{
				if (logTimestampNone.isOn) return ConsoleTimestampMode.None;
				else if (logTimestampRunTime.isOn) return ConsoleTimestampMode.RunTime;
				else if (logTimestampRealTime.isOn) return ConsoleTimestampMode.RealTime;
				else if (logTimestampDateTime.isOn) return ConsoleTimestampMode.DateTime;
				else if (logTimestampDateTimeZone.isOn) return ConsoleTimestampMode.DateTimeZone;
				else return ConsoleTimestampMode.RunTime;
			}
		}
		[SerializeField] Toggle includeInfoLogs;
		[SerializeField] Toggle includeDebugLogs;
		[SerializeField] Toggle includeNetworkLogs;
		[SerializeField] Toggle includeDatabaseLogs;
		[SerializeField] Toggle includeAssertLogs;
		[SerializeField] Toggle includeWarningLogs;
		[SerializeField] Toggle includeErrorLogs;
		[SerializeField] Toggle includeExceptionLogs;


		#region Mono

		protected override void PostInitialize()
		{
			CloseWindow();
			logsText.text = "";
			CreateCommands();
		}

		void Update()
		{
			if (KeyBindingTriggered()) { if (window.activeSelf) { CloseWindow(); } else { OpenWindow(); } }
			NavigateCommandHistory();
		}

		private void OnEnable()
		{
			Application.logMessageReceived += OnLogReceived;
		}

		private void OnDisable()
		{
			Application.logMessageReceived -= OnLogReceived;
		}

		#endregion


		#region Setup & Maintenance

		bool KeyBindingTriggered()
		{
			bool atLeastOneBindingWasDownThisFrame = false;

			foreach (KeyCode keyCode in keyBinding)
			{
				if (Input.GetKey(keyCode))
				{
					if (Input.GetKeyDown(keyCode)) { atLeastOneBindingWasDownThisFrame = true; }
				}
				else return false;
			}

			return atLeastOneBindingWasDownThisFrame;
		}

		void NavigateCommandHistory()
		{
			if (commandField.isFocused)
			{
				if (Input.GetKeyDown(KeyCode.UpArrow) && commandHistoryIndex < commandHistory.Count - 1)
				{
					DisplayCommandHistory(++commandHistoryIndex);
				}

				if (Input.GetKeyDown(KeyCode.DownArrow) && commandHistoryIndex > -1)
				{
					DisplayCommandHistory(--commandHistoryIndex);
				}
			}
		}

		void DisplayCommandHistory(int index)
		{
			if (index <= -1)
			{
				commandField.text = "";
			}
			else
			{
				commandField.text = commandHistory[index];
				commandField.caretPosition = commandField.text.Length;
			}
		}

		void OpenWindow()
		{
			window.SetActive(true);
			ResetCommandField();
		}

		void CloseWindow()
		{
			window.SetActive(false);
		}

		void ResetCommandField()
		{
			commandField.text = "";
			commandField.ActivateInputField();
			commandField.Select();
			commandHistoryIndex = -1;
		}

		IEnumerator ForceScrollDown()
		{
			// Wait for end of frame AND force update all canvases before setting to bottom.
			yield return new WaitForEndOfFrame();
			Canvas.ForceUpdateCanvases();
			scrollRect.verticalNormalizedPosition = 0f;
			Canvas.ForceUpdateCanvases();
		}

		#endregion


		#region Commands

		void CreateCommands()
		{
			commands = new List<Command>()
			{
				new Command(
					"help",
					new string[] { "[command-name]" },
					(string[] args) => { Command_Help(); }
				),

				new Command(
					"clear",
					null,
					(string[] args) => { ClearLogs(); }
				),

				new Command(
					"log",
					new string[] { "message" },
					(string[] args) => {
						if (args.Length > 0) { Debug.Log(args[0]); }
						else { Debug.LogError("Can't log nothing"); }
					}
				)
			};
		}

		// Make sure this is called on the End Edit of the command field
		public void RunCommand()
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				string fullLine = commandField.text;
				ResetCommandField();
				if (System.String.IsNullOrEmpty(fullLine)) { return; }

				List<string> userArgs = fullLine.ToLower().Split(' ').ToList();
				string userCommand = userArgs[0];
				userArgs.RemoveAt(0);

				bool commandFound = false;
				foreach (Command command in commands)
				{
					if (String.Equals(userCommand, command.name))
					{
						commandFound = true;
						command.Action(userArgs.ToArray());
					}
				}

				if (!commandFound) { Debug.LogWarning($"Command \"{userCommand}\" not found"); }

				if (commandHistory.Contains(fullLine)) { commandHistory.RemoveAll(s => String.Equals(s, fullLine)); }
				commandHistory.Insert(0, fullLine);
				StartCoroutine(ForceScrollDown());
			}
		}

		void Command_Help()
		{
			string output = "Listing available commands:";
			foreach (Command command in commands) { output += "\n\t- " + command.HelpText; }
			Debug.Log(output);
		}

		#endregion


		#region Logging

		public static void Log(string message, ConsoleLogType type = ConsoleLogType.Info)
		{
			logs.Add(new Log(message, type));
			_.StartCoroutine(_.ForceScrollDown());
			_.RefreshLogDisplay();
		}

		void OnLogReceived(string message, string stackTrace, LogType unityLogType)
		{
			ConsoleLogType logTypeToPass;
			switch (unityLogType)
			{
				case LogType.Log: logTypeToPass = ConsoleLogType.Info; break;
				case LogType.Assert: logTypeToPass = ConsoleLogType.Assert; break;
				case LogType.Warning: logTypeToPass = ConsoleLogType.Warning; break;
				case LogType.Error: logTypeToPass = ConsoleLogType.Error; break;
				case LogType.Exception: logTypeToPass = ConsoleLogType.Exception; break;
				default: logTypeToPass = ConsoleLogType.Info; break;
			}

			logs.Add(new Log(message, logTypeToPass));
			StartCoroutine(ForceScrollDown());
			RefreshLogDisplay();
		}

		public void RefreshLogDisplay()
		{
			string totalOutput = "";
			foreach (Log log in logs)
			{
				switch (log.type)
				{
					case ConsoleLogType.Info: if (!includeInfoLogs.isOn) continue; else break;
					case ConsoleLogType.Debug: if (!includeDebugLogs.isOn) continue; else break;
					case ConsoleLogType.Network: if (!includeNetworkLogs.isOn) continue; else break;
					case ConsoleLogType.Database: if (!includeDatabaseLogs.isOn) continue; else break;
					case ConsoleLogType.Assert: if (!includeAssertLogs.isOn) continue; else break;
					case ConsoleLogType.Warning: if (!includeWarningLogs.isOn) continue; else break;
					case ConsoleLogType.Error: if (!includeErrorLogs.isOn) continue; else break;
					case ConsoleLogType.Exception: if (!includeExceptionLogs.isOn) continue; else break;
					default: continue;
				}

				totalOutput += "\n" + log.GetText(CurrentTimestampMode);
			}

			logsText.text = totalOutput;
			Vector2 size = logsText.rectTransform.sizeDelta;
			size.y = logsText.preferredHeight;
			logsText.rectTransform.sizeDelta = size;
		}

		public void ClearLogs()
		{
			logs.Clear();
			RefreshLogDisplay();
		}

		#endregion
	}

	[Serializable]
	public struct Command
	{
		public delegate void CommandAction(string[] args);

		public string name;
		public string[] args;
		public CommandAction Action;

		public Command(string name, string[] args, CommandAction Action)
		{
			this.name = name;
			this.args = args;
			this.Action = Action;
		}

		public void ValidateCommand()
		{
			if (name.Contains(' ')) { Debug.LogError($"Command name can't contain a space: {name}"); }
			foreach (string arg in args) { if (arg.Contains(' ')) { Debug.LogError($"Command argument can't contain a space: {name}.{arg}"); } }
		}

		public string HelpText
		{
			get
			{
				if (args != null) { return name + " " + String.Join(" ", args); }
				else { return name; }
			}
		}
	}

	public struct Log
	{
		public float runTimestamp;
		public DateTimeOffset realTimestamp;
		public string message;
		public ConsoleLogType type;

		public Log(string message, ConsoleLogType type)
		{
			runTimestamp = Time.time;
			realTimestamp = DateTimeOffset.Now;
			this.message = message;
			this.type = type;
		}

		string ColorCode
		{
			get
			{
				switch (type)
				{
					case ConsoleLogType.Info: return "#777";
					case ConsoleLogType.Debug: return "#FFF";
					case ConsoleLogType.Network: return "#0AF";
					case ConsoleLogType.Database: return "#93B";
					case ConsoleLogType.Assert: return "#F70";
					case ConsoleLogType.Warning: return "#EB0";
					case ConsoleLogType.Error: return "#F00";
					case ConsoleLogType.Exception: return "#A00";
					default: return "#F0D";
				}
			}
		}

		public string GetText(ConsoleTimestampMode timestampMode)
		{
			List<string> timestampOutput = new List<string>();

			switch (timestampMode)
			{
				case ConsoleTimestampMode.None: return $"<color={ColorCode}>{message}</color>";
				case ConsoleTimestampMode.RunTime: return $"<color={ColorCode}>[{runTimestamp.ToString("F3")}] {message}</color>";
				default: return $"<color={ColorCode}>[{GetRealTimestampOutput(timestampMode)}] {message}</color>";
			}
		}

		public string GetRealTimestampOutput(ConsoleTimestampMode timestampMode)
		{
			List<string> timestampOutput = new List<string>();

			if (timestampMode == ConsoleTimestampMode.DateTime || timestampMode == ConsoleTimestampMode.DateTimeZone)
			{
				timestampOutput.Add(realTimestamp.DateTime.ToString("yyyy/MM/dd"));
			}

			timestampOutput.Add(realTimestamp.DateTime.ToString("HH:mm:ss") + "." + realTimestamp.DateTime.Millisecond.ToString("D3"));

			if (timestampMode == ConsoleTimestampMode.DateTimeZone)
			{
				timestampOutput.Add("TZ" + realTimestamp.Offset.TotalHours.ToString("F1"));
			}

			return String.Join(" ", timestampOutput);
		}
	}
}
