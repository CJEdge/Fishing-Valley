using FMOD.Studio;
using UnityEngine;

public class FisrstTutorialBaitShop : BaitShop
{
	protected override void EnterState(State state) {
		base.EnterState(state);
		switch (state) {
			case State.Defualt:
				break;
			case State.Entering:
				AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitShopIntros[0]);
				break;
			case State.Trading:
				break;
			case State.Leaving:
				break;
			default:
				break;
		}
	}

    public override void VoiceLineOver(EventInstance eventInstance)
    {
		base.VoiceLineOver(eventInstance);
        switch (this.CurrentState)
        {
            case State.Defualt:
                break;
            case State.Entering:
                break;
            case State.Trading:
                if (this.FishSellPrice > 0)
                {
                    AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.price);
                    this.FishSellPrice = 0;
                    StartCoroutine(WaitOneFrameThenChangeSellState(TutorialState.BuyingTutorial));
                }
                if (tutorialState == TutorialState.BuyingTutorial)
                {
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

    public override void SellFish() {
		base.SellFish();
		switch (sellTpye) {
			case SellTpye.SellAllFish:
				AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitShopSellYourItems[0]);
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
}
