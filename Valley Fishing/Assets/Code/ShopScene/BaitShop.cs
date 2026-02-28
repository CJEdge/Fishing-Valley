using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaitShop : Shop {

	#region Serialized Fields

	[SerializeField] private GameObject baitShopObject;
	[SerializeField] protected FishBoard fishBoard;
	[SerializeField] protected BaitBoard baitBoard;
	[SerializeField] protected EventSystem eventSystem;
	[SerializeField] private float shopEnterTime;
	[SerializeField] protected EventReference[] fishboardTutorials;
	[SerializeField] protected EventReference[] baitboardTutorials;
	[SerializeField] protected ButtonVoiceOverComponent fishBoardButton;
	[SerializeField] protected ButtonVoiceOverComponent fishBasketButton;
	[SerializeField] protected ButtonVoiceOverComponent baitBoardButton;
	[SerializeField] protected ButtonVoiceOverComponent leaveShopButton;
	[SerializeField] protected EventReference fishBoardEvent;
	[SerializeField] protected EventReference fishBasketEvent;
	[SerializeField] protected EventReference baitBoardEvent;
	[SerializeField] protected EventReference leaveShopEvent;

	#endregion


	#region Properties

	public int FishSellPrice { get;	set; }
	public bool TutorialBaitBought { get; set; }
	public Coroutine RunEnterShop {	get; set; }
	public bool[] FishboardTutorialsCompleted { get; set; }
	public bool[] BaitboardTutorialsCompleted { get; set; }
	[field:SerializeField] public int[] BaitQuantities { get; set; }

    #endregion


    #region Shop

    public override void Awake()
    {
		base.Start();
		for (int i = 0; i < fishboardTutorials.Length; i++)
		{
			this.FishboardTutorialsCompleted = new bool[fishboardTutorials.Length];
		}
		for (int i = 0; i < baitboardTutorials.Length; i++)
		{
			this.BaitboardTutorialsCompleted = new bool[baitboardTutorials.Length];
		}
		fishBoardButton.SelectAction += FishBoardSelected;
		fishBasketButton.SelectAction += FishBasketSelected;
		baitBoardButton.SelectAction += BaitBoardSelected;
		leaveShopButton.SelectAction += LeaveShopSelected;
        this.RunEnterShop = StartCoroutine(EnterShop(true));
        //		case State.Trading:
        //			GameManager.Instance.InputController.SelectButton(fishBoardButton.gameObject);
        //			break;
    }
    //protected override void EnterState(State state) {
    //	switch (state) {
    //		case State.Defualt:
    //			for (int i = 0; i < fishboardTutorials.Length; i++) {
    //				this.FishboardTutorialsCompleted = new bool[fishboardTutorials.Length];
    //			}
    //			for (int i = 0; i < baitboardTutorials.Length; i++) {
    //				this.BaitboardTutorialsCompleted = new bool[baitboardTutorials.Length];
    //			}
    //			fishBoardButton.SelectAction += FishBoardSelected;
    //			fishBasketButton.SelectAction += FishBasketSelected;
    //			baitBoardButton.SelectAction += BaitBoardSelected;
    //			leaveShopButton.SelectAction += LeaveShopSelected;
    //			break;
    //		case State.Entering:
    //			this.RunEnterShop = StartCoroutine(EnterShop(true));
    //			break;
    //		case State.Trading:
    //			GameManager.Instance.InputController.SelectButton(fishBoardButton.gameObject);
    //			break;
    //		case State.Leaving:
    //			break;
    //		default:
    //			break;
    //	}
    //}

    public override void OnDestroy() {
		base.OnDestroy();
		fishBoardButton.SelectAction -= FishBoardSelected;
		fishBasketButton.SelectAction -= FishBasketSelected;
		baitBoardButton.SelectAction -= BaitBoardSelected;
		leaveShopButton.SelectAction -= LeaveShopSelected;
	}

	#endregion


	#region Private Methods

	public override void VoiceLineOver(EventReference eventReference, bool skipped) {
		//switch (this.CurrentState) {
		//	case State.Defualt:
		//		break;
		//	case State.Entering:
		//		break;
		//	case State.Trading:
		//		break;
		//	case State.Leaving:
		//		break;
		//	default:
		//		break;
		//}
	}

	#endregion


	#region Public Methods

	public virtual void SellFish(int fishIndex) {
		if(GameManager.Instance.CaughtFish[fishIndex] == 0) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ClickError);
			return;
		}
		AudioManager.Instance.SkipVoiceOver();
		for (int i = 0; i < GameManager.Instance.CaughtFish[fishIndex]; i++) {
			GameManager.Instance.Money += GameManager.Instance.Fish[fishIndex].SellPrice;
			this.FishSellPrice += GameManager.Instance.Fish[fishIndex].SellPrice;
		}
		if (GameManager.Instance.CaughtFish[fishIndex] > 0) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.MoneyEarnt);
			List<EventReference> voiceOverChain = new List<EventReference>();
			voiceOverChain.Add(FMODManager.Instance.BaitShopSoldItem[0]);
			for (int i = 0; i < FMODManager.Instance.GetNumber(GameManager.Instance.Money).Count; i++) {
				voiceOverChain.Add(FMODManager.Instance.GetNumber(GameManager.Instance.Money)[i]);
			}
			voiceOverChain.Add(FMODManager.Instance.Gold);
			AudioManager.Instance.PlayVoiceOverChain(voiceOverChain);
		}
		StartCoroutine(WaitOneFrame(PerformSellFish, fishIndex));
	}

	public virtual void SellAllFish() {
		if(GameManager.Instance.TotalCaughtFish == 0) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ClickError);
			return;
		}
		for (int i = 0; i < GameManager.Instance.CaughtFish.Count; i++) {
			if (GameManager.Instance.CaughtFish[i] == 0) {
				continue;
			}
			for (int j = GameManager.Instance.CaughtFish[i] - 1; j >= 0; j--) {
				GameManager.Instance.Money += GameManager.Instance.Fish[i].SellPrice;
				this.FishSellPrice += GameManager.Instance.Fish[i].SellPrice;
				GameManager.Instance.CaughtFish[i]--;
			}
		}
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.MoneyEarnt);
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.MoneyEarnt);
		List<EventReference> voiceOverChain = new List<EventReference>();
		voiceOverChain.Add(FMODManager.Instance.BaitShopSoldItem[0]);
		for (int i = 0; i < FMODManager.Instance.GetNumber(GameManager.Instance.Money).Count; i++) {
			voiceOverChain.Add(FMODManager.Instance.GetNumber(GameManager.Instance.Money)[i]);
		}
		voiceOverChain.Add(FMODManager.Instance.Gold);
		AudioManager.Instance.PlayVoiceOverChain(voiceOverChain);
	}

	public virtual void BuyBait(int baitIndex, int sellQuantity) {
		Debug.Log(this.BaitQuantities[baitIndex]);
		if(this.BaitQuantities[baitIndex] == 0) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ClickError);
			return;
		}
		if (GameManager.Instance.Baits[baitIndex].BaitPrice * sellQuantity > GameManager.Instance.Money) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ClickError);
			return;
		}
		GameManager.Instance.Money -= GameManager.Instance.Baits[baitIndex].BaitPrice * sellQuantity;
		GameManager.Instance.CurrentBaits[baitIndex] += sellQuantity;
		this.BaitQuantities[baitIndex] -= sellQuantity;
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.MoneyEarnt);
		List<EventReference> voiceOverChain = new List<EventReference>();
		voiceOverChain.Add(FMODManager.Instance.BaitShopSoldItem[0]);
		for (int i = 0; i < FMODManager.Instance.GetNumber(GameManager.Instance.Money).Count; i++) {
			voiceOverChain.Add(FMODManager.Instance.GetNumber(GameManager.Instance.Money)[i]);
		}
		voiceOverChain.Add(FMODManager.Instance.Gold);
		AudioManager.Instance.PlayVoiceOverChain(voiceOverChain);
	}

	public void LeaveBaitShop() {
		if (AudioManager.Instance.VoiceLineInProgress) {
			return;
		}
		this.RunEnterShop = StartCoroutine(EnterShop(false));
	}

	#endregion


	#region Private Methods

	private void PerformSellFish(int fishIndex) {
		GameManager.Instance.CaughtFish[fishIndex] = 0;
	}

	public virtual IEnumerator EnterShop(bool enter) {
		if (enter) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.ShopEnter);
			yield return new WaitForSeconds(shopEnterTime);
			baitShopObject.SetActive(enter);
		} else {
			this.baitShopObject.SetActive(false);
			GameManager.Instance.ShopController.ShoreMenu.FinishedInShops[0] = true;
			GameManager.Instance.ShopController.ShoreMenu.gameObject.SetActive(true);
			GameManager.Instance.ShopController.SetState(ShopController.State.Shore);
		}
	}

	public virtual IEnumerator WaitOneFrame(Action<int> callback, int integer) {
		yield return new WaitForEndOfFrame();
		callback?.Invoke(integer);
	}

	public virtual void OpenFishBoard() {
		fishBoard.OpenFishBoard();
	}
	public virtual void OpenBaitBoard() {
		baitBoard.OpenBaitBoard();
	}

	public override void Skip() {
		base.Skip();
		AudioManager.Instance.DisableSkipping();
	}

	public virtual void FishBoardSelected() {
	}

	public virtual void FishBasketSelected() {

	}

	public virtual void BaitBoardSelected() {

	}
	public virtual void LeaveShopSelected() {

	}

	#endregion

}
