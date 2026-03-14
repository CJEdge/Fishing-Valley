using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager> {


	public FishDatas FishDatas;

	[System.Serializable]
	public class FishTypeCatchData {
		public int quantity;
		public FishDatas.FishData CaughtFishData;
	}
	public List<FishTypeCatchData> FishTypeCatchDatas;


	public override void Awake() {
		base.Awake();
		for (int i = 0; i < this.FishDatas.Datas.Length; i++) {
			FishTypeCatchData fishTypeCatchData = new FishTypeCatchData();
			fishTypeCatchData.quantity = 0;
			fishTypeCatchData.CaughtFishData = this.FishDatas.Datas[i];
			FishTypeCatchDatas.Add(fishTypeCatchData);
		}
	}

}
