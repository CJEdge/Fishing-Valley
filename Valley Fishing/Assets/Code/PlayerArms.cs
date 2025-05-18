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

	#endregion


	#region Mono Behaviours

	public void Start() {
		GameManager.Instance.InputController.OnClick += Click;
		GameManager.Instance.InputController.OnReelStateChanged += Reel;
	}

	public void OnDestroy() {
		GameManager.Instance.InputController.OnClick -= Click;
		GameManager.Instance.InputController.OnReelStateChanged -= Reel;
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
				StartCoroutine(StartThrowRod());
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
		AudioManager.Instance.SetReelRate(GameManager.Instance.InputController.ReelSpeed);
		switch (GameManager.Instance.InputController.reelState) {
			case InputController.ReelState.reelingLocked:
				Debug.Log("locked");
				animator.Play(Idle);
				break;
			case InputController.ReelState.notReeling:
				animator.Play(IdleReel);
				break;
			case InputController.ReelState.calmReeling:
				animator.Play(SlowReel);
				break;
			case InputController.ReelState.normalReeling:
				animator.Play(MediumReel);
				break;
			case InputController.ReelState.fastReeling:
				Debug.Log("fast");
				animator.Play(FastReel);
				break;
			default:
				break;
		}
	}

	private IEnumerator StartThrowRod() {
		animator.Play(Throw);
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.ThrowRod,transform.position);
		yield return new WaitForSeconds(windUpLength);
		AudioManager.Instance.SkipVoiceOver();
		animator.Play(Idle);
		yield return new WaitForSeconds(throwWait);
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.LandRod, transform.position);
		GameManager.Instance.LevelController.SetState(LevelController.State.WaitingForBite);
		AudioManager.Instance.PlayReelSound(FMODManager.Instance.ReelSound);
	}

}
