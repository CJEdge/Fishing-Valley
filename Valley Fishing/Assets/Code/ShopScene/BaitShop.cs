using FMOD.Studio;
using System.Collections;
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
	private GameObject sellButton;

	[SerializeField]
	private GameObject initialBaitButton;

	[SerializeField]
	private EventSystem eventSystem;

	[SerializeField]
	private int[] baitQuantities;


	#endregion


	#region Properties

	private int FishSellPrice {
		get;
		set;
	}

	public bool TutorialBaitBought {
		get;
		set;
	}

	#endregion


	#region Shop

	protected override void EnterState(State state) {
		switch (state) {
			case State.Defualt:
				AudioManager.Instance.VoiceLineOver += VoiceLineOver;
				break;
			case State.Entering:
				baitShopObject.SetActive(true);
				break;
			case State.Trading:
				eventSystem.SetSelectedGameObject(sellButton);
				break;
			case State.Leaving:
				baitShopObject.SetActive(false);
				break;
			default:
				break;
		}
	}

	#endregion


	#region Private Methods

	public override void VoiceLineOver(EventInstance eventInstance) {
		switch (this.CurrentState) {
			case State.Defualt:
				break;
			case State.Entering:
				SetState(State.Trading);
				break;
			case State.Trading:
				if(this.FishSellPrice > 0) {
					AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.price);
					this.FishSellPrice = 0;
					StartCoroutine(WaitOneFrameThenChangeSellState(TutorialState.BuyingTutorial));
				}
				if(tutorialState == TutorialState.BuyingTutorial) {
					AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitShopTutorialItemIntros[0]);
					StartCoroutine(WaitOneFrameThenChangeSellState(TutorialState.TutorialsOver));
				}
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
					for (int j = 0; j < GameManager.Instance.CaughtFish[i]; j++) {
						GameManager.Instance.Money += GameManager.Instance.Fish[GameManager.Instance.CaughtFish[i]].SellPrice;
						this.FishSellPrice += GameManager.Instance.Fish[GameManager.Instance.CaughtFish[i]].SellPrice;
						GameManager.Instance.CaughtFish[i]--;
					}
				}
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
	}

	public void LeaveBaitShop() {
		GameManager.Instance.ShopController.ShoreMenu.FinishedInShop(this);
		baitShopObject.SetActive(false);
		GameManager.Instance.ShopController.SetState(ShopController.State.Shore);
	}

	#endregion


	#region Private Methods

	private IEnumerator WaitOneFrameThenChangeSellState(TutorialState state) {
		yield return new WaitForEndOfFrame();
		tutorialState = state;
	}

	#endregion

}
