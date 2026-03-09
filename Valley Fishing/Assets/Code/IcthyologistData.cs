using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "IcthyologistData", menuName = "Scriptable Objects/IcthyologistData")]
public class IcthyologistData : ScriptableObject
{
    [System.Serializable]
	public class IcthyologistFishData {
		public EventReference ButtonHoverEvent;
		public EventReference ButtonSoldEvent;
		public EventReference FishInfoEvent;
	}
	public IcthyologistFishData[] IcthyologistFishDatas;
}
