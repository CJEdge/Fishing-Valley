using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatManager : Singleton<CheatManager>
{
	[System.Serializable]
	public struct BaitCheatData {
		public int BaitIndex;
		public int BaitAmount;
	}

	[System.Serializable]
	public struct CaughtFishCheatData {
		public int FishIndex;
		public int FishAmount;
	}

	private int ScenesLoaded;

	#region Mono Behaviours

	public void Start() {
		SceneManager.sceneLoaded -= SceneLoaded;
		SceneManager.sceneLoaded += SceneLoaded;
		if (ScenesLoaded > 0) {
			return;
		}
		ShowFirstShopTutorialCheats();
		ShowSecondCatchTutorialCheats();
		ShowThirdCatchTutorialCheats();
	}

	public void OnDestroy() {
		SceneManager.sceneLoaded -= SceneLoaded;
	}

	private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
		ScenesLoaded++;
	}

	#endregion


	#region Second Catch Turotial

	[System.Serializable]
	public class SecondCatchTutorialCheats {
		public BaitCheatData[] Baits;
	}

	[SerializeField]
	private SecondCatchTutorialCheats secondCatchTutorialCheats;

	private void ShowSecondCatchTutorialCheats() {
		if (SceneManager.GetActiveScene().name == LevelManager.CatchTutorial_01) {
			for (int i = 0; i < secondCatchTutorialCheats.Baits.Length; i++) {
				GameManager.Instance.CurrentBaits[secondCatchTutorialCheats.Baits[i].BaitIndex] = secondCatchTutorialCheats.Baits[i].BaitAmount;
			}
		}
	}

	#endregion


	#region Third Catch Turotial

	[System.Serializable]
	public class ThirdCatchTutorialCheats {
		public BaitCheatData[] Baits;
	}

	[SerializeField]
	private ThirdCatchTutorialCheats thirdCatchTutorialCheats;

	private void ShowThirdCatchTutorialCheats() {
		if (SceneManager.GetActiveScene().name == LevelManager.CatchTutorial_02) {
			for (int i = 0; i < thirdCatchTutorialCheats.Baits.Length; i++) {
				GameManager.Instance.CurrentBaits[thirdCatchTutorialCheats.Baits[i].BaitIndex] = thirdCatchTutorialCheats.Baits[i].BaitAmount;
			}
		}
	}

	#endregion


	#region First Shop Turotial

	[System.Serializable]
	public class FirstShopTutorialCheats {
		public CaughtFishCheatData[] CaughtFish;
	}

	[SerializeField]
	private FirstShopTutorialCheats firstShopTutorialCheats;

	private void ShowFirstShopTutorialCheats() {
		if (SceneManager.GetActiveScene().name == LevelManager.ShopTutorial_01) {
			for (int i = 0; i < firstShopTutorialCheats.CaughtFish.Length; i++) {
				foreach (var fish in firstShopTutorialCheats.CaughtFish) {
					while (GameManager.Instance.CaughtFish.Count <= fish.FishIndex) {
						GameManager.Instance.CaughtFish.Add(0);
					}
					GameManager.Instance.CaughtFish[fish.FishIndex] = fish.FishAmount;
				}
			}
		}
	}

	#endregion

}
