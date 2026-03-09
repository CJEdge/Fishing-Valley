using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
	#region Serialized Fields

	[SerializeField]
	private GameObject pauseMenuObject;

	#endregion


	#region Mono Behaviours

	public void Start() {
		GameManager.Instance.InputController.OnPause -= PauseGame;
		GameManager.Instance.InputController.OnPause += PauseGame;
		pauseMenuObject.SetActive(false);
	}

	public void OnDestroy() {
		if(GameManager.Instance == null) {
			return;
		}
		GameManager.Instance.InputController.OnPause -= PauseGame;
	}

	#endregion


	#region Public Methods

	public void PauseGame() {
		pauseMenuObject.SetActive(!pauseMenuObject.activeSelf);
	}

	public void ReturnToMenu() {
		SceneManager.LoadScene("Menu");
	}

	#endregion
}
