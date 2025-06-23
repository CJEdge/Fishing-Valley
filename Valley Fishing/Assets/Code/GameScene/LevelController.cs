using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelController : AbstractState<LevelController.State> {

	#region Serialized Fields

	[SerializeField]
	private VoiceOverController voiceOverController;

	[SerializeField]
	private Transform gameplayContainer;

	[SerializeField]
	private Transform fishSpawnTransform;

	[SerializeField]
	private FishView fishView;

	[SerializeField]
	private BaitView baitView;

	[SerializeField]
	private Transform leftStrafeTransform;

	[SerializeField]
	private Transform rightStrafeTransform;

	[SerializeField]
	private EventSystem eventSystem;

	#endregion


	#region Properties

	public VoiceOverController VoiceOverController {
		get {
			return voiceOverController;
		}
	}

	public List<Fish> Fish {
		get {
			return GameManager.Instance.Fish;
		}
	}

	public Bait CurrentBait {
		get {
			return GameManager.Instance.CurrentBait;
		}
	}

	public FishView FishView {
		get {
			return fishView;
		}
	}

	public Transform LeftStrafeTransform {
		get {
			return leftStrafeTransform;
		}
	}

	public Transform RightStrafeTransform {
		get {
			return rightStrafeTransform;
		}
	}

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
				GameManager.Instance.EventSystem = eventSystem;
				SetState(State.Cutscene);
				break;
			case State.Cutscene:
				break;
			case State.Idle:
				break;
			case State.AttatchBait:
				baitView.EnableBaitUI(true);
				break;
			case State.IdleWithBait:
				break;
			case State.WaitingForBite:
				SpawnFish();
				break;
			case State.ReelingFish:
				GameManager.Instance.InputController.SetState(InputController.State.NotReeling);
				break;
			case State.FishCaught:
				GameManager.Instance.InputController.SetState(InputController.State.ReelingLocked);
				fishView.EnableFishUI(true);
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
		SetState(State.ReelingFish);
		GameManager.Instance.CurrentFish.Initialize();
	}

	private void SpawnFish() {
		int fishIndex = 0;
		float randomValue = UnityEngine.Random.value;
		float cumulative = 0f;
		for (int i = 0; i < this.CurrentBait.CatchChances.Length; i++) {
			cumulative += this.CurrentBait.CatchChances[i];
			if (randomValue < cumulative) {
				fishIndex = i;
				break;
			}
		}
		Fish fishInstance = Instantiate(this.Fish[fishIndex], fishSpawnTransform.position, Quaternion.identity);
		fishInstance.name = this.Fish[fishIndex].name;
		fishInstance.transform.parent = gameplayContainer;
		fishInstance.IsFailable = this.CurrentBait.Isfailable;
		fishInstance.IsTutorial = this.CurrentBait.IsTutorial;
		GameManager.Instance.CurrentFish = fishInstance;
		StartCoroutine(WaitForBite());
	}

	#endregion
}
