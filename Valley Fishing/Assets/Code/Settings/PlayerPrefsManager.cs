using UnityEngine;

public static class PlayerPrefsManager
{
	public const string Language = "Language";

	public static void Save(string name, int value) {
		PlayerPrefs.SetInt(name, value);
	}

	public static int Load(string name) {
		return PlayerPrefs.GetInt(name);
	}
}
