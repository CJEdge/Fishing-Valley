using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icthyologists : Shop
{

	#region Serialized Fields

	[SerializeField] private IcthyologistData icthyologistData;
	[SerializeField] private ItemDataButton fishButton;
	[SerializeField] private Transform buttonParent;
	[SerializeField] private Button leaveShopButton;

	#endregion


	#region Properties

	public List<Button> Buttons;
	private bool JustSoldFish { get; set; }
	private int LastSoldFish { get; set; }
	private bool Initialized { get; set; }

	#endregion


	#region Mono Behaviours

	public void OnEnable() {
		List<bool> buttonsToEnable = new List<bool>();
		for (int i = 0; i < InventoryManager.Instance.OwnedFishTypeDatas.Count; i++) {
			if (InventoryManager.Instance.OwnedFishTypeDatas[i].quantity > 0) {
				buttonsToEnable.Add(true);
			} else {
				buttonsToEnable.Add(false);
			}
		}
		for (int i = 0; i < IcthyologistManager.Instance.SoldFish.Length; i++) {
			if (IcthyologistManager.Instance.SoldFish[i]) {
				buttonsToEnable[i] = true;
			}
		}
		if (!this.Initialized) {
			for (int i = 0; i < InventoryManager.Instance.OwnedFishTypeDatas.Count; i++) {
				ItemDataButton buttonInstance = Instantiate(fishButton, buttonParent);
				buttonInstance.AssignData(InventoryManager.Instance.OwnedFishTypeDatas[i].OwnedFishData);
				buttonInstance.name = InventoryManager.Instance.OwnedFishTypeDatas[i].OwnedFishData.ItemName;
				Buttons.Add(buttonInstance.Button);
			}
		}
		Utilities.DisableUnusedButtons(buttonsToEnable, this.Buttons);
		Utilities.LinkHorizontalButtons(this.Buttons, leaveShopButton);
		leaveShopButton.transform.SetAsLastSibling();
		this.Initialized = true;
		for (int i = 0; i < this.Buttons.Count; i++) {
			if (this.Buttons[i].gameObject.activeSelf) {
				GameManager.Instance.EventSystem.SetSelectedGameObject(this.Buttons[i].gameObject);
				return;
			}
		}
	}

	#endregion


	#region Public Methods

	public void HoverFish(int fishIndex) {
		
		if(InventoryManager.Instance.OwnedFishTypeDatas[fishIndex].quantity == 0 || IcthyologistManager.Instance.SoldFish[fishIndex]) { 
			AudioManager.Instance.PlayVoiceOver(InventoryManager.Instance.OwnedFishTypeDatas[fishIndex].OwnedFishData.fishNameAudio);
		} else {
			AudioManager.Instance.PlayVoiceOver(InventoryManager.Instance.OwnedFishTypeDatas[fishIndex].OwnedFishData.fishNameAudio);
		}
	}

	public void SellFish(int fishIndex) {
		if (!IcthyologistManager.Instance.SoldFish[fishIndex]) {
			GameManager.Instance.Money += InventoryManager.Instance.OwnedFishTypeDatas[fishIndex].OwnedFishData.ItemSellPrice;
			InventoryManager.Instance.OwnedFishTypeDatas[fishIndex].quantity--;
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ItemBuy);
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.YouHave);
			this.LastSoldFish = fishIndex;
			this.JustSoldFish = true;
			this.OnSaleMade?.Invoke();
		} else {
			PlayFishInfo(fishIndex);
		}		
	}

	public void PlayFishInfo(int fishIndex) {
		AudioManager.Instance.PlayVoiceOver(InventoryManager.Instance.OwnedFishTypeDatas[fishIndex].OwnedFishData.icthyologistInfoAudio);
		this.JustSoldFish = false;
	}

	#endregion


	#region Shop

	public override void VoiceLineOver(EventReference eventReference, bool skipped) {
		if (this.JustSoldFish) {
			PlayFishInfo(this.LastSoldFish);
		}
	}

	public override void LeaveShop() {
		base.LeaveShop();
		this.JustSoldFish = false;
	}

	#endregion

}
