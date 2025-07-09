using FMOD.Studio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThirdShoreTutorial : Shore {
	public override void Initialize() {
		base.Initialize();
		if (!this.AllShopsFinished) {
			for (int i = 0; i < shopButtons.Length; i++) {
				if (i != 0) {
					shopButtons[shopButtons.Length - 1].SetActive(false);
				}
			}
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.ShoreIntros[2]);
		} else {
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.LeaveShorePrompts[2]);
			for (int i = 0; i < shopButtons.Length; i++) {
				if (i == 0 || i == shopButtons.Length - 1) {
					shopButtons[shopButtons.Length - 1].SetActive(true);
				}
			}
			shopButtons[0].SetActive(false);
		}
	}

	public override void VoiceOverSkipped(EventInstance eventInstance, bool skipped) {
		if (shoreMenuObject == null) {
			return;
		}
		if (!shoreMenuObject.activeInHierarchy) {
			return;
		}
		base.VoiceOverSkipped(eventInstance, skipped);
		if (this.TimesSkipped == 0) {
			if (this.AllShopsFinished) {
				SceneManager.LoadScene(LevelManager.CatchTutorial_03);
			}
		} else {
			if (!this.AllShopsFinished) {
				GameManager.Instance.InputController.SelectButton(shopButtons[this.CurrentButtonIndex]);
			} else {
				GameManager.Instance.InputController.SelectButton(shopButtons[shopButtons.Length - 1]);
			}
		}
	}
}
