using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(CheatManager))]
public class CheatManagerEditor : Editor {
	public override void OnInspectorGUI() {
		serializedObject.Update();

		string currentScene = SceneManager.GetActiveScene().name;

		if (currentScene == LevelManager.CatchTutorial_01) {
			EditorGUILayout.PropertyField(serializedObject.FindProperty("secondCatchTutorialCheats"));
		}
		if (currentScene == LevelManager.ShopTutorial_01) {
			EditorGUILayout.PropertyField(serializedObject.FindProperty("firstShopTutorialCheats"));
		}
		serializedObject.ApplyModifiedProperties();
	}
}