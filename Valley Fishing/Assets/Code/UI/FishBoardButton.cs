using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class FishBoardButton : ButtonVoiceOverComponent
{
	[SerializeField]
	private FishBoard fishBoard;

	public override void DoHoverEffect() {
		List <EventReference> voiceoverChain = new List<EventReference>();
		voiceoverChain.Add(FMODManager.Instance.FishBoardFish[transform.GetSiblingIndex()]);
		voiceoverChain.Add(FMODManager.Instance.AreWorth);
		voiceoverChain.Add(FMODManager.Instance.BaitNumbers[0]);
		voiceoverChain.Add(FMODManager.Instance.YouHave);
		voiceoverChain.Add(FMODManager.Instance.BaitNumbers[GameManager.Instance.CaughtFish[transform.GetSiblingIndex()]]);
		AudioManager.Instance.PlayVoiceOverChain(voiceoverChain);
	}
}
