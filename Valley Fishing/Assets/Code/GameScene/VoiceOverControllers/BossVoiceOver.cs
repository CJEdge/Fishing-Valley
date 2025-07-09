using UnityEngine;
using UnityEngine.SceneManagement;

public class BossVoiceOver : VoiceOverController
{
	public override bool PerformStateSwitch() {
		if (!base.PerformStateSwitch()) {
			return false;
		}
		switch (this.LevelController.CurrentState) {
			case LevelController.State.Idle:
				GameManager.Instance.CurrentBaits[8] = 1;
				if (!AllTutorialsCompleted(this.AttatchBaitTutorialsCompleted)) {
					PlayNextTutotialVoiceOver(this.AttatchBaitTutorialsCompleted, applyBaitTutorials);
					IncrementTutorial(this.AttatchBaitTutorialsCompleted);
				}
				break;
			case LevelController.State.AttatchBait:
				if (GameManager.Instance.TotalBaitsLeft == 0) {
					GameManager.Instance.LevelController.FishView.EnableFishUI(false);
					SceneManager.LoadScene(LevelManager.Menu);
					return false;
				}
				break;
			case LevelController.State.ReelingFish:				
				PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
				IncrementTutorial(this.ReelTutorialsCompleted);
				break;
			case LevelController.State.FishCaught:
				AudioManager.Instance.PlayVoiceOver(tutorialCatchVoices[0]);
				break;
			default:
				break;
		}
		return true;
	}
}
