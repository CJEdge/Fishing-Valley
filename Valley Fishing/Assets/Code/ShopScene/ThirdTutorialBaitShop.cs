using FMOD.Studio;
using System.Collections;
using UnityEngine;

public class ThirdTutorialBaitShop : BaitShop {
	protected override void EnterState(State state) {
		base.EnterState(state);
		switch (state) {
			case State.Defualt:
				leaveShopButton.SetActive(false);
				break;
			case State.Entering:
				break;
			case State.Trading:
				break;
			case State.Leaving:
				break;
			default:
				break;
		}
	}

	public override void VoiceLineOver(EventInstance eventInstance, bool skipped) {
		base.VoiceLineOver(eventInstance, skipped);
		switch (this.CurrentState) {
			case State.Defualt:
				break;
			case State.Entering:
				break;
			case State.Trading:
				switch (tutorialState) {
					case TutorialState.SellingTutorial:
						if (GameManager.Instance.TotalCaughtFish == 0 && !AudioManager.Instance.InVoiceOverChain) {
							StartCoroutine(WaitOneFrame(SetBuyState));
						}
						break;
					case TutorialState.BuyingTutorial:
						//GameManager.Instance.InputController.SelectButton(initialBaitButton);
						//tutorialState = TutorialState.TutorialsOver;
						break;
					case TutorialState.TutorialsOver:
						break;
					default:
						break;
				}
				break;
			case State.Leaving:
				break;
			default:
				break;
		}
	}

	public override void SellFish() {
		base.SellFish();
		switch (sellTpye) {
			case SellTpye.SellAllFish:
				//List<EventReference> voiceOverChain = new List<EventReference>();
				//voiceOverChain.Add(FMODManager.Instance.BaitShopSellYourItems[0]);
				//voiceOverChain.Add(FMODManager.Instance.price);
				//AudioManager.Instance.PlayVoiceOverChain(voiceOverChain);
				sellButton.SetActive(false);
				AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitShopTutorialItemIntros[2]);
				break;
			case SellTpye.SellIndividualFish:
				break;
			default:
				break;
		}
	}

	public override void BuyBait(int baitIndex) {
		base.BuyBait(baitIndex);
		initialBaitButton.SetActive(false);
		leaveShopButton.SetActive(true);
		GameManager.Instance.InputController.SelectButton(leaveShopButton);
		AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.LeaveShopPrompts[2]);

		//if (!this.TutorialBaitBought) {
		//	AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitShopThanks[0]);
		//	this.TutorialBaitBought = true;
		//} else {
		//	AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitShopThanks[1]);
		//}
	}

	public override IEnumerator EnterShop(bool enter) {
		yield return StartCoroutine(base.EnterShop(enter));
		if (enter) {
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitShopIntros[2]);
			initialBaitButton.SetActive(false);
		}
	}

	private void SetBuyState() {
		tutorialState = TutorialState.BuyingTutorial;
		initialBaitButton.SetActive(true);
		GameManager.Instance.InputController.SelectButton(initialBaitButton);
		tutorialState = TutorialState.TutorialsOver;
		//AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitShopTutorialItemIntros[0]);
	}

}