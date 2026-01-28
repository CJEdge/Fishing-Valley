using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class FishBoardButton : ButtonVoiceOverComponent
{
	[SerializeField]
	private FishBoard fishBoard;

	public override void DoHoverEffect() {
		int fishValue = GameManager.Instance.Fish[transform.GetSiblingIndex()].SellPrice;
		int caughtFishCount = GameManager.Instance.CaughtFish[transform.GetSiblingIndex()];

		List <EventReference> voiceoverChain = new List<EventReference>();
		voiceoverChain.Add(FMODManager.Instance.FishBoardFish[transform.GetSiblingIndex()]);
		//voiceoverChain.Add(FMODManager.Instance.AreWorth);
		for (int i = 0; i < FMODManager.Instance.GetNumber(fishValue).Count; i++) {
			voiceoverChain.Add(FMODManager.Instance.GetNumber(fishValue)[i]);
		}
		voiceoverChain.Add(FMODManager.Instance.Gold);
		voiceoverChain.Add(FMODManager.Instance.YouHave);
		for (int i = 0; i < FMODManager.Instance.GetNumber(caughtFishCount).Count; i++) {
			voiceoverChain.Add(FMODManager.Instance.GetNumber(caughtFishCount)[i]);
		}
		AudioManager.Instance.PlayVoiceOverChain(voiceoverChain);
	}

	public void OnClick() {
		GameManager.Instance.ShopController.BaitShop.SellFish(transform.GetSiblingIndex());
	}
}
