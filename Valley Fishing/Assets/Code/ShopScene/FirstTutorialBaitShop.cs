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

	public override void SellFish() {
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
