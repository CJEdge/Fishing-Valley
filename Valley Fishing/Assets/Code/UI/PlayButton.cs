using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
	#region Levels

	public enum Level {
		Menu,
		CatchTutorial_01,
		CatchTutorial_02,
		CatchTutorial_03,
		CatchTutorial_04,
		ShopTutorial_01,
		ShopTutorial_02,
		ShopTutorial_03,
	}

	#endregion


	#region Properties

	[field:SerializeField]
	public Level LevelToLoad {
		get;
		set;
	}

	#endregion


	#region Public Methods

	public void LoadGameLevel() {
		switch (LevelToLoad) {
			case Level.Menu:
				SceneManager.LoadScene(LevelManager.Menu);
				break;
			case Level.CatchTutorial_01:
				SceneManager.LoadScene(LevelManager.CatchTutorial_00);
				break;
			case Level.CatchTutorial_02:
				SceneManager.LoadScene(LevelManager.CatchTutorial_01);
				break;
			case Level.CatchTutorial_03:
				SceneManager.LoadScene(LevelManager.CatchTutorial_02);
				break;
			case Level.CatchTutorial_04:
				SceneManager.LoadScene(LevelManager.CatchTutorial_03);
				break;
			case Level.ShopTutorial_01:
				SceneManager.LoadScene(LevelManager.ShopTutorial_01);
				break;
			case Level.ShopTutorial_02:
				SceneManager.LoadScene(LevelManager.ShopTutorial_02);
				break;
			default:
				break;
		}
	}

	#endregion
}
