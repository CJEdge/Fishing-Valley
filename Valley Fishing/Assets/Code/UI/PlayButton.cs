using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
	#region Public Methods

	public void LoadGameLevel() {
		SceneManager.LoadScene("Game");
	}

	#endregion
}
