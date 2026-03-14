using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static InventoryManager;

public class InventoryManager : Singleton<InventoryManager> {


	public FishDatas FishDatas;

	[System.Serializable]
	public class OwnedFishTypeData {
		public int quantity;
		public FishDatas.FishData OwnedFishData;
	}
	public List<OwnedFishTypeData> OwnedFishTypeDatas;
    public int TotalOwnedFish
    {
        get
        {
            int count = 0;
            for (int i = 0; this.OwnedFishTypeDatas.Count > 0; i++)
            {
                count += this.OwnedFishTypeDatas[i].quantity;
            }
            return count;
        }
    }

    public BaitDatas BaitDatas;

    [System.Serializable]
    public class OwnedBaitTypeData
    {
        public int quantity;
        public BaitDatas.Datas OwnedBaitData;
    }
	public List<OwnedBaitTypeData> OwnedBaitTypeDatas;

	public int TotalOwnedBaits { get
		{
			int count = 0;
			for (int i = 0; this.OwnedBaitTypeDatas.Count > 0; i++)
			{
				count += this.OwnedBaitTypeDatas[i].quantity;
			}
			return count;
		} 
	}

	public BaitDatas.Datas CurrentBait;

    public override void Awake() {
		base.Awake();
		for (int i = 0; i < this.FishDatas.Datas.Length; i++) {
            OwnedFishTypeData fishTypeCatchData = new OwnedFishTypeData();
			fishTypeCatchData.quantity = 0;
			fishTypeCatchData.OwnedFishData = this.FishDatas.Datas[i];
            OwnedFishTypeDatas.Add(fishTypeCatchData);
		}
		for (int i = 0; i < this.OwnedBaitTypeDatas.Count; i++)
		{
			this.OwnedBaitTypeDatas.Add(new OwnedBaitTypeData());
		}
		for(int i = 0; i < this.OwnedFishTypeDatas.Count; i++)
		{
			this.OwnedFishTypeDatas.Add(new OwnedFishTypeData());
		}
	}

}
