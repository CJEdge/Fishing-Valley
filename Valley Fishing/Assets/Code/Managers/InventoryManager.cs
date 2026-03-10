using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager> {


	[SerializeField] FishDatas fishDatas;

	[System.Serializable]
	public class FishTypeCatchData {
		public int quantity;
		public FishDatas.FishData CaughtFishData;
	}
	public List<FishTypeCatchData> FishTypeCatchDatas;


	public override void Awake() {
		base.Awake();
		for (int i = 0; i < fishDatas.Datas.Length; i++) {
			FishTypeCatchData fishTypeCatchData = new FishTypeCatchData();
			fishTypeCatchData.quantity = 0;
			fishTypeCatchData.CaughtFishData = fishDatas.Datas[i];
			FishTypeCatchDatas.Add(fishTypeCatchData);
		}
	}

}
