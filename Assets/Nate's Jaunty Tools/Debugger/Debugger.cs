using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace NatesJauntyTools
{
	public class Debugger : MonoBehaviour
	{
		static Debugger _ = null;
		static List<Log> logs = new List<Log>();
		static List<string> commandHistory = new List<string>();
		static int commandHistoryIndex = -1;
		static List<Command> commands = new List<Command>();

		public GameObject window;
		public ScrollRect scrollRect;
		public TMP_Text logsText;
		public TMP_InputField commandField;

		[Header("Settings")]
		public KeyCode toggleKey = KeyCode.F12;
		
		[SerializeField] bool timestampDateBool;
		[SerializeField] bool timestampTimeBool;
		[SerializeField] bool timestampMillisecondsBool;
		[SerializeField] bool timestampTimezoneBool;
		[SerializeField] bool stackTraceBool;

		[SerializeField] Toggle timestampDateToggle;
		[SerializeField] Toggle timestampTimeToggle;
		[SerializeField] Toggle timestampMillisecondsToggle;
		[SerializeField] Toggle timestampTimezoneToggle;
		[SerializeField] Toggle stackTraceToggle;

		public bool ShowTimestampDate
		{
			get { return (timestampDateToggle == null) ? timestampDateBool : timestampDateToggle.isOn; }
		}
		public bool ShowTimestampTime
		{
			get { return (timestampTimeToggle == null) ? timestampTimeBool : timestampTimeToggle.isOn; }
		}
		public bool ShowTimestampMilliseconds
		{
			get { return (timestampMillisecondsToggle == null) ? timestampMillisecondsBool : timestampMillisecondsToggle.isOn; }
		}
		public bool ShowTimestampTimezone
		{
			get { return (timestampTimezoneToggle == null) ? timestampTimezoneBool : timestampTimezoneToggle.isOn; }
		}
		public bool ShowStackTrace
		{
			get { return (stackTraceToggle == null) ? stackTraceBool : stackTraceToggle.isOn; }
		}


		#region Mono
		
		void Awake() 
		{
			Initialize();
			CreateCommands();
		}

		void Update()
		{
			ToggleShowWindow();
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

		void Initialize()
		{
			_ = this;

			CloseWindow();
			logsText.text = "";
		}

		void ToggleShowWindow()
		{
			if (Input.GetKeyDown(toggleKey))
			{
				if (window.activeSelf) { CloseWindow(); }
				else { OpenWindow(); }
			}
		}

		void NavigateCommandHistory()
		{
			if (commandField.isFocused)
			{
				if (Input.GetKeyDown(KeyCode.UpArrow) && commandHistoryIndex < commandHistory.Count - 1)
				{
					commandHistoryIndex++;
					DisplayCommandHistory(commandHistoryIndex);
				}
				
				if (Input.GetKeyDown(KeyCode.DownArrow) && commandHistoryIndex > -1)
				{
					commandHistoryIndex--;
					DisplayCommandHistory(commandHistoryIndex);
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
			yield return new WaitForEndOfFrame ();
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
					(string[] args) => { logs.Clear(); }
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

		bool RepresentsTrue(string arg) { return (arg == "t" || arg == "true" || arg == "y" || arg == "yes"); }

		bool RepresentsFalse(string arg) { return (arg == "f" || arg == "false" || arg == "n" || arg == "no"); }

		#endregion


		#region Logging
		
		void OnLogReceived(string message, string stackTrace, LogType logType)
		{
			logs.Add(new Log(message, stackTrace, logType));
			StartCoroutine(ForceScrollDown());
			RefreshLogDisplay();
		}

		public void RefreshLogDisplay()
		{
			string totalOutput = "";
			foreach (Log log in logs) { totalOutput += "\n" + GetLogText(log); }
			logsText.text = totalOutput;
			Vector2 size = logsText.rectTransform.sizeDelta;
			size.y = logsText.preferredHeight;
			logsText.rectTransform.sizeDelta = size;
		}

		string GetLogText(Log log)
		{
			const string LOG_COLOR = "#FFFFFF";
			const string WARNING_COLOR = "#EEBB00";
			const string ERROR_COLOR = "#FF0000";
			const string EXCEPTION_COLOR = "#AA0000";
			const string ASSERT_COLOR = "#9933BB";
			const string UNHANDLED_COLOR = "#1AFF00";

			string colorCode;
			switch (log.type)
			{
				case LogType.Log: colorCode = LOG_COLOR; break;
				case LogType.Warning: colorCode = WARNING_COLOR; break;
				case LogType.Error: colorCode = ERROR_COLOR; break;
				case LogType.Exception: colorCode = EXCEPTION_COLOR; break;
				case LogType.Assert: colorCode = ASSERT_COLOR; break;
				default: colorCode = UNHANDLED_COLOR; break;
			}
			
			string output = $"<color={colorCode}>";

			List<string> timestampOutput = new List<string>();
			if (ShowTimestampDate) { timestampOutput.Add(log.timestamp.DateTime.ToString("yyyy/MM/dd")); }
			if (ShowTimestampTime)
			{
				string timeString = "";
				timeString += log.timestamp.DateTime.ToString("HH:mm:ss");
				if (ShowTimestampMilliseconds) { timeString += "." + log.timestamp.DateTime.Millisecond.ToString("D3"); }
				if (!String.IsNullOrEmpty(timeString)) { timestampOutput.Add(timeString); }
			}
			if (ShowTimestampTimezone) { timestampOutput.Add("TZ" + log.timestamp.Offset.TotalHours.ToString("F1")); }

			if (timestampOutput.Count > 0)
			{
				output += $"[{String.Join(" ", timestampOutput)}] ";
			}

			output += log.message;
			if (ShowStackTrace) { output += "\n" + log.stackTrace; }

			output += "</color>";
			return output;
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
		public DateTimeOffset timestamp;
		public string message;
		public string stackTrace;
		public LogType type;

		public Log(string message, string stackTrace, LogType type)
		{
			timestamp = DateTimeOffset.Now;
			this.message = message;
			this.stackTrace = stackTrace;
			this.type = type;
		}
	}
}
