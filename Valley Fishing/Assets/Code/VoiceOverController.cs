using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoiceOverController : MonoBehaviour
{

	#region Serialized Fields

	[SerializeField]
	protected EventReference[] applyBaitTutorials;

	[SerializeField]
	protected EventReference[] castRodTutorials;

	[SerializeField]
	protected EventReference[] reelTutorials;

	[SerializeField]
	protected EventReference[] tutorialCatchVoices;

	[SerializeField]
	protected int levelIndex;

	#endregion


	#region Protected Variables

	protected EventInstance CurrentTutorialEventInstance;

	protected bool[] AttatchBaitTutorialsCompleted;

	protected bool[] CastRodTutorialsCompleted;

	protected bool[] ReelTutorialsCompleted;

	protected bool[] CaughtFishTutorialsCompleted;

	#endregion


	#region Properties

	protected LevelController LevelController {
		get {
			return GameManager.Instance.LevelController;
		}
	}

	#endregion


	#region Mono Behaviours

	public void Start() {
		this.LevelController.StateChanged += LevelStateChanged;
		AudioManager.Instance.VoiceLineOver += VoiceOverFinished;
		this.AttatchBaitTutorialsCompleted = new bool[applyBaitTutorials.Length];
		this.CastRodTutorialsCompleted = new bool[castRodTutorials.Length];
		this.ReelTutorialsCompleted = new bool[reelTutorials.Length];
		this.CaughtFishTutorialsCompleted = new bool[tutorialCatchVoices.Length];
		AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.IntroCutscenes[levelIndex]);
	}

	public void OnDestroy() {
		if(this.LevelController == null) {
			return;
		}
		this.LevelController.StateChanged -= LevelStateChanged;
		AudioManager.Instance.VoiceLineOver -= VoiceOverFinished;
	}

	#endregion


	#region Public Methods

	public void LevelStateChanged() {
		PerformStateSwitch();
	}

	public virtual bool PerformStateSwitch() {
		switch (this.LevelController.CurrentState) {
			case LevelController.State.Idle:
				if (AllTutorialsCompleted(this.AttatchBaitTutorialsCompleted)) {
					this.LevelController.SetState(LevelController.State.AttatchBait);
					return false;
				}
				return true;
			case LevelController.State.AttatchBait:
				return true;
			case LevelController.State.IdleWithBait:
				if (AllTutorialsCompleted(this.CastRodTutorialsCompleted)) {
					return false;
				}
				if(PlayNextTutotialVoiceOver(this.CastRodTutorialsCompleted, castRodTutorials)) {
					return false;
				}
				return true;
			case LevelController.State.WaitingForBite:
				IncrementTutorial(this.CastRodTutorialsCompleted);
				break;
			case LevelController.State.ReelingFish:
				if (AllTutorialsCompleted(this.ReelTutorialsCompleted)) {
					return false;
				}
				if (!GameManager.Instance.CurrentFish.IsTutorial) {
					return false;
				}
				PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
				IncrementTutorial(this.ReelTutorialsCompleted);
				return true;
			case LevelController.State.FishCaught:
				return true;
			default:
				return true;
		}
		return true;
	}

	#endregion


	#region Private Methods

	public bool AllTutorialsCompleted(bool[] tutorials) {
		bool allTutorialsComplete = true;
		for (int i = 0; i < tutorials.Length; i++) {
			if (!tutorials[i]) {
				allTutorialsComplete = false;
			}
		}
		return allTutorialsComplete;
	}

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

	public virtual void VoiceOverFinished(EventInstance finishedEvent) {
		switch (this.LevelController.CurrentState) {
			case LevelController.State.Cutscene:
				LevelController.SetState(LevelController.State.Idle);
				break;
			case LevelController.State.Idle:
				LevelController.SetState(LevelController.State.AttatchBait);
				break;
			default:
				break;
		}
	}

	#endregion

}
