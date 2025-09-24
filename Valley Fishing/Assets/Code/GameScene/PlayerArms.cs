using FMOD.Studio;
using System.Collections;
using UnityEngine;

public class PlayerArms : MonoBehaviour
{
	#region Constants

	private const string Idle = "Idle";
	private const string Throw = "Throw";
	private const string IdleReel = "IdleReel";
	private const string SlowReel = "SlowReel";
	private const string MediumReel = "MediumReel";
	private const string FastReel = "FastReel";

	#endregion


	#region Serialized Fields

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private float windUpLength;

	[SerializeField]
	private float throwWait;

	#endregion


	#region Properties

	private LevelController LevelController {
		get {
			return GameManager.Instance.LevelController;
		}
	}

	private bool IsCasting {
		get;
		set;
	}

	#endregion


	#region Mono Behaviours

	public void Start() {
		GameManager.Instance.InputController.OnClick += Click;
		GameManager.Instance.InputController.OnStateChanged += Reel;
	}

	public void OnDestroy() {
		if(GameManager.Instance == null) {
			return;
		}
		GameManager.Instance.InputController.OnClick -= Click;
		GameManager.Instance.InputController.OnStateChanged -= Reel;
	}

	#endregion


	#region Private Methods

	private void Click() {
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
				if (!this.IsCasting) {
					StartCoroutine(StartThrowRod());
				}
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

	public void Reel() {
		AudioManager.Instance.SetReelRate(GameManager.Instance.InputController.ReelLevel);
		switch (GameManager.Instance.InputController.CurrentState) {
			case InputController.State.ReelingLocked:
				animator.Play(Idle);
				break;
			case InputController.State.NotReeling:
				animator.Play(IdleReel);
				break;
			case InputController.State.CalmReeling:
				animator.Play(SlowReel);
				break;
			case InputController.State.NormalReeling:
				animator.Play(MediumReel);
				break;
			case InputController.State.FastReeling:
				animator.Play(FastReel);
				break;
			default:
				break;
		}
	}

	private IEnumerator StartThrowRod() {
		this.IsCasting = true;
		animator.Play(Throw);
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.ThrowRod,transform.position);
		yield return new WaitForSeconds(windUpLength);
		AudioManager.Instance.SkipVoiceOver();
		animator.Play(Idle);
		yield return new WaitForSeconds(throwWait);
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.LandRod, transform.position);
		GameManager.Instance.LevelController.SetState(LevelController.State.WaitingForBite);
		AudioManager.Instance.PlayReelSound(FMODManager.Instance.ReelSound);
		this.IsCasting = false;
	}

}
