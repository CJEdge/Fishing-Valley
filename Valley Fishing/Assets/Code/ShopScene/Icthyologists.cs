using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class Icthyologists : Shop
{

	#region Serialized Fields

	[SerializeField] private IcthyologistData icthyologistData;
	[SerializeField] private Button[] fishButtons;
	[SerializeField] private Button leaveShopButton;

	#endregion


	#region Properties

	private bool JustSoldFish { get; set; }
	private int LastSoldFish { get; set; }


	#endregion


	#region Mono Behaviours

	public void OnEnable() {
		bool[] buttonsToEnable = new bool[GameManager.Instance.Fish.Count];
		for (int i = 0; i < GameManager.Instance.CaughtFish.Count; i++) {
			if (GameManager.Instance.CaughtFish[i] > 0) {
				buttonsToEnable[i] = true;
			}
		}
		for (int i = 0; i < IcthyologistManager.Instance.SoldFish.Length; i++) {
			if (IcthyologistManager.Instance.SoldFish[i]) {
				buttonsToEnable[i] = true;
			}
		}
		Utilities.DisableUnusedButtons(buttonsToEnable, fishButtons);
		Utilities.LinkHorizontalButtons(fishButtons, leaveShopButton);
		for (int i = 0; i < fishButtons.Length; i++) {
			if (fishButtons[i].gameObject.activeSelf) {
				GameManager.Instance.EventSystem.SetSelectedGameObject(fishButtons[i].gameObject);
				return;
			}
		}
		
	}

	#endregion


	#region Public Methods

	public void HoverFish(int fishIndex) {
		if(GameManager.Instance.CaughtFish[fishIndex] == 0 || IcthyologistManager.Instance.SoldFish[fishIndex]) { 
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.FishBoardFish[fishIndex]);
		} else {
			AudioManager.Instance.PlayVoiceOver(icthyologistData.IcthyologistFishDatas[fishIndex].ButtonHoverEvent);
		}
	}

	public void SellFish(int fishIndex) {
		if (GameManager.Instance.Money > GameManager.Instance.Fish[fishIndex].SellPrice) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ItemBuy);
			AudioManager.Instance.PlayVoiceOver(icthyologistData.IcthyologistFishDatas[fishIndex].ButtonSoldEvent);
			this.LastSoldFish = fishIndex;
			this.JustSoldFish = true;
			this.OnSaleMade?.Invoke();
		} else {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ClickError);
		}
	}

	public void PlayFishInfo(int fishIndex) {
		AudioManager.Instance.PlayVoiceOver(icthyologistData.IcthyologistFishDatas[fishIndex].FishInfoEvent);
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
