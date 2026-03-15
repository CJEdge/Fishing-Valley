using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishSellingUI : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private FishDataButton fishButton;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private Button leaveShopButton;
    #endregion

    #region Properties
    public List<Button> Buttons;
    private bool Initialized { get; set; }
    #endregion

    #region Mono Behaviours

    public void OnEnable()
    {
        List<bool> buttonsToEnable = new List<bool>();
        for (int i = 0; i < InventoryManager.Instance.OwnedFishTypeDatas.Count; i++)
        {
            if (InventoryManager.Instance.OwnedFishTypeDatas[i].quantity > 0)
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
            for (int i = 0; i < InventoryManager.Instance.OwnedFishTypeDatas.Count; i++)
            {
                FishDataButton buttonInstance = Instantiate(fishButton, buttonParent);
                buttonInstance.AssignData(InventoryManager.Instance.OwnedFishTypeDatas[i].OwnedFishData);
                buttonInstance.name = InventoryManager.Instance.OwnedFishTypeDatas[i].OwnedFishData.FishName;
                Buttons.Add(buttonInstance.Button);
            }
        }
        Utilities.DisableUnusedButtons(buttonsToEnable, this.Buttons);
        Utilities.LinkVerticalButtons(this.Buttons, leaveShopButton);
        leaveShopButton.transform.SetAsLastSibling();
        this.Initialized = true;
        for (int i = 0; i < this.Buttons.Count; i++)
        {
            if (this.Buttons[i].gameObject.activeSelf)
            {
                GameManager.Instance.EventSystem.SetSelectedGameObject(this.Buttons[i].gameObject);
                return;
            }
        }
    }

    #endregion
}
