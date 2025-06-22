using TMPro;
using UnityEngine;

public class FishView : MonoBehaviour
{
	#region Serialized Fields

	[SerializeField]
	private GameObject[] fishUis;

	[SerializeField]
	private TMP_Text fishText;

	[SerializeField]
	private string[] fishCaughtTexts;

	#endregion


	#region Mono Behaviours

	public void Start() {
		GameManager.Instance.InputController.OnClick -= DisableFishUI;
		GameManager.Instance.InputController.OnSkip -= DisableFishUI;
		GameManager.Instance.InputController.OnClick += DisableFishUI;
		GameManager.Instance.InputController.OnSkip += DisableFishUI;
	}

	public void OnDestroy() {
		if(GameManager.Instance == null) {
			return;
		}
		GameManager.Instance.InputController.OnClick -= DisableFishUI;
		GameManager.Instance.InputController.OnSkip -= DisableFishUI;
	}

	#endregion


	#region Public Methods

	public void EnableFishUI(bool enable) {
		if (enable) {
			for (int i = 0; i < GameManager.Instance.Fish.Count; i++) {
				if (GameManager.Instance.Fish[i].FishName == GameManager.Instance.CurrentFish.FishName) {
					fishUis[i].SetActive(enable);
				}
			}
			int randomCaughtTextIndex = Random.Range(0, fishCaughtTexts.Length);
			fishText.text = fishCaughtTexts[randomCaughtTextIndex] + " " + GameManager.Instance.CurrentFish.FishName + "!";
			fishText.gameObject.SetActive(enable);
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.FishCatch);
		}
		else {
			for (int i = 0; i < GameManager.Instance.Fish.Count; i++) {
				fishUis[i].SetActive(enable);
			}
		fishText.gameObject.SetActive(enable);
		}
	}

	#endregion


	#region Private Methods

	public void DisableFishUI() {
		bool allreadyDisabled = true;
		for (int i = 0; i < GameManager.Instance.Fish.Count; i++) {
			if (fishUis[i].activeSelf) {
				allreadyDisabled = false;
			}
		}
		if (allreadyDisabled) {
			return;
		}
		EnableFishUI(false);
		if(GameManager.Instance.LevelController.CurrentState == LevelController.State.FishCaught) {
			GameManager.Instance.LevelController.SetState(LevelController.State.Idle);
		}
	}

	#endregion

}
