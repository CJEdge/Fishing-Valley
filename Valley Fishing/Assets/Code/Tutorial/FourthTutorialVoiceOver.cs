using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FourthTutorialVoiceOver : VoiceOverController
{
	public override bool PerformStateSwitch() {
		if (!base.PerformStateSwitch()) {
			return false;
		}
		switch (this.LevelController.CurrentState) {
			case LevelController.State.Idle:
				if (!AllTutorialsCompleted(this.AttatchBaitTutorialsCompleted)) {
					PlayNextTutotialVoiceOver(this.AttatchBaitTutorialsCompleted, applyBaitTutorials);
					IncrementTutorial(this.AttatchBaitTutorialsCompleted);
				}
				break;
			case LevelController.State.AttatchBait:
				if (InventoryManager.Instance.TotalOwnedFish == 5) {
					GameManager.Instance.LevelController.FishView.EnableFishUI(false);
					SceneManager.LoadScene(LevelManager.BossTutorial_00);
					return false;
				}
				break;
			case LevelController.State.ReelingFish:
				if (GameManager.Instance.CurrentFish.IsTutorial || InventoryManager.Instance.TotalOwnedFish < 1) {
				PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
				IncrementTutorial(this.ReelTutorialsCompleted);
				}
				break;
			case LevelController.State.FishCaught:
				if (InventoryManager.Instance.TotalOwnedBaits == 0) {
					List<EventReference> voiceOverChain = new List<EventReference>();
					voiceOverChain.Add(this.CurrentFish.FishData.fishCatchAudio);
					voiceOverChain.Add(tutorialCatchVoices[0]);
					AudioManager.Instance.PlayVoiceOverChain(voiceOverChain);
				} else {
					if (this.CurrentFish.IsTutorial) {
						//AudioManager.Instance.PlayVoiceOver(this.CurrentFish.TutorialCaughtVoiceLine);
					} else {
						AudioManager.Instance.PlayVoiceOver(this.CurrentFish.FishData.fishCatchAudio);
					}
				}
				break;
			default:
				break;
		}
		return true;
	}

	public override void VoiceOverFinished(EventReference eventReference, bool skipped) {
		base.VoiceOverFinished(eventReference, skipped);
	}
}
