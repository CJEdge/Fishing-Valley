using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaitDatas", menuName = "Scriptable Objects/BaitDatas")]
public class BaitDatas : ScriptableObject {
	[System.Serializable]
	public class FishSpawnChance {
		public FishName FishName;
		public float SpawnChance;
	}

	[System.Serializable]
	public class Datas : BaseItemData {
		public bool IsTutorial;
		public List<FishSpawnChance> FishSpawnChances = new List<FishSpawnChance>();
	}

	public Datas[] datas;
}
