using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FisrstTutorialBaitShop : BaitShop
{
	protected override void EnterState(State state) {
		base.EnterState(state);
		switch (state) {
			case State.Defualt:
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

    public override void VoiceLineOver(EventInstance eventInstance, bool skipped)
    {
		base.VoiceLineOver(eventInstance,skipped);
        switch (this.CurrentState)
        {
            case State.Defualt:
                break;
            case State.Entering:
                break;
            case State.Trading:
				if(this.CurrentVoiceOverChain.Count > 0 && this.ChainVoiceOverPosition < this.CurrentVoiceOverChain.Count) {
					PlayVoiceOverChain();
				}
                if (tutorialState == TutorialState.BuyingTutorial) {
					GameManager.Instance.InputController.SelectButton(initialBaitButton);
                    tutorialState = TutorialState.TutorialsOver;
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
				this.CurrentVoiceOverChain.Clear();
				this.CurrentVoiceOverChain.Add(FMODManager.Instance.BaitShopSellYourItems[0]);
				this.CurrentVoiceOverChain.Add(FMODManager.Instance.price);
				PlayVoiceOverChain();
				break;
			case SellTpye.SellIndividualFish:
				break;
			default:
				break;
		}
	}

	public override void BuyBait(int baitIndex) {
		base.BuyBait(baitIndex);
		if (!this.TutorialBaitBought) {
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitShopThanks[0]);
			this.TutorialBaitBought = true;
		} else {
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitShopThanks[1]);
		}
	}

	public override IEnumerator EnterShop(bool enter) {
		yield return StartCoroutine(base.EnterShop(enter));
		if (enter) {
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitShopIntros[0]);
		}
	}

	public override void PlayVoiceOverChain() {
		base.PlayVoiceOverChain();
	}

	public override void VoiceOverChainFinished() {
		if (this.CurrentVoiceOverChain.Count == 0) {
			return;
		}
		AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitShopTutorialItemIntros[0]);
		base.VoiceOverChainFinished();
		StartCoroutine(WaitOneFrame(SetBuyState));
	}

	private void SetBuyState() {
		tutorialState = TutorialState.BuyingTutorial;
	}

}
