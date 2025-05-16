using FMODUnity;
using FMOD.Studio;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
	#region Properties

	private EventInstance CurrentTutorialEventInstance {
		get;
		set;
	}

	[field:SerializeField]
	private bool AttatchBaitTutorialCompleted {
		get;
		set;
	}

	[field: SerializeField]
	private bool CastRodTutorialCompleted {
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
	}

	public void Click() {
		switch (this.LevelController.CurrentState) {
			case LevelController.State.Default:
				break;
			case LevelController.State.Cutscene:
				break;
			case LevelController.State.Idle:
				this.AttatchBaitTutorialCompleted = true;
				break;
			case LevelController.State.AttatchBait:
				break;
			case LevelController.State.IdleWithBait:
				this.CastRodTutorialCompleted = true;
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
				if (this.AttatchBaitTutorialCompleted) {
					return;
				}
				AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.ApplyBaitTutorial);
				this.CurrentTutorialEventInstance = AudioManager.Instance.VoiceLineEventInstance;
				break;
			case LevelController.State.AttatchBait:
				break;
			case LevelController.State.IdleWithBait:
				if (this.CastRodTutorialCompleted) {
					return;
				}
				AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.CastRodTutorial);
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
