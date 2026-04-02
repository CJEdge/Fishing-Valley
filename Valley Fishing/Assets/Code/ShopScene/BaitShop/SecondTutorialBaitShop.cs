using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecondTutorialBaitShop : BaitShop {

	#region Properties

	public bool AllFishSold { get => InventoryManager.Instance.TotalOwnedFish > 0; }
	private bool FishBoardNotClosedForFirstTime { get; set; }


    #endregion

	public override void Awake()
	{
		base.Awake();
        leaveShopButton.gameObject.SetActive(false);
        GameManager.Instance.InputController.SelectButton(fishBasketButton.gameObject);
		GameManager.Instance.InputController.SelectionManuallySet = false;
		transform.position = lerpObjectToPositions[0].destination.position;
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
		if (InventoryManager.Instance.TotalOwnedFish == 0 && !this.BaitboardTutorialsCompleted[0] && !fishBoard.gameObject.activeSelf) {
			Debug.Log("here");
				PlayNextTutotialVoiceOver(this.BaitboardTutorialsCompleted, baitboardTutorials);
				IncrementTutorial(this.BaitboardTutorialsCompleted);
				FishBoardNotClosedForFirstTime = true;
		}
		//if (InventoryManager.Instance.TotalOwnedFish == 0 && !this.BaitboardTutorialsCompleted[0] && fishBoard.gameObject.activeSelf) {
		//	PlayNextTutotialVoiceOver(this.FishboardTutorialsCompleted, fishboardTutorials);
		//	IncrementTutorial(this.FishboardTutorialsCompleted);
		//}
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
		if (InventoryManager.Instance.TotalOwnedFish == 0) {
			Debug.Log("here");
			PlayNextTutotialVoiceOver(this.BaitboardTutorialsCompleted, baitboardTutorials);
			IncrementTutorial(this.BaitboardTutorialsCompleted);
			FishBoardNotClosedForFirstTime = true;
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
		if (enter) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ShopEnter);
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitShopIntros[1]);
			yield return new WaitForSeconds(shopEnterTime);
			this.OnGreeting?.Invoke();			
		} else {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ShopEnter);
			this.OnGreeting?.Invoke();
			yield return new WaitForSeconds(shopEnterTime);
		}
		gameObject.SetActive(enter);
	}

	public override void FishBoardSelected() {
		base.FishBoardSelected();
		AudioManager.Instance.PlayVoiceOver(fishBoardEvent);
	}

	public override void FishBasketSelected() {
		base.FishBasketSelected();
		AudioManager.Instance.PlayVoiceOver(fishBasketEvent);
	}

	public override void BaitBoardSelected() {
		base.BaitBoardSelected();
		AudioManager.Instance.PlayVoiceOver(baitBoardEvent);
	}

	public override void LeaveShopSelected() {
		base.LeaveShopSelected();
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

	public override void LeaveShop() {
		if(InventoryManager.Instance.TotalOwnedBaits == 10) {
			SceneManager.LoadScene(LevelManager.CatchTutorial_02);
		} else {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ClickError);
		}
	}
}
