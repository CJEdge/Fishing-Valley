using FMODUnity;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDataButton : ButtonVoiceOverComponent
{
    #region Serialized Fields

    [SerializeField] private TMP_Text itemName;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemSellPrice;

    #endregion


    #region Properties

    public BaseItemData ItemData { get; set; }

    #endregion


    #region Public Methods

    public void AssignData(BaseItemData data)
    {
        this.ItemData = data;
        if (itemName != null)
        {
            itemName.text = data.ItemName;
        }
        if (itemImage != null)
        {
            itemImage.sprite = data.ItemImage;
        }
        if (itemSellPrice != null)
        {
            itemSellPrice.text = data.ItemSellPrice.ToString();
        }
    }

    public override void DoHoverEffect()
    {
        base.DoHoverEffect();
        List<EventReference> voiceoverChain = new List<EventReference>();
        
        if (this.ItemData is FishDatas.FishData)
        {
            int itemIndex = Array.IndexOf(InventoryManager.Instance.FishDatas.Datas, this.ItemData);
            int intemQuantity = InventoryManager.Instance.OwnedFishTypeDatas[itemIndex].quantity;
            voiceoverChain.Add(this.ItemData.ItemNameEvent);
            voiceoverChain.AddRange(FMODManager.Instance.GetNumber(this.ItemData.ItemSellPrice));
            voiceoverChain.Add(FMODManager.Instance.Gold);
            voiceoverChain.Add(FMODManager.Instance.YouHave);
            for (int i = 0; i < FMODManager.Instance.GetNumber(intemQuantity).Count; i++)
            {
                voiceoverChain.Add(FMODManager.Instance.GetNumber(intemQuantity)[i]);
            }            
        }

        if(this.ItemData is BaitDatas.BaitData)
        {
            int itemIndex = Array.IndexOf(InventoryManager.Instance.FishDatas.Datas, this.ItemData);
            int itemValue = InventoryManager.Instance.OwnedFishTypeDatas[itemIndex].quantity * 5;
            voiceoverChain.AddRange(FMODManager.Instance.GetNumber(itemValue));
            voiceoverChain.Add(FMODManager.Instance.Gold);
            if (GameManager.Instance.ShopController.BaitShop.BaitQuantities[itemIndex] == 0)
            {
                voiceoverChain.Add(FMODManager.Instance.SoldOut);
                AudioManager.Instance.PlayVoiceOverChain(voiceoverChain);
                return;
            }
            voiceoverChain.AddRange(FMODManager.Instance.GetNumber(GameManager.Instance.ShopController.BaitShop.BaitBoard.BaitQuantities[itemIndex]));
            voiceoverChain.Add(FMODManager.Instance.Left);
        }
        AudioManager.Instance.PlayVoiceOverChain(voiceoverChain);
    }

    public void OnClick()
    {
        if (this.ItemData is FishDatas.FishData)
        {
            GameManager.Instance.ShopController.BaitShop.SellFish(transform.GetSiblingIndex());
        }
    }

    #endregion

}
