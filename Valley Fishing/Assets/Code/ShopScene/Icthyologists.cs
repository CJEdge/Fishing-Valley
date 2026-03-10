using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icthyologists : Shop
{

	#region Serialized Fields

	[SerializeField] private IcthyologistData icthyologistData;
	[SerializeField] private FishDataButton fishButton;
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
		for (int i = 0; i < InventoryManager.Instance.FishTypeCatchDatas.Count; i++) {
			if (InventoryManager.Instance.FishTypeCatchDatas[i].quantity > 0) {
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
			for (int i = 0; i < InventoryManager.Instance.FishTypeCatchDatas.Count; i++) {
				FishDataButton buttonInstance = Instantiate(fishButton, buttonParent);
				buttonInstance.AssignData(InventoryManager.Instance.FishTypeCatchDatas[i].CaughtFishData);
				buttonInstance.name = InventoryManager.Instance.FishTypeCatchDatas[i].CaughtFishData.FishName;
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
		
		if(GameManager.Instance.CaughtFish[fishIndex] == 0 || IcthyologistManager.Instance.SoldFish[fishIndex]) { 
			AudioManager.Instance.PlayVoiceOver(InventoryManager.Instance.FishTypeCatchDatas[fishIndex].CaughtFishData.fishNameAudio);
		} else {
			AudioManager.Instance.PlayVoiceOver(InventoryManager.Instance.FishTypeCatchDatas[fishIndex].CaughtFishData.fishNameAudio);
		}
	}

	public void SellFish(int fishIndex) {
		if (!IcthyologistManager.Instance.SoldFish[fishIndex]) {
			GameManager.Instance.Money += InventoryManager.Instance.FishTypeCatchDatas[fishIndex].CaughtFishData.SellPrice;
			InventoryManager.Instance.FishTypeCatchDatas[fishIndex].quantity--;
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
		AudioManager.Instance.PlayVoiceOver(InventoryManager.Instance.FishTypeCatchDatas[fishIndex].CaughtFishData.icthyologistInfoAudio);
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
