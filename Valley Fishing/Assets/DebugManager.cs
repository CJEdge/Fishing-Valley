using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
	public TMP_Text consoleText; // Replace with TMP_Text if using TextMeshPro
	private List<string> logEntries = new List<string>();
	private int maxLogs = 5;

	void OnEnable() {
		Application.logMessageReceived += HandleLog;
	}

	void OnDisable() {
		Application.logMessageReceived -= HandleLog;
	}

	void HandleLog(string logString, string stackTrace, LogType type) {
		logEntries.Add($"[{type}] {logString}");

		if (logEntries.Count > maxLogs) {
			logEntries.RemoveAt(0); // Remove oldest
		}

		consoleText.text = string.Join("\n", logEntries);
	}
}
