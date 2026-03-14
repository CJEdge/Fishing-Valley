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
		GameManager.Instance.EventSystem = eventSystem;
		GameManager.Instance.InputController.SelectButton(initialMenuButton);
		AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.MenuGreeting);
    }

	public void EnterMainMenu() {
		mainMenuObject.SetActive(true);
		settingsMenuObject.SetActive(false);
		GameManager.Instance.InputController.SelectButton(initialMenuButton);
	}

	public void EnterSettingsMenu() {
		mainMenuObject.SetActive(false);
		settingsMenuObject.SetActive(true);
		GameManager.Instance.InputController.SelectButton(initialSettingsMenuButton);
	}
}
