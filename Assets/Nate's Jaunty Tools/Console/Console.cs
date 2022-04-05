using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace NatesJauntyTools
{
	public class Console : Singleton<Console>
	{
		public enum TimestampMode { None, RunTime, RealTime, DateTime, DateTimeZone }
		public enum LogType { Info, Debug, Network, Database, Warning, Error, Exception, Assert }


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
		public List<KeyBinding> keyBindings = new List<KeyBinding>();

		[Header("Log Settings")]
		[SerializeField] Toggle logTimestampNone;
		[SerializeField] Toggle logTimestampRunTime;
		[SerializeField] Toggle logTimestampRealTime;
		[SerializeField] Toggle logTimestampDateTime;
		[SerializeField] Toggle logTimestampDateTimeZone;
		public Console.TimestampMode CurrentTimestampMode
		{
			get
			{
				if (logTimestampNone.isOn) return Console.TimestampMode.None;
				else if (logTimestampRunTime.isOn) return Console.TimestampMode.RunTime;
				else if (logTimestampRealTime.isOn) return Console.TimestampMode.RealTime;
				else if (logTimestampDateTime.isOn) return Console.TimestampMode.DateTime;
				else if (logTimestampDateTimeZone.isOn) return Console.TimestampMode.DateTimeZone;
				else return Console.TimestampMode.RunTime;
			}
		}

		[SerializeField] GameObject logFilterPrefab;
		[SerializeField] Transform logFilterParent;
		[SerializeField] List<LogType> includedLogTypes = new List<LogType>();


		#region Mono

		protected override void PostInitialize()
		{
			CloseWindow();
			logsText.text = "";
			CreateCommands();

			PopulateLogFilters();
		}

		void Update()
		{
			if (AnyKeyBindingTriggered()) { ToggleWindow(); }
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

		bool AnyKeyBindingTriggered()
		{
			bool atLeastOneBindingWasTriggered = false;

			foreach (var binding in keyBindings)
			{
				if (binding.TriggeredThisFrame()) atLeastOneBindingWasTriggered = true;
			}

			return atLeastOneBindingWasTriggered;
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

		void ToggleWindow()
		{
			if (window.activeSelf) CloseWindow();
			else OpenWindow();
		}

		void ResetCommandField()
		{
			commandField.text = "";
			commandField.ActivateInputField();
			commandField.Select();
			commandHistoryIndex = -1;
		}

		IEnumerator ForceScrollToBottom()
		{
			// Wait for end of frame AND force update all canvases before setting to bottom.
			yield return new WaitForEndOfFrame();
			Canvas.ForceUpdateCanvases();
			scrollRect.verticalNormalizedPosition = 0f;
			Canvas.ForceUpdateCanvases();
		}

		void PopulateLogFilters()
		{
			includedLogTypes = Tools.GetEnumValues<LogType>();

			foreach (LogType logType in includedLogTypes)
			{
				ConsoleLogFilter newFilter = Instantiate(logFilterPrefab, logFilterParent).GetComponent<ConsoleLogFilter>();
				newFilter.SetData(logType);
				newFilter.OnValueChanged.AddListener((bool value) => AdjustLogFilter(newFilter.logType, value));
			}


			void AdjustLogFilter(LogType logType, bool included)
			{
				if (included) includedLogTypes.Add(logType);
				else includedLogTypes.Remove(logType);

				RefreshLogDisplay();
			}
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
		public void RunCommand(string commandText)
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				ResetCommandField();
				if (System.String.IsNullOrEmpty(commandText)) { return; }

				List<string> userArgs = commandText.ToLower().Split(' ').ToList();
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

				if (commandHistory.Contains(commandText)) { commandHistory.RemoveAll(s => String.Equals(s, commandText)); }
				commandHistory.Insert(0, commandText);
				StartCoroutine(ForceScrollToBottom());
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

		public static void Log(string message, Console.LogType type = Console.LogType.Info)
		{
			logs.Add(new Log(message, type));
			_.StartCoroutine(_.ForceScrollToBottom());
			_.RefreshLogDisplay();
		}

		void OnLogReceived(string message, string stackTrace, UnityEngine.LogType unityLogType)
		{
			Console.LogType consoleLogType = unityLogType switch
			{
				UnityEngine.LogType.Log => Console.LogType.Info,
				UnityEngine.LogType.Assert => Console.LogType.Assert,
				UnityEngine.LogType.Warning => Console.LogType.Warning,
				UnityEngine.LogType.Error => Console.LogType.Error,
				UnityEngine.LogType.Exception => Console.LogType.Exception,

				_ => Console.LogType.Info
			};

			logs.Add(new Log(message, consoleLogType));
			StartCoroutine(ForceScrollToBottom());
			RefreshLogDisplay();
		}

		public void RefreshLogDisplay()
		{
			StringBuilder totalOutput = new StringBuilder();
			foreach (Log log in logs)
			{
				if (!includedLogTypes.Contains(log.type)) continue; // Skip this log

				totalOutput.Append("\n" + log.GetText(CurrentTimestampMode));
			}

			logsText.text = totalOutput.ToString();
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
		public Console.LogType type;

		public Log(string message, Console.LogType type)
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
					case Console.LogType.Info: return "#777";
					case Console.LogType.Debug: return "#FFF";
					case Console.LogType.Network: return "#0AF";
					case Console.LogType.Database: return "#93B";
					case Console.LogType.Assert: return "#F70";
					case Console.LogType.Warning: return "#EB0";
					case Console.LogType.Error: return "#F00";
					case Console.LogType.Exception: return "#A00";
					default: return "#F0D";
				}
			}
		}

		public string GetText(Console.TimestampMode timestampMode)
		{
			List<string> timestampOutput = new List<string>();

			switch (timestampMode)
			{
				case Console.TimestampMode.None: return $"<color={ColorCode}>{message}</color>";
				case Console.TimestampMode.RunTime: return $"<color={ColorCode}>[{runTimestamp.ToString("F3")}] {message}</color>";
				default: return $"<color={ColorCode}>[{GetRealTimestampOutput(timestampMode)}] {message}</color>";
			}
		}

		public string GetRealTimestampOutput(Console.TimestampMode timestampMode)
		{
			List<string> timestampOutput = new List<string>();

			if (timestampMode == Console.TimestampMode.DateTime || timestampMode == Console.TimestampMode.DateTimeZone)
			{
				timestampOutput.Add(realTimestamp.DateTime.ToString("yyyy/MM/dd"));
			}

			timestampOutput.Add(realTimestamp.DateTime.ToString("HH:mm:ss") + "." + realTimestamp.DateTime.Millisecond.ToString("D3"));

			if (timestampMode == Console.TimestampMode.DateTimeZone)
			{
				timestampOutput.Add("TZ" + realTimestamp.Offset.TotalHours.ToString("F1"));
			}

			return String.Join(" ", timestampOutput);
		}
	}

	[Serializable]
	public struct KeyBinding
	{
		public List<KeyCode> keys;

		public KeyBinding(List<KeyCode> keys)
		{
			this.keys = keys;
		}

		public bool TriggeredThisFrame()
		{
			bool atLeastOneBindingWasDownThisFrame = false;

			foreach (KeyCode keyCode in keys)
			{
				if (Input.GetKey(keyCode))
				{
					if (Input.GetKeyDown(keyCode)) { atLeastOneBindingWasDownThisFrame = true; }
				}
				else return false;
			}

			return atLeastOneBindingWasDownThisFrame;
		}
	}
}
