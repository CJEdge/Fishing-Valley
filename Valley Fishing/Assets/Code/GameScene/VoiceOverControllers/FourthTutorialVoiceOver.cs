using FMOD.Studio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FourthTutorialVoiceOver : VoiceOverController
{
	public override bool PerformStateSwitch() {
		switch (this.LevelController.CurrentState) {
			case LevelController.State.ReelingFish:
				if (GameManager.Instance.CurrentFish.IsTutorial || GameManager.Instance.TotalCaughtFish == 3) {
					PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
					IncrementTutorial(this.ReelTutorialsCompleted);
				}
				break;
			case LevelController.State.FishCaught:
				if (this.CurrentFish.IsTutorial) {
					//AudioManager.Instance.PlayVoiceOver(this.CurrentFish.TutorialCaughtVoiceLine);
				} else {
					AudioManager.Instance.PlayVoiceOver(this.CurrentFish.CaughtVoiceLine);
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
				if (GameManager.Instance.TotalBaitsLeft == 0) {
					GameManager.Instance.LevelController.FishView.EnableFishUI(false);
					SceneManager.LoadScene(LevelManager.ShopTutorial_03);
				}
				break;
			default:
				break;
		}
	}
}
