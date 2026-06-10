using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
	#region Serialized Fields

	[SerializeField] private GameObject pauseMenuObject;
	[SerializeField] private GameObject initialButton;
	[SerializeField] private EventReference pauseLine;

	#endregion

	FMOD.Studio.Bus	gameplayBus;

	#region Mono Behaviours

	public void Start() {
		GameManager.Instance.InputController.OnPause -= PauseGame;
		GameManager.Instance.InputController.OnPause += PauseGame;
		pauseMenuObject.SetActive(false);
		gameplayBus = FMODUnity.RuntimeManager.GetBus("bus:/Gameplay");
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
		if (pauseMenuObject.activeInHierarchy) {
			AudioManager.Instance.PlayVoiceOver(pauseLine);
			GameManager.Instance.EventSystem.SetSelectedGameObject(initialButton);
			Time.timeScale = 0;
			gameplayBus.setPaused(true);
		} else {
			Time.timeScale = 1;
			gameplayBus.setPaused(false);
		}
	}

	public void ReturnToMenu() {
		SceneManager.LoadScene("Menu");
	}

	#endregion


	#region Private Methods

	private void OnApplicationFocus(bool hasFocus) {
		if (!hasFocus) {
			return;
		}
		if (pauseMenuObject.activeInHierarchy) {
			gameplayBus.setPaused(true);
		}
	}

	#endregion
}
