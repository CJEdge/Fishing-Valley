using FMODUnity;
using FMOD.Studio;
using UnityEngine;

public class VoiceOverController : MonoBehaviour
{
	#region Properties

	private EventInstance CurrentTutorialEventInstance {
		get;
		set;
	}

	private bool[] AttatchBaitTutorialsCompleted {
		get;
		set;
	}

	[field: SerializeField]
	private bool[] CastRodTutorialsCompleted {
		get;
		set;
	}

	[field: SerializeField]
	private bool[] ReelTutorialsCompleted {
		get;
		set;
	}

	[field: SerializeField]
	private bool[] CaughtFishTutorialsCompleted {
		get;
		set;
	}

	private LevelController LevelController {
		get {
			return GameManager.Instance.LevelController;
		}
	}

	#endregion


	#region Mono Behaviours

	public void Start() {
		this.LevelController.StateChanged += LevelStateChanged;
		AudioManager.Instance.VoiceLineOver += VoiceOverFinished;
		GameManager.Instance.InputController.OnClick += Click;
		this.AttatchBaitTutorialsCompleted = new bool[FMODManager.Instance.ApplyBaitTutorials.Length];
		this.CastRodTutorialsCompleted = new bool[FMODManager.Instance.CastRodTutorials.Length];
		this.ReelTutorialsCompleted = new bool[FMODManager.Instance.ReelTutorials.Length];
		this.CaughtFishTutorialsCompleted = new bool[FMODManager.Instance.TutorialCatchVoices.Length];
	}

	public void Click() {
		switch (this.LevelController.CurrentState) {
			case LevelController.State.Default:
				break;
			case LevelController.State.Cutscene:
				break;
			case LevelController.State.Idle:
				break;
			case LevelController.State.AttatchBait:
				break;
			case LevelController.State.IdleWithBait:
				break;
			case LevelController.State.WaitingForBite:
				break;
			case LevelController.State.ReelingFish:
				break;
			case LevelController.State.FishCaught:
				break;
			default:
				break;
		}
	}

	#endregion


	#region Public Methods

	public void LevelStateChanged() {
		switch (this.LevelController.CurrentState) {
			case LevelController.State.Default:
				break;
			case LevelController.State.Cutscene:
				break;
			case LevelController.State.Idle:
				bool allTutorialsCompleted = true;
				for (int i = 0; i < this.AttatchBaitTutorialsCompleted.Length; i++) {
					if (!this.AttatchBaitTutorialsCompleted[i]) {
						allTutorialsCompleted = false;
					}
				}
				if (allTutorialsCompleted) {
					this.LevelController.SetState(LevelController.State.AttatchBait);
					return;
				}
				for (int i = 0; i < this.AttatchBaitTutorialsCompleted.Length; i++) {
					if (!this.AttatchBaitTutorialsCompleted[i]) {
						if (i == 3) {
							GameManager.Instance.CurrentBaits[i] = 6;
						} else {
							GameManager.Instance.CurrentBaits[i] = 1;
						}
						AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.ApplyBaitTutorials[i]);
						break;
					}
				}
				this.CurrentTutorialEventInstance = AudioManager.Instance.VoiceLineEventInstance;
				break;
			case LevelController.State.AttatchBait:
				for (int i = 0; i < this.AttatchBaitTutorialsCompleted.Length; i++) {
					if (!this.AttatchBaitTutorialsCompleted[i]) {
						this.AttatchBaitTutorialsCompleted[i] = true;
						break;
					}
				}
				break;
			case LevelController.State.IdleWithBait:
				allTutorialsCompleted = true;
				for (int i = 0; i < this.CastRodTutorialsCompleted.Length; i++) {
					if (!this.CastRodTutorialsCompleted[i]) {
						allTutorialsCompleted = false;
					}
				}
				if (allTutorialsCompleted) {
					return;
				}
				for (int i = 0; i < this.CastRodTutorialsCompleted.Length; i++) {
					if (!this.CastRodTutorialsCompleted[i]) {
						AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.CastRodTutorials[i]);
						break;
					}
				}
				this.CurrentTutorialEventInstance = AudioManager.Instance.VoiceLineEventInstance;
				break;
			case LevelController.State.WaitingForBite:
				for (int i = 0; i < this.CastRodTutorialsCompleted.Length; i++) {
					if (!this.CastRodTutorialsCompleted[i]) {
						this.CastRodTutorialsCompleted[i] = true;
						break;
					}
				}
				break;
			case LevelController.State.ReelingFish:
				allTutorialsCompleted = true;
				for (int i = 0; i < this.ReelTutorialsCompleted.Length; i++) {
					if (!this.ReelTutorialsCompleted[i]) {
						allTutorialsCompleted = false;
					}
				}
				if (allTutorialsCompleted) {
					return;
				}
				for (int i = 0; i < this.ReelTutorialsCompleted.Length; i++) {
					if (!this.ReelTutorialsCompleted[i]) {
						AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.ReelTutorials[i]);
						break;
					}
				}
				this.CurrentTutorialEventInstance = AudioManager.Instance.VoiceLineEventInstance;

				for (int i = 0; i < this.ReelTutorialsCompleted.Length; i++) {
					if (!this.ReelTutorialsCompleted[i]) {
						this.ReelTutorialsCompleted[i] = true;
						break;
					}
				}
				break;
			case LevelController.State.FishCaught:
				allTutorialsCompleted = true;
				for (int i = 0; i < this.CaughtFishTutorialsCompleted.Length; i++) {
					if (!this.CaughtFishTutorialsCompleted[i]) {
						allTutorialsCompleted = false;
					}
				}
				if (allTutorialsCompleted) {
					AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.CatchVoices[GameManager.Instance.CurrentFish.FishIndex]);
					this.CurrentTutorialEventInstance = AudioManager.Instance.VoiceLineEventInstance;
					return;
				}
				for (int i = 0; i < this.CaughtFishTutorialsCompleted.Length; i++) {
					if (!this.CaughtFishTutorialsCompleted[i]) {
						AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.TutorialCatchVoices[i]);
						break;
					}
				}
				this.CurrentTutorialEventInstance = AudioManager.Instance.VoiceLineEventInstance;
				for (int i = 0; i < this.CaughtFishTutorialsCompleted.Length; i++) {
					if (!this.CaughtFishTutorialsCompleted[i]) {
						this.CaughtFishTutorialsCompleted[i] = true;
						break;
					}
				}
				break;
			default:
				break;
		}
	}

	#endregion


	#region Private Methods

	private void VoiceOverFinished(EventInstance finishedEvent) {
		switch (this.LevelController.CurrentState) {
			case LevelController.State.Cutscene:
				LevelController.SetState(LevelController.State.Idle);
				break;
			case LevelController.State.Idle:
				LevelController.SetState(LevelController.State.AttatchBait);
				break;
			case LevelController.State.AttatchBait:
				break;
			case LevelController.State.WaitingForBite:
				break;
			case LevelController.State.ReelingFish:
				break;
			case LevelController.State.FishCaught:
				break;
			default:
				break;
		}
	}

	#endregion
}
