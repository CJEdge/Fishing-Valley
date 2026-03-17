using UnityEngine;

[System.Serializable]
public class OwnedItemTypeData
{
    public OwnedItemTypeData(int quant, BaseItemData itemData) { 
        this.quantity = quant;
        this.OwnedItemData = itemData;
    }

    public OwnedItemTypeData() { }
    public int quantity;
    public BaseItemData OwnedItemData;
}
