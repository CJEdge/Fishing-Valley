using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public abstract class Shop : MonoBehaviour
{

	#region Protected Variables

	protected EventInstance CurrentTutorialEventInstance;

	#endregion


	#region Mono Behaviours

	public virtual void Awake()
	{

	}

	public virtual void Start() {
		GameManager.Instance.InputController.OnSkip += Skip;
		AudioManager.Instance.OnVoiceLineOver += VoiceLineOver;		
	}

	public virtual void OnDestroy() {
		AudioManager.Instance.OnVoiceLineOver -= VoiceLineOver;
		GameManager.Instance.InputController.OnSkip -= Skip;
	}

	#endregion


	#region Public Methods

	public abstract void VoiceLineOver(EventReference eventReference, bool skipped);

	public bool PlayNextTutotialVoiceOver(bool[] tutorialsCompleted, EventReference[] tutorialVoiceLines) {
		if(tutorialsCompleted == null) {
			return false;
		}
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
		if(tutorial == null) {
			return;
		}
		for (int i = 0; i < tutorial.Length; i++) {
			if (!tutorial[i]) {
				tutorial[i] = true;
				break;
			}
		}
	}

	public virtual void Skip() {

	}

	#endregion
}
