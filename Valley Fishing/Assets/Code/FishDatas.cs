using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "FishDatas", menuName = "Scriptable Objects/FishDatas")]
public class FishDatas : ScriptableObject { 

	[System.Serializable]
	public class FishData {
		public Image image;
		public string FishName;
		public int SellPrice;
	}
	public FishData[] Datas;

}
