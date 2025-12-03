using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public abstract class Shop : AbstractState<Shop.State>
{
	#region States

	public enum State {
		Defualt,
		Entering,
		Trading,
		Leaving
	}

	#endregion


	#region Protected Variables

	protected EventInstance CurrentTutorialEventInstance;

	#endregion


	#region Public Methods

	public abstract void VoiceLineOver(EventReference eventReference, bool skipped);

	public bool PlayNextTutotialVoiceOver(bool[] tutorialsCompleted, EventReference[] tutorialVoiceLines) {
		for (int i = 0; i < tutorialsCompleted.Length; i++) {
			if (!tutorialsCompleted[i]) {
				AudioManager.Instance.PlayVoiceOver(tutorialVoiceLines[i]);
				this.CurrentTutorialEventInstance = AudioManager.Instance.VoiceLineEventInstance;
				return true;
			}
		}
		return false;
	}

	public void IncrementTutorial(bool[] tutorial) {
		for (int i = 0; i < tutorial.Length; i++) {
			if (!tutorial[i]) {
				tutorial[i] = true;
				break;
			}
		}
	}

	#endregion
}
