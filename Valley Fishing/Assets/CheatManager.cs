using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatManager : Singleton<CheatManager>
{
	private int ScenesLoaded;

	public void Start()
    {
		SceneManager.sceneLoaded -= SceneLoaded;
		SceneManager.sceneLoaded += SceneLoaded;
		if(ScenesLoaded > 0) {
			return;
		}
		if(SceneManager.GetActiveScene().name == LevelManager.Instance.CatchTutorial_02) {
			GameManager.Instance.CurrentBaits[3] = 1;
			GameManager.Instance.CurrentBaits[5] = 1;
		}
	}

	public void OnDestroy() {
		SceneManager.sceneLoaded -= SceneLoaded;
	}

	private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
		ScenesLoaded++;
	}
}
