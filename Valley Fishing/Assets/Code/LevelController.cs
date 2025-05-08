using System;
using System.Collections;
using UnityEngine;

public class LevelController : AbstractState<LevelController.State> {

	#region Serialized Fields

	[SerializeField]
	private TutorialController tutorialController;

	[SerializeField]
	private Transform gameplayContainer;

	[SerializeField]
	private Transform fishSpawnTransform;

	#endregion


	#region Abstract State

	public enum State {
		Default,
		Cutscene,
		Idle,
		AttatchBait,
		IdleWithBait,
		WaitingForBite,
		ReelingFish,
		FishCaught
	}

	protected override void EnterState(State state) {
		switch (state) {
			case State.Default:
				GameManager.Instance.LevelController = this;
				SetState(State.Cutscene);
				break;
			case State.Cutscene:
				AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.Level1IntroCutscene);
				break;
			case State.Idle:
				tutorialController.LevelStateChanged();
				break;
			case State.AttatchBait:
				break;
			case State.IdleWithBait:
				break;
			case State.WaitingForBite:
				StartCoroutine(WaitForBite());
				break;
			case State.ReelingFish:
				break;
			case State.FishCaught:
				break;
		}
	}

	#endregion


	#region Public Methods

	public void Skip() {
		switch (this.CurrentState) {
			case State.Default:
				break;
			case State.Cutscene:
				break;
			case State.Idle:
				break;
			case State.AttatchBait:
				break;
			case State.IdleWithBait:
				break;
			case State.WaitingForBite:
				break;
			case State.ReelingFish:
				break;
			case State.FishCaught:
				break;
		}
	}

	#endregion


	#region Private Methods

	private IEnumerator WaitForBite() {
		yield return new WaitForSeconds(3);
		Fish fishInstance = Instantiate(GameManager.Instance.Fish[0], fishSpawnTransform.position, Quaternion.identity);
		fishInstance.transform.parent = gameplayContainer;
		SetState(State.ReelingFish);
	}

	#endregion
}
