using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayButton : ButtonVoiceOverComponent {
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

	public override bool ButtonClicked(bool buttonInteractable) {
		if (!buttonInteractable) {
			return false;
		}
		switch (LevelToLoad) {
			case Level.Menu:
				SceneManager.LoadScene(LevelManager.Menu);
				break;
			case Level.CatchTutorial_01:
				SceneManager.LoadScene(LevelManager.CatchTutorial_00);
				break;
			case Level.CatchTutorial_02:
				Debug.Log("here");
				SceneManager.LoadScene(LevelManager.CatchTutorial_01);
				break;
			case Level.CatchTutorial_03:
				SceneManager.LoadScene(LevelManager.CatchTutorial_02);
				break;
			case Level.CatchTutorial_04:
				SceneManager.LoadScene(LevelManager.CatchTutorial_03);
				break;
			case Level.ShopTutorial_01:
				SceneManager.LoadScene(LevelManager.ShopTutorial_00);
				break;
			case Level.ShopTutorial_02:
				SceneManager.LoadScene(LevelManager.ShopTutorial_01);
				break;
			default:
				break;
		}
		return false;
	}

	#endregion
}
