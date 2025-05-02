using UnityEngine;

public class OpenMenuButton : MonoBehaviour
{
	#region Serialized Fields

	[SerializeField]
	private GameObject menuToClose;

	[SerializeField]
	private GameObject menuToOpen;

	#endregion


	#region Public Methods

	public void OpenMenu() {
		menuToOpen.SetActive(true);
		menuToClose.SetActive(false);
	}

	#endregion

}
