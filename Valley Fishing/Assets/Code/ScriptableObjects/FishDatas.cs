using FMODUnity;
using System.Collections.Generic;
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
		public List<MovementDirection> movementDirections;
		public EventReference fishNameAudio;
		public EventReference fishCatchAudio;
		public EventReference icthyologistInfoAudio;
	}

	public FishData[] Datas;

}
