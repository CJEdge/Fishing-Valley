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
		GameManager.Instance.InputController.OnClick -= DisableFishUI;
		GameManager.Instance.InputController.OnSkip -= DisableFishUI;
	}

	#endregion


	#region Public Methods

	public void EnableFishUI(bool enable) {
		for (int i = 0; i < GameManager.Instance.Fish.Count; i++) {
			if (GameManager.Instance.Fish[i].FishName == GameManager.Instance.CurrentFishName) {
				fishUis[i].SetActive(enable);
			}

		}
		if (enable) {
			int randomCaughtTextIndex = Random.Range(0, fishCaughtTexts.Length);
			fishText.text = fishCaughtTexts[randomCaughtTextIndex] + " " + GameManager.Instance.CurrentFishName + "!";
		} 
		fishText.gameObject.SetActive(enable);
	}

	#endregion


	#region Private Methods

	private void DisableFishUI() {
		EnableFishUI(false);
		if(GameManager.Instance.LevelController.CurrentState == LevelController.State.FishCaught) {
			GameManager.Instance.LevelController.SetState(LevelController.State.AttatchBait);
		}
	}

	#endregion




}
