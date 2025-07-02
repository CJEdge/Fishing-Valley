using FMOD.Studio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecondTutorialVoiceOverController : VoiceOverController
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
			case LevelController.State.ReelingFish:
				if (GameManager.Instance.CurrentFish.IsTutorial || GameManager.Instance.TotalCaughtFish == 3) {
					PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
					IncrementTutorial(this.ReelTutorialsCompleted);
				}
				break;
			case LevelController.State.FishCaught:
				if(GameManager.Instance.TotalBaitsLeft == 0) {
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
		return true;
	}

	public override void VoiceOverFinished(EventInstance eventInstance, bool skipped) {
		base.VoiceOverFinished(eventInstance,skipped);
		switch (this.LevelController.CurrentState) {
			case LevelController.State.FishCaught:
				if (AllTutorialsCompleted(this.CaughtFishTutorialsCompleted)) {
					GameManager.Instance.LevelController.FishView.EnableFishUI(false);
					SceneManager.LoadScene(LevelManager.CatchTutorial_02);
				}
				break;
			default:
				break;
		}
	}
}
