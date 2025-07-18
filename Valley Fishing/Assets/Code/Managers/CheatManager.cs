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
		ShowSecondShopTutorialCheats();
		ShowThirdShopTutorialCheats();
		ShowSecondCatchTutorialCheats();
		ShowThirdCatchTutorialCheats();
		ShowFourthCatchTutorialCheats();
		ShowFirstBossTutorialCheats();
	}

	public void OnDestroy() {
		SceneManager.sceneLoaded -= SceneLoaded;
	}

	private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
		ScenesLoaded++;
		if (GameManager.Instance.TotalBaitsLeft == 0) {
			ShowThirdCatchTutorialCheats();
		}
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


	#region Third Catch Turotial

	[System.Serializable]
	public class FourthCatchTutorialCheats {
		public BaitCheatData[] Baits;
	}

	[SerializeField]
	private FourthCatchTutorialCheats fourthCatchTutorialCheats;

	private void ShowFourthCatchTutorialCheats() {
		if (SceneManager.GetActiveScene().name == LevelManager.CatchTutorial_03) {
			for (int i = 0; i < fourthCatchTutorialCheats.Baits.Length; i++) {
				GameManager.Instance.CurrentBaits[fourthCatchTutorialCheats.Baits[i].BaitIndex] = fourthCatchTutorialCheats.Baits[i].BaitAmount;
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
		if (SceneManager.GetActiveScene().name == LevelManager.ShopTutorial_00) {
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


	#region  Second Shop Turotial

	[System.Serializable]
	public class SecondShopTutorialCheats {
		public CaughtFishCheatData[] CaughtFish;
	}

	[SerializeField]
	private SecondShopTutorialCheats secondShopTutorialCheats;

	private void ShowSecondShopTutorialCheats() {
		if (SceneManager.GetActiveScene().name == LevelManager.ShopTutorial_01) {
			for (int i = 0; i < secondShopTutorialCheats.CaughtFish.Length; i++) {
				foreach (var fish in secondShopTutorialCheats.CaughtFish) {
					while (GameManager.Instance.CaughtFish.Count <= fish.FishIndex) {
						GameManager.Instance.CaughtFish.Add(0);
					}
					GameManager.Instance.CaughtFish[fish.FishIndex] = fish.FishAmount;
				}
			}
		}
	}

	#endregion


	#region  Third Shop Turotial

	[System.Serializable]
	public class SThirdShopTutorialCheats {
		public CaughtFishCheatData[] CaughtFish;
	}

	[SerializeField]
	private SThirdShopTutorialCheats thirdShopTutorialCheats;

	private void ShowThirdShopTutorialCheats() {
		if (SceneManager.GetActiveScene().name == LevelManager.ShopTutorial_02) {
			for (int i = 0; i < thirdShopTutorialCheats.CaughtFish.Length; i++) {
				foreach (var fish in thirdShopTutorialCheats.CaughtFish) {
					while (GameManager.Instance.CaughtFish.Count <= fish.FishIndex) {
						GameManager.Instance.CaughtFish.Add(0);
					}
					GameManager.Instance.CaughtFish[fish.FishIndex] = fish.FishAmount;
				}
			}
		}
	}

	#endregion


	#region First Boss Turotial

	[System.Serializable]
	public class FirstBossTutorialCheats {
		public BaitCheatData[] Baits;
	}

	[SerializeField]
	private FirstBossTutorialCheats firstBossTutorialCheats;

	private void ShowFirstBossTutorialCheats() {
		if (SceneManager.GetActiveScene().name == LevelManager.BossTutorial_00) {
			for (int i = 0; i < firstBossTutorialCheats.Baits.Length; i++) {
				GameManager.Instance.CurrentBaits[firstBossTutorialCheats.Baits[i].BaitIndex] = firstBossTutorialCheats.Baits[i].BaitAmount;
			}
		}
	}

	#endregion

}
