using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static InventoryManager;

public class InventoryManager : Singleton<InventoryManager> {


	public FishDatas FishDatas;

	//Can be used for both fish and bait because they are both BaseItemData
	[System.Serializable]
	public class OwnedItemTypeData {
		public int quantity;
		public BaseItemData OwnedItemData;
	}

	public List<OwnedItemTypeData> OwnedFishTypeDatas;
    public int TotalOwnedFish
    {
        get
        {
            int count = 0;
            for (int i = 0; i < this.OwnedFishTypeDatas.Count; i++)
            {
                count += this.OwnedFishTypeDatas[i].quantity;
            }
            return count;
        }
    }

    public BaitDatas BaitDatas;
	public List<OwnedItemTypeData> OwnedBaitTypeDatas;

	public int TotalOwnedBaits { get
		{
			int count = 0;
			for (int i = 0; i < this.OwnedBaitTypeDatas.Count; i++)
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
            OwnedItemTypeData fishTypeCatchData = new OwnedItemTypeData();
			fishTypeCatchData.quantity = 0;
			fishTypeCatchData.OwnedItemData = this.FishDatas.Datas[i];
            OwnedFishTypeDatas.Add(fishTypeCatchData);
		}
		for (int i = 0; i < this.BaitDatas.datas.Length; i++)
		{
			this.OwnedBaitTypeDatas.Add(new OwnedItemTypeData());
		}
	}

}
