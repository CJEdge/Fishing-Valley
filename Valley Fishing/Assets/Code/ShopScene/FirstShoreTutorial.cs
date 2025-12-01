using FMODUnity;
using UnityEngine;

public class FirstShoreTutorial : Shore {
	public override void Initialize() {
		base.Initialize();
		AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.ShoreIntros[0]);
	}

	public override void VoiceLineOver(EventReference eventReference, bool skipped) {
		EnterBaitShop();
	}
}
