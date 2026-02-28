using FMODUnity;
using System.Collections;
using UnityEngine;

public class FirstTutorialBaitShop : BaitShop {

	#region Serialized Fields

	[SerializeField] protected EventReference fishBoardIntroEvent;
	[SerializeField] protected EventReference fishBasketIntroEvent;
	[SerializeField] protected EventReference baitBoardIntroEvent;
	[SerializeField] private GameObject lockedFishBoardButton;

	#endregion


	#region Properties
	public bool AllFishSold { get => GameManager.Instance.TotalCaughtFish > 0; }
	private bool FishBoardNotClosedForFirstTime { get; set; }
	private bool PlayFishBoardIntro { get; set; } = true;
	private bool PlayFishBasketIntro { get; set; } = true;
	private bool PlayBaitBoardIntro { get; set; } = true;
	private bool FishSold { get; set; }


    #endregion


    public override void Awake()
    {
		base.Awake();
        leaveShopButton.gameObject.SetActive(false);
        GameManager.Instance.InputController.SelectButton(lockedFishBoardButton.gameObject);
    }

	public override void VoiceLineOver(EventReference eventReference, bool skipped) {
		base.VoiceLineOver(eventReference, skipped);
		if (GameManager.Instance.CurrentBaits[4] == 5 && !baitBoard.Initialized) {
			PlayNextTutotialVoiceOver(this.BaitboardTutorialsCompleted, baitboardTutorials);
			IncrementTutorial(this.BaitboardTutorialsCompleted);
		}
		if (GameManager.Instance.CurrentBaits[4] == 5 && !this.BaitboardTutorialsCompleted[1]) {
			PlayNextTutotialVoiceOver(this.BaitboardTutorialsCompleted, baitboardTutorials);
			IncrementTutorial(this.BaitboardTutorialsCompleted);
		}
		if (GameManager.Instance.TotalCaughtFish > 0 && GameManager.Instance.TotalCaughtFish < 9 && fishBoard.Initialized) {
			if (!this.FishboardTutorialsCompleted[0]) {
				PlayNextTutotialVoiceOver(this.FishboardTutorialsCompleted, fishboardTutorials);
				IncrementTutorial(this.FishboardTutorialsCompleted);
			}
		}
		if(this.FishSold && !this.FishboardTutorialsCompleted[1]) {
			PlayNextTutotialVoiceOver(this.FishboardTutorialsCompleted, fishboardTutorials);
			IncrementTutorial(this.FishboardTutorialsCompleted);
		}
		if (GameManager.Instance.TotalCaughtFish == 0) {
			if (!FishBoardNotClosedForFirstTime) {
				PlayNextTutotialVoiceOver(this.FishboardTutorialsCompleted, fishboardTutorials);
				IncrementTutorial(this.FishboardTutorialsCompleted);
				FishBoardNotClosedForFirstTime = true;
			}
		}
	}

	public override void SellFish(int fishIndex) {
		base.SellFish(fishIndex);
		this.FishSold = true;
	}

	public override void BuyBait(int baitIndex, int baitQuantity) {
		base.BuyBait(baitIndex, baitQuantity);
	}

	public override void OpenFishBoard() {
		if(fishBoard.Initialized && GameManager.Instance.TotalCaughtFish != 0) {
			return;
		}
		if(GameManager.Instance.TotalCaughtFish == 0) {
			GameManager.Instance.InputController.SelectButton(fishBoardButton.gameObject);
			GameManager.Instance.InputController.SelectionManuallySet = false;
		}
		base.OpenFishBoard();
		if (GameManager.Instance.CurrentBaits[4] != 5) {
			leaveShopButton.gameObject.SetActive(false);
		}
		lockedFishBoardButton.gameObject.SetActive(false);
		PlayNextTutotialVoiceOver(this.FishboardTutorialsCompleted,fishboardTutorials);
		IncrementTutorial(this.FishboardTutorialsCompleted);
	}

	public override void OpenBaitBoard() {
		if (baitBoard.Initialized && GameManager.Instance.CurrentBaits[4] == 0) {
			return;
		}
		if (GameManager.Instance.CurrentBaits[4] == 5) {
			GameManager.Instance.InputController.SelectButton(baitBoardButton.gameObject);

			GameManager.Instance.InputController.SelectionManuallySet = false;
		}
		base.OpenBaitBoard();
		PlayNextTutotialVoiceOver(this.BaitboardTutorialsCompleted, baitboardTutorials);
		IncrementTutorial(this.BaitboardTutorialsCompleted);
	}

	public override IEnumerator EnterShop(bool enter) {
		yield return StartCoroutine(base.EnterShop(enter));
		if (enter) {
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitShopIntros[0]);
		}
	}

	public override void FishBoardSelected() {
		if (this.PlayFishBoardIntro) {
			AudioManager.Instance.PlayVoiceOver(fishBoardIntroEvent);
			this.PlayFishBoardIntro = false;
			return;
		}
		AudioManager.Instance.PlayVoiceOver(fishBoardEvent);
	}

	public override void FishBasketSelected() {
		if (this.PlayFishBasketIntro) {
			AudioManager.Instance.PlayVoiceOver(fishBasketIntroEvent);
			this.PlayFishBasketIntro = false;
			return;
		}
		AudioManager.Instance.PlayVoiceOver(fishBasketEvent);
	}

	public override void BaitBoardSelected() {
		if (this.PlayBaitBoardIntro) {
			AudioManager.Instance.PlayVoiceOver(baitBoardIntroEvent);
			this.PlayBaitBoardIntro = false;
			return;
		}
		AudioManager.Instance.PlayVoiceOver(baitBoardEvent);
	}
	public override void LeaveShopSelected() {
		AudioManager.Instance.PlayVoiceOver(leaveShopEvent);
	}

	public override void Skip() {
		base.Skip();
		if (GameManager.Instance.TotalCaughtFish == 0 && fishBoard.Initialized) {
			OpenFishBoard();
		}
		if (GameManager.Instance.CurrentBaits[4] == 5 && baitBoard.Initialized) {
			OpenBaitBoard();
		}
	}
}