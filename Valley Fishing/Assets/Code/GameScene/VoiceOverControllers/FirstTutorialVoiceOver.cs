using FMOD.Studio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstTutorialVoiceOver : VoiceOverController
{

	public override bool PerformStateSwitch() {
		if (!base.PerformStateSwitch()) {
			return false;
		}
		switch (this.LevelController.CurrentState) {
			case LevelController.State.Idle:
				for (int i = 0; i < this.AttatchBaitTutorialsCompleted.Length; i++) {
					if (!this.AttatchBaitTutorialsCompleted[i]) {
						if (!AttatchBaitTutorialExtras(i)) {
							GameManager.Instance.CurrentBaits[i] = 1;
							AudioManager.Instance.PlayVoiceOver(applyBaitTutorials[i]);
						}
						break;
					}
				}
				this.CurrentTutorialEventInstance = AudioManager.Instance.VoiceLineEventInstance;
				break;
			case LevelController.State.AttatchBait:
				for (int i = 0; i < this.AttatchBaitTutorialsCompleted.Length; i++) {
					if (!this.AttatchBaitTutorialsCompleted[i]) {
						if (!AttatchBaitTutorialCompleteExtras(i)) {
							this.AttatchBaitTutorialsCompleted[i] = true;
						}
						break;
					}
				}
				break;
			case LevelController.State.ReelingFish:
				if (GameManager.Instance.CurrentFish.IsTutorial || GameManager.Instance.TotalCaughtFish == 3) {
					PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
					IncrementTutorial(this.ReelTutorialsCompleted);
				}
				break;
			case LevelController.State.FishCaught:
				if(GameManager.Instance.TotalCaughtFish == 9) {
					SceneManager.LoadScene(LevelManager.ShopTutorial_00);
				}
				if (AllTutorialsCompleted(this.CaughtFishTutorialsCompleted) || !GameManager.Instance.CurrentFish.IsTutorial) {
					AudioManager.Instance.PlayVoiceOver(this.CurrentFish.CaughtVoiceLine);
					this.CurrentTutorialEventInstance = AudioManager.Instance.VoiceLineEventInstance;

				} else {
					PlayNextTutotialVoiceOver(this.CaughtFishTutorialsCompleted, tutorialCatchVoices);
					IncrementTutorial(this.CaughtFishTutorialsCompleted);
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
				if (GameManager.Instance.CurrentFish.FishIndex == 3) {
					GameManager.Instance.LevelController.FishView.EnableFishUI(false);
					SceneManager.LoadScene(LevelManager.ShopTutorial_00);
				}
				break;
			default:
				break;
		}
	}

	private bool AttatchBaitTutorialExtras(int currentTutorialIndex) {
		if (currentTutorialIndex == 3) {
			if (GameManager.Instance.CurrentBaits[currentTutorialIndex] == 0 && GameManager.Instance.TotalCaughtFish < 9) {
				GameManager.Instance.CurrentBaits[currentTutorialIndex] = 5;
				AudioManager.Instance.PlayVoiceOver(applyBaitTutorials[currentTutorialIndex]);
				return true;
			} else {
				GameManager.Instance.LevelController.SetState(LevelController.State.AttatchBait);
				return true;
			}
		} else {
			return false;
		}
	}

	private bool AttatchBaitTutorialCompleteExtras(int currentTutorialIndex) {
		if (currentTutorialIndex == 3 && GameManager.Instance.TotalCaughtFish < 7) {
			return true;
		} else {
			return false;
		}
	}
}
