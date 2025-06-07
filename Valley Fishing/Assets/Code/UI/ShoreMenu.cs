using UnityEngine;
using UnityEngine.EventSystems;

public class ShoreMenu : MonoBehaviour
{
	[SerializeField]
	private int lastShopIndex = -1;

	[SerializeField]
	private GameObject[] shopButtons;

	[SerializeField]
	private GameObject shoreMenuObject;

	[SerializeField]
	private EventSystem eventSystem;

    public void Initialize() {
		AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.ShoreIntros[0]);
		shoreMenuObject.SetActive(true);
		eventSystem.SetSelectedGameObject(shopButtons[lastShopIndex + 1]);
	}

	public void EnterBaitShop() {
		shoreMenuObject.SetActive(false);
		GameManager.Instance.ShopController.SetState(ShopController.State.BaitShop);
	}
}
