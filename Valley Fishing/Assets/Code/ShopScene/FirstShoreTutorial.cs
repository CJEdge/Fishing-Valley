using FMOD.Studio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstShoreTutorial : Shore
{
	public override void Initialize() {
		base.Initialize();
		if (!this.AllShopsFinished) {
			for (int i = 0; i < shopButtons.Length; i++) {
				if (i != 0) {
					shopButtons[shopButtons.Length - 1].SetActive(false);
				}
			}
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.ShoreIntros[0]);
		} else {
			Debug.Log("init");
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.LeaveShorePrompts[0]);
			for (int i = 0; i < shopButtons.Length; i++) {
				if (i == 0 || i == shopButtons.Length - 1) {
					shopButtons[shopButtons.Length - 1].SetActive(true);
				}
			}
		}
	}

	public override void VoiceOverSkipped(EventInstance eventInstance) {
		base.VoiceOverSkipped(eventInstance);
		if(this.TimesSkipped == 0) { 
			if (this.AllShopsFinished) {
				SceneManager.LoadScene(LevelManager.CatchTutorial_01);
			}
		} else {
			if (!this.AllShopsFinished) {
				eventSystem.SetSelectedGameObject(shopButtons[this.CurrentButtonIndex]);
			} else {
				eventSystem.SetSelectedGameObject(shopButtons[shopButtons.Length - 1]);
			}
		}
	}
}
