using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
	#region Serialized Fields

	[SerializeField]
	private EventSystem eventSystem;

	[SerializeField]
	private GameObject mainMenuObject;

	[SerializeField]
	private GameObject initialMenuButton;

	[SerializeField]
	private GameObject initialSettingsMenuButton;

	[SerializeField]
	private GameObject settingsMenuObject;

	#endregion



	public void Awake()
    {
		GameManager.Instance.MainMenuController = this;
		eventSystem.SetSelectedGameObject(initialMenuButton);
		AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.MenuGreeting);
    }

	public void EnterMainMenu() {
		mainMenuObject.SetActive(true);
		settingsMenuObject.SetActive(false);
		eventSystem.SetSelectedGameObject(initialMenuButton);
	}

	public void EnterSettingsMenu() {
		mainMenuObject.SetActive(false);
		settingsMenuObject.SetActive(true);
		eventSystem.SetSelectedGameObject(initialSettingsMenuButton);
	}
}
