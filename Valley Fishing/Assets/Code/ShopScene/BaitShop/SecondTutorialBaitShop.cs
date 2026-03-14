using FMODUnity;
using System.Collections;
using UnityEngine;

public class SecondTutorialBaitShop : BaitShop {

	#region Properties

	public bool AllFishSold { get => InventoryManager.Instance.TotalOwnedFish > 0; }
	private bool FishBoardNotClosedForFirstTime { get; set; }


    #endregion

    //public void Awake()
    //{

    //}

	public override void Awake()
	{
		base.Awake();
        leaveShopButton.gameObject.SetActive(false);
        GameManager.Instance.InputController.SelectButton(fishBasketButton.gameObject);
    }

	public override void VoiceLineOver(EventReference eventReference, bool skipped) {
		base.VoiceLineOver(eventReference, skipped);
		if (InventoryManager.Instance.OwnedBaitTypeDatas[5].quantity == 5 && InventoryManager.Instance.OwnedBaitTypeDatas[6].quantity == 5 && baitBoard.Initialized && !this.BaitboardTutorialsCompleted[2]) {
			PlayNextTutotialVoiceOver(this.BaitboardTutorialsCompleted, baitboardTutorials);
			IncrementTutorial(this.BaitboardTutorialsCompleted);
		}
		if (InventoryManager.Instance.OwnedBaitTypeDatas[5].quantity == 5 && InventoryManager.Instance.OwnedBaitTypeDatas[6].quantity == 5 && !baitBoard.Initialized && !this.BaitboardTutorialsCompleted[3]) {
			PlayNextTutotialVoiceOver(this.BaitboardTutorialsCompleted, baitboardTutorials);
			IncrementTutorial(this.BaitboardTutorialsCompleted);
		}
		if (baitBoard.Initialized && !this.BaitboardTutorialsCompleted[1]) {
			PlayNextTutotialVoiceOver(this.BaitboardTutorialsCompleted, baitboardTutorials);
			IncrementTutorial(this.BaitboardTutorialsCompleted);
		}
		if (InventoryManager.Instance.TotalOwnedFish == 0 && !this.BaitboardTutorialsCompleted[0]) {
				PlayNextTutotialVoiceOver(this.BaitboardTutorialsCompleted, baitboardTutorials);
				IncrementTutorial(this.BaitboardTutorialsCompleted);
				FishBoardNotClosedForFirstTime = true;
		}
	}

	public override void SellFish(int fishIndex) {
		base.SellFish(fishIndex);
	}

	public override void BuyBait(int baitIndex, int baitQuantity) {
		base.BuyBait(baitIndex, baitQuantity);
	}

	public override void OpenFishBoard() {
		if (fishBoard.Initialized && InventoryManager.Instance.TotalOwnedFish != 0) {
			return;
		}
		if (InventoryManager.Instance.TotalOwnedFish == 0) {
			GameManager.Instance.InputController.SelectButton(baitBoardButton.gameObject);
			GameManager.Instance.InputController.SelectionManuallySet = false;
		}
		base.OpenFishBoard();
		if (InventoryManager.Instance.OwnedBaitTypeDatas[4].quantity != 5) {
			leaveShopButton.gameObject.SetActive(false);
		}
		PlayNextTutotialVoiceOver(this.FishboardTutorialsCompleted, fishboardTutorials);
		IncrementTutorial(this.FishboardTutorialsCompleted);
	}

	public override void OpenBaitBoard() {
		base.OpenBaitBoard();
		PlayNextTutotialVoiceOver(this.BaitboardTutorialsCompleted, baitboardTutorials);
		IncrementTutorial(this.BaitboardTutorialsCompleted);
	}

	public override IEnumerator EnterShop(bool enter) {
		yield return StartCoroutine(base.EnterShop(enter));
		if (enter) {
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitShopIntros[1]);
		}
	}

	public override void FishBoardSelected() {
		AudioManager.Instance.PlayVoiceOver(fishBoardEvent);
	}

	public override void FishBasketSelected() {
		AudioManager.Instance.PlayVoiceOver(fishBasketEvent);
	}

	public override void BaitBoardSelected() {
		AudioManager.Instance.PlayVoiceOver(baitBoardEvent);
	}

	public override void LeaveShopSelected() {
		AudioManager.Instance.PlayVoiceOver(leaveShopEvent);
	}

	public override void Skip() {
		base.Skip();
		if (InventoryManager.Instance.TotalOwnedFish == 0 && fishBoard.Initialized) {
			OpenFishBoard();
		}
		if (InventoryManager.Instance.OwnedBaitTypeDatas[5].quantity == 5 && InventoryManager.Instance.OwnedBaitTypeDatas[6].quantity == 5 && baitBoard.Initialized) {
			OpenBaitBoard();
		}
	}
}
