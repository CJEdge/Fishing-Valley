using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecondShoreTutorial : Shore {

	public override void Initialize() {
		base.Initialize();
		if (this.AllShopsFinished) {
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.LeaveShorePrompts[0]);
		} else {
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.ShoreIntros[1]);
		}
	}

	public override void VoiceLineOver(EventReference eventReference, bool skipped) {
		if (!AllShopsFinished) {
			EnterBaitShop();
		} else {
			SceneManager.LoadScene(LevelManager.CatchTutorial_02);
		}
	}
}
