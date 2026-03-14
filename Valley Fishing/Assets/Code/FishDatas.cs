using FMODUnity;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "FishDatas", menuName = "Scriptable Objects/FishDatas")]
public class FishDatas : ScriptableObject { 

	[System.Serializable]
	public class FishData {
		public string FishName;
		public Sprite Sprite;
		public int SellPrice;
		public bool IsFailable;
		public float ReelInSpeed;
		public float SwimAwaySpeed;
		public float StrafedSwimAwaySpeed;
		public enum ActivityLevel {
			none,
			calm,
			medium,
			active
		}
		public List<ActivityLevel> ActivityLevels;

		public enum MovementDirection {
			none,
			left,
			right
		}
		public List<MovementDirection> MovementDirections;
		public EventReference fishNameAudio;
		public EventReference fishCatchAudio;
		public EventReference icthyologistInfoAudio;
	}

	public FishData[] Datas;

    public void OnValidate()
    {
        List<string> fishNames = new List<string>();
        for (int i = 0; i < Datas.Length; i++)
        {
            fishNames.Add(Datas[i].FishName.Replace(" ", ""));
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("public enum FishName");
        sb.AppendLine("{");

        foreach (var name in fishNames)
        {
            sb.AppendLine($"    {name},");
        }

        sb.AppendLine("}");
        File.WriteAllText("Assets/Code/FishNames.cs", sb.ToString());

        AssetDatabase.Refresh();
    }
}
