using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "RodShopData", menuName = "Scriptable Objects/RodShopData")]
public class RodShopData : ScriptableObject
{
    [System.Serializable]
	public class ReelSpeedData {
		public int ReelSpeedPrices;
		public EventReference ReelSpeedHoverEvents;
		public EventReference ReelSpeedBoughtEvents;
	}
	public ReelSpeedData[] ReelSpeedDatas;

	[System.Serializable]
	public class StrafeSpeedData {
		public int StrafeSpeedPrices;
		public EventReference StrafeSpeedHoverEvents;
		public EventReference StrafeSpeedBoughtEvents;
	}
	public StrafeSpeedData[] StrafeSpeedDatas;

	[System.Serializable]
	public class FailSpeedData {
		public int FailSpeedPrices;
		public EventReference FailSpeedHoverEvents;
		public EventReference FailSpeedBoughtEvents;
	}
	public FailSpeedData[] FailSpeedDatas;
}
