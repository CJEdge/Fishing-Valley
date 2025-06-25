using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(CheatManager))]
public class CheatManagerEditor : Editor {
	public override void OnInspectorGUI() {
		serializedObject.Update();

		GUI.enabled = false;
		EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
		GUI.enabled = true;

		string currentScene = SceneManager.GetActiveScene().name;

		if (currentScene == LevelManager.CatchTutorial_01) {
			EditorGUILayout.PropertyField(serializedObject.FindProperty("secondCatchTutorialCheats"));
		}
		if (currentScene == LevelManager.CatchTutorial_02) {
			EditorGUILayout.PropertyField(serializedObject.FindProperty("thirdCatchTutorialCheats"));
		}
		if (currentScene == LevelManager.CatchTutorial_03) {
			EditorGUILayout.PropertyField(serializedObject.FindProperty("fourthCatchTutorialCheats"));
		}
		if (currentScene == LevelManager.ShopTutorial_00) {
			EditorGUILayout.PropertyField(serializedObject.FindProperty("firstShopTutorialCheats"));
		}
		serializedObject.ApplyModifiedProperties();
	}
}