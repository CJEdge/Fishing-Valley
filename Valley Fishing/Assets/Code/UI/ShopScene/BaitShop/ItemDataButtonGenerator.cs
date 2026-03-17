using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemDataButtonGenerator : MonoBehaviour
{
    #region Enums
    enum ListUsed
    {
        Fish,
        Bait,
        Junk
    }
    #endregion

    #region Serialized Fields
    [SerializeField] private ItemDataButton itemButton;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private Button leaveShopButton;
    [SerializeField] private ListUsed listUsed = ListUsed.Fish;
    #endregion

    #region Properties
    private List<Button> Buttons = new List<Button>();
    private bool Initialized { get; set; }
    #endregion

    #region Mono Behaviours

    public void OnEnable()
    {
        List<OwnedItemTypeData> chosenList;
        switch(listUsed)
        {
            case ListUsed.Fish:
                chosenList = InventoryManager.Instance.OwnedFishTypeDatas;
                break;
            case ListUsed.Bait:
                chosenList = GetTempBaitListForSelling();
                break;
            default:
                chosenList = InventoryManager.Instance.OwnedFishTypeDatas;
                break;
        }

        List<bool> buttonsToEnable = new List<bool>();
        for (int i = 0; i < chosenList.Count; i++)
        {
            if (chosenList[i].quantity > 0)
            {
                buttonsToEnable.Add(true);
            }
            else
            {
                buttonsToEnable.Add(false);
            }
        }

        if (!this.Initialized)
        {
            for (int i = 0; i < chosenList.Count; i++)
            {
                ItemDataButton buttonInstance = Instantiate(itemButton, buttonParent);
                buttonInstance.AssignData(chosenList[i].OwnedItemData);
                buttonInstance.name = chosenList[i].OwnedItemData.ItemName;
                Buttons.Add(buttonInstance.Button);
            }
        }
        Utilities.DisableUnusedButtons(buttonsToEnable, this.Buttons);
        Utilities.LinkVerticalButtons(this.Buttons, leaveShopButton);
        leaveShopButton.transform.SetAsLastSibling();
        this.Initialized = true;
        Buttons.Add(leaveShopButton);
        for (int i = 0; i < this.Buttons.Count; i++)
        {
            if (this.Buttons[i].gameObject.activeSelf)
            {
                this.Buttons[i].gameObject.AddComponent<SelectButtonUtility>();
                return;
            }
        }
    }

    #endregion

    #region Methods 
    //This list is representative of the shopkeeper's stock, because we havent figured out representing it proper
    //This will be removed
    private List<OwnedItemTypeData> GetTempBaitListForSelling()
    {
        List<OwnedItemTypeData> tempList = new();
        BaitDatas.Datas[] listCopy = InventoryManager.Instance.BaitDatas.datas.ToArray();
        for (var i = 0; i < listCopy.Length; i++)
        {
            tempList.Add(new OwnedItemTypeData(1, listCopy[i]));
        }
        return tempList;
    }
    #endregion
}
