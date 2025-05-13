using FMOD.Studio;
using System.Collections;
using UnityEngine;

public class PlayerArms : MonoBehaviour
{
	#region Serialized Fields

	[SerializeField]
	private GameObject[] arms;

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
				this.LevelController.SetState(LevelController.State.IdleWithBait);
				break;
			case LevelController.State.IdleWithBait:
				TurnOffAllArms();
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



	public void BeginReel() {
		TurnOffAllArms();
		arms[2].SetActive(true);
	}

	public void Reel() {
		AudioManager.Instance.SetReelRate(GameManager.Instance.InputController.ReelSpeed);
	}

	public void ResetArms() {
		TurnOffAllArms();
		arms[0].SetActive(true);
	}

	private void TurnOffAllArms() {
		for (int i = 0; i < arms.Length; i++) {
			arms[i].SetActive(false);
		}
	}

	public void Update() {
		Reel();
	}

	private IEnumerator StartThrowRod() {
		TurnOffAllArms();
		arms[1].SetActive(true);
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.ThrowRod,transform.position);
		yield return new WaitForSeconds(windUpLength);
		TurnOffAllArms();
		arms[2].SetActive(true);
		yield return new WaitForSeconds(throwWait);
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.LandRod, transform.position);
		GameManager.Instance.LevelController.SetState(LevelController.State.WaitingForBite);
		AudioManager.Instance.PlayReelSound(FMODManager.Instance.ReelSound);
	}

}
