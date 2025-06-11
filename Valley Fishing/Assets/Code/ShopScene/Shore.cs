using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shore : MonoBehaviour
{

	#region Serialized Fields

	[SerializeField]
	private int lastShopIndex = -1;

	[SerializeField]
	private GameObject[] shopButtons;

	[SerializeField]
	private GameObject shoreMenuObject;

	[SerializeField]
	private EventSystem eventSystem;

	#endregion


	#region Properties

	[field:SerializeField]
	public List<bool> FinishedInShops {
		get;
		set;
	} = new List<bool>();

	public bool AllShopsFinished {
		get;
		set;
	}

	#endregion


	#region Mono Behaviours

	public void Awake() {
		for (int i = 0; i < shopButtons.Length -1; i++) {
			this.FinishedInShops.Add(false);
		}
	}

	#endregion


	#region Public Methods

	public virtual void Initialize() {
		for (int i = 0; i < this.FinishedInShops.Count; i++) {
			if (!this.FinishedInShops[i]) {
				this.AllShopsFinished = false;
				break;
			}
		}
		this.AllShopsFinished = true;
		shoreMenuObject.SetActive(true);		
		eventSystem.SetSelectedGameObject(shopButtons[lastShopIndex + 1]);
	}

	public void EnterBaitShop() {
		shoreMenuObject.SetActive(false);
		GameManager.Instance.ShopController.SetState(ShopController.State.BaitShop);
	}

	public void FinishedInShop(Shop shopType) {
		if(shopType is BaitShop) {
			this.FinishedInShops[0] = true;
		}
	}

	#endregion
}
