using System.Collections;
using UnityEngine;

public class PlayerArms : MonoBehaviour
{
	[SerializeField]
	private GameObject[]arms;

	[SerializeField]
	private InputController inputController;

	[SerializeField]
	private GameObject throwWindUpSfx;

	[SerializeField]
	private float windUpLength;

	[SerializeField]
	private GameObject throwSfx;

	[SerializeField]
	private float throwWait;

	[SerializeField]
	private GameObject landSfx;

	public void ThrowRod() {
		TurnOffAllArms();
		StartCoroutine(StartThrowRod());

	}

	public void BeginReel() {
		TurnOffAllArms();
		arms[2].SetActive(true);
	}

	public void Reel() {
		if (inputController.CurrentReelAudio == null) {
			return;
		}
		if (!inputController.CurrentReelAudio.CurrentAudioSource.isPlaying) {
			if (arms[2].activeSelf) {
				TurnOffAllArms();
				arms[3].SetActive(true);
			} else {
				TurnOffAllArms();
				arms[2].SetActive(true);
			}
		}
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
		throwWindUpSfx.SetActive(false);
		throwWindUpSfx.SetActive(false);
		yield return new WaitForSeconds(windUpLength);
		throwSfx.SetActive(false);
		throwSfx.SetActive(true);
		TurnOffAllArms();
		arms[2].SetActive(true);
		yield return new WaitForSeconds(throwWait);
		landSfx.SetActive(false);
		landSfx.SetActive(true);
		//GameManager.Instance.FishController.LandRod();
	}

}
