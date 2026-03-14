using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThirdTutorialVoiceOver : VoiceOverController
{
	public override bool PerformStateSwitch() {
		switch (this.LevelController.CurrentState) {
			case LevelController.State.Idle:
				if (!AllTutorialsCompleted(this.AttatchBaitTutorialsCompleted)) {
					PlayNextTutotialVoiceOver(this.AttatchBaitTutorialsCompleted, applyBaitTutorials);
					IncrementTutorial(this.AttatchBaitTutorialsCompleted);
				}
				break;
			case LevelController.State.AttatchBait:
				if (GameManager.Instance.TotalBaitsLeft == 0) {
					GameManager.Instance.LevelController.FishView.EnableFishUI(false);
					SceneManager.LoadScene(LevelManager.BossTutorial_00);
					return false;
				}
				break;
			case LevelController.State.ReelingFish:
				break;
			case LevelController.State.FishCaught:
				if (GameManager.Instance.TotalBaitsLeft == 0) {
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
		if (!base.PerformStateSwitch()) {
			return false;
		}
		return true;
	}

	public override void FishStrafed(FishDatas.FishData.MovementDirection movementDirection) {
		//if (AllTutorialsCompleted(this.ReelTutorialsCompleted)) {
		//	return;
		//}
		//PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
		//IncrementTutorial(this.ReelTutorialsCompleted);
	}

	public override void FishSpawned() {
		//if (this.ReelTutorialsCompleted[0] == false) {
		//	this.CurrentFish.ActivityLevels.Clear();
		//	this.CurrentFish.ActivityLevels.Add(Fish.ActivityLevel.active);
		//} else if (this.ReelTutorialsCompleted[1] == false) {
		//	this.CurrentFish.ActivityLevels.Clear();
		//	this.CurrentFish.ActivityLevels.Add(Fish.ActivityLevel.calm);
		//}
	}
}
