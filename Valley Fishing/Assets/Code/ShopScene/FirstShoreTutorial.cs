using UnityEngine;

public class FirstShoreTutorial : Shore
{
	public override void Initialize() {
		if (!this.AllShopsFinished) {
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.ShoreIntros[0]);
		} else {
			AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.LeaveShorePrompts[0]);
		}
		base.Initialize();
	}
}
