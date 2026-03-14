using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "BaitDatas", menuName = "Scriptable Objects/BaitDatas")]
public class BaitDatas : ScriptableObject
{
    [System.Serializable]
    public class FishSpawnChance
    {
        public FishName FishName;
        public float SpawnChance;        
    }

    [System.Serializable]
    public class Datas
    {
        public string BaitName;
        public Sprite BaitImage;
        public int BaitPrice;
        public bool IsTutorial;
        public List<FishSpawnChance> FishSpawnChances = new List<FishSpawnChance>();
    }
    public Datas[] datas;



    [CustomPropertyDrawer(typeof(FishSpawnChance))]
    public class FishSpawnChanceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty fishName = property.FindPropertyRelative("FishName");
            SerializedProperty spawnChance = property.FindPropertyRelative("SpawnChance");

            float halfWidth = position.width * 0.6f;

            Rect enumRect = new Rect(position.x, position.y, halfWidth, position.height);
            Rect chanceRect = new Rect(position.x + halfWidth + 5, position.y, position.width - halfWidth - 5, position.height);

            EditorGUI.PropertyField(enumRect, fishName, GUIContent.none);
            EditorGUI.PropertyField(chanceRect, spawnChance, GUIContent.none);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
