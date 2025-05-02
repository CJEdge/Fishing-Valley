using System;
using System.Collections;
using UnityEngine;

public class LevelController : AbstractState<LevelController.State> {

	#region Serialized Fields

	[SerializeField]
	private TutorialController tutorialController;

	#endregion


	#region Abstract State

	public enum State {
		Default,
		Cutscene,
		Idle,
		AttatchBait,
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
				StartCoroutine(WaitAndEndCutscene(10));
				break;
			case State.Idle:
				tutorialController.LevelStateChanged();
				break;
			case State.AttatchBait:
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


	#region Public Methods

	public void Skip() {
		switch (this.CurrentState) {
			case State.Default:
				break;
			case State.Cutscene:
				StartCoroutine(WaitAndEndCutscene(0));
				break;
			case State.Idle:
				break;
			case State.AttatchBait:
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

	private IEnumerator WaitAndEndCutscene(int waitTime) {
		yield return new WaitForSeconds(waitTime);
		AudioManager.Instance.SkipVoiceOver();
		if (this.CurrentState == State.Cutscene) {
			SetState(State.Idle);
		}
	}

	#endregion
}
