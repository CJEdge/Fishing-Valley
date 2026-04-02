using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BaitDatas.FishSpawnChance))]
public class FishSpawnChanceDrawer : PropertyDrawer {
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		SerializedProperty fishName = property.FindPropertyRelative("FishName");
		SerializedProperty spawnChance = property.FindPropertyRelative("SpawnChance");

		float halfWidth = position.width * 0.6f;

		Rect enumRect = new Rect(position.x, position.y, halfWidth, position.height);
		Rect chanceRect = new Rect(position.x + halfWidth + 5, position.y, position.width - halfWidth - 5, position.height);

		EditorGUI.PropertyField(enumRect, fishName, GUIContent.none);
		EditorGUI.PropertyField(chanceRect, spawnChance, GUIContent.none);
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		return EditorGUIUtility.singleLineHeight;
	}
}
