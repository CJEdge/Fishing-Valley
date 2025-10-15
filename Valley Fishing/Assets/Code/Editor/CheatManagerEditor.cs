using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

//[CustomEditor(typeof(CheatManager))]
public class CheatManagerEditor : Editor {

	//private void OnEnable() {
	//	SceneManager.activeSceneChanged += OnSceneChanged;
	//}

	//private void OnDisable() {
	//	SceneManager.activeSceneChanged -= OnSceneChanged;
	//}

	//private void OnSceneChanged(Scene oldScene, Scene newScene) {
	//	Debug.Log("here");
	//	Repaint();
	//}

	//public override void OnInspectorGUI() {
	//	serializedObject.Update();

	//	GUI.enabled = false;
	//	EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
	//	GUI.enabled = true;
	//	EditorGUILayout.PropertyField(serializedObject.FindProperty("useCheats"));
	//	EditorGUILayout.PropertyField(serializedObject.FindProperty("superFastCatch"));
	//	string currentScene = SceneManager.GetActiveScene().name;

	//	if (currentScene == LevelManager.CatchTutorial_01) {
	//		EditorGUILayout.PropertyField(serializedObject.FindProperty("secondCatchTutorialCheats"));
	//	}
	//	if (currentScene == LevelManager.CatchTutorial_02) {
	//		Debug.Log("change");
	//		EditorGUILayout.PropertyField(serializedObject.FindProperty("thirdCatchTutorialCheats"));
	//	}
	//	if (currentScene == LevelManager.CatchTutorial_03) {
	//		EditorGUILayout.PropertyField(serializedObject.FindProperty("fourthCatchTutorialCheats"));
	//	}
	//	if (currentScene == LevelManager.ShopTutorial_00) {
	//		EditorGUILayout.PropertyField(serializedObject.FindProperty("firstShopTutorialCheats"));
	//	}
	//	if (currentScene == LevelManager.ShopTutorial_01) {
	//		EditorGUILayout.PropertyField(serializedObject.FindProperty("secondShopTutorialCheats"));
	//	}
	//	if (currentScene == LevelManager.ShopTutorial_02) {
	//		EditorGUILayout.PropertyField(serializedObject.FindProperty("thirdShopTutorialCheats"));
	//	}
	//	if (currentScene == LevelManager.BossTutorial_00) {
	//		EditorGUILayout.PropertyField(serializedObject.FindProperty("firstBossTutorialCheats"));
	//	}
	//	serializedObject.ApplyModifiedProperties();
	//}
}