using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaitShop : Shop {

	#region States

	public enum SellTpye {
		SellAllFish,
		SellIndividualFish
	}

	public SellTpye sellTpye;

	public enum TutorialState {
		SellingTutorial,
		BuyingTutorial,
		TutorialsOver
	}

	public TutorialState tutorialState;

	#endregion


	#region Serialized Fields

	[SerializeField]
	private GameObject baitShopObject;

	[SerializeField]
	protected GameObject sellButton;

	[SerializeField]
	protected GameObject initialBaitButton;

	[SerializeField]
	protected GameObject leaveShopButton;

	[SerializeField]
	protected EventSystem eventSystem;

	[SerializeField]
	private int[] baitQuantities;

	[SerializeField]
	private float shopEnterTime;


	#endregion


	#region Properties

	public int FishSellPrice {
		get;
		set;
	}

	public bool TutorialBaitBought {
		get;
		set;
	}

	public Coroutine RunEnterShop {
		get;
		set;
	}

	#endregion


	#region Shop

	protected override void EnterState(State state) {
		switch (state) {
			case State.Defualt:
				AudioManager.Instance.OnVoiceLineOver += VoiceLineOver;
				break;
			case State.Entering:
				this.RunEnterShop = StartCoroutine(EnterShop(true));
				break;
			case State.Trading:
				GameManager.Instance.InputController.SelectButton(sellButton);
				break;
			case State.Leaving:
				break;
			default:
				break;
		}
	}

	#endregion


	#region Private Methods

	public override void VoiceLineOver(EventInstance eventInstance, bool skipped) {
		switch (this.CurrentState) {
			case State.Defualt:
				break;
			case State.Entering:
				StopCoroutine(this.RunEnterShop);
				SetState(State.Trading);
				break;
			case State.Trading:
				break;
			case State.Leaving:
				break;
			default:
				break;
		}
	}

	#endregion


	#region Public Methods

	public virtual void SellFish() {
		switch (sellTpye) {
			case SellTpye.SellAllFish:
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
				break;
			case SellTpye.SellIndividualFish:
				break;
			default:
				break;
		}
	}
	public virtual void BuyBait(int baitIndex) {
		if (GameManager.Instance.Baits[baitIndex].BaitPrice < GameManager.Instance.Money) {
			GameManager.Instance.Money -= GameManager.Instance.Baits[baitIndex].BaitPrice;
			GameManager.Instance.CurrentBaits[baitIndex] += baitQuantities[baitIndex];
		}
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.ItemBuy);
	}

	public void LeaveBaitShop() {
		if (AudioManager.Instance.VoiceLineInProgress) {
			return;
		}
		this.RunEnterShop = StartCoroutine(EnterShop(false));
	}

	#endregion


	#region Private Methods

	public virtual IEnumerator EnterShop(bool enter) {
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.ShopEnter);
		yield return new WaitForSeconds(shopEnterTime);
		baitShopObject.SetActive(enter);
		if (!enter) {
			GameManager.Instance.ShopController.ShoreMenu.FinishedInShop(this);
			GameManager.Instance.ShopController.SetState(ShopController.State.Shore);
		}
	}

	public virtual IEnumerator WaitOneFrame(Action callback) {
		yield return new WaitForEndOfFrame();
		callback?.Invoke();
	}

	#endregion

}
