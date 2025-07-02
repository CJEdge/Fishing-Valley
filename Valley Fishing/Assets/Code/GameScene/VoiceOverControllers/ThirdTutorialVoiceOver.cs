using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThirdTutorialVoiceOver : VoiceOverController
{
	public override bool PerformStateSwitch() {
		switch (this.LevelController.CurrentState) {
			case LevelController.State.ReelingFish:
				if(this.ReelTutorialsCompleted[0] == false) {
					this.CurrentFish.movementDirections.Clear(); ;
					this.CurrentFish.movementDirections.Add(Fish.MovementDirection.left);
					this.CurrentFish.CurrentActivityLevel = Fish.ActivityLevel.active;
				} else if (this.ReelTutorialsCompleted[1] == false ) {
					this.CurrentFish.movementDirections.Clear(); ;
					this.CurrentFish.movementDirections.Add(Fish.MovementDirection.right);
					this.CurrentFish.CurrentActivityLevel = Fish.ActivityLevel.calm;
				} else if (this.ReelTutorialsCompleted[2] == false) {
					PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
					IncrementTutorial(this.ReelTutorialsCompleted);
				}
				break;
			case LevelController.State.FishCaught:
				if (GameManager.Instance.TotalBaitsLeft == 0) {
					PlayNextTutotialVoiceOver(this.CaughtFishTutorialsCompleted, tutorialCatchVoices);
					IncrementTutorial(this.CaughtFishTutorialsCompleted);
				} else {
					if (this.CurrentFish.IsTutorial) {
						//AudioManager.Instance.PlayVoiceOver(this.CurrentFish.TutorialCaughtVoiceLine);
					} else {
						AudioManager.Instance.PlayVoiceOver(this.CurrentFish.CaughtVoiceLine);
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

	public override void VoiceOverFinished(EventInstance eventInstance, bool skipped) {
		base.VoiceOverFinished(eventInstance, skipped);
		switch (this.LevelController.CurrentState) {
			case LevelController.State.FishCaught:
				if (AllTutorialsCompleted(this.CaughtFishTutorialsCompleted)) {
					GameManager.Instance.LevelController.FishView.EnableFishUI(false);
					SceneManager.LoadScene(LevelManager.CatchTutorial_03);
				}
				break;
			default:
				break;
		}
	}

	public override void FishStrafed(Fish.MovementDirection movementDirection) {
		if (AllTutorialsCompleted(this.ReelTutorialsCompleted)) {
			return;
		}
		PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
		IncrementTutorial(this.ReelTutorialsCompleted);
	}

	public override void FishSpawned() {
		if (this.ReelTutorialsCompleted[0] == false) {
			Debug.Log("0");
			this.CurrentFish.ActivityLevels.Clear();
			this.CurrentFish.ActivityLevels.Add(Fish.ActivityLevel.active);
		} else if (this.ReelTutorialsCompleted[1] == false) {
			Debug.Log("1");
			this.CurrentFish.ActivityLevels.Clear();
			this.CurrentFish.ActivityLevels.Add(Fish.ActivityLevel.calm);
		}
	}
}
