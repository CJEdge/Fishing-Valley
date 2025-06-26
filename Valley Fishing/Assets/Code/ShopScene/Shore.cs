using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shore : MonoBehaviour
{
	#region Levels

	public enum LevelToLoad {
		CatchTutorial_00,
		CatchTutorial_01,
		CatchTutorial_02,
		CatchTutorial_03,
	}

	#endregion


	#region Serialized Fields

	[SerializeField]
	private int lastShopIndex = -1;

	[SerializeField]
	protected GameObject[] shopButtons;

	[SerializeField]
	private GameObject shoreMenuObject;

	[SerializeField]
	protected EventSystem eventSystem;

	#endregion


	#region Properties

	[field:SerializeField]
	public List<bool> FinishedInShops {
		get;
		set;
	} = new List<bool>();

	[field: SerializeField]
	public bool AllShopsFinished {
		get;
		set;
	}

	public int CurrentButtonIndex {
		get;
		set;
	}

	[field: SerializeField]
	public int TimesSkipped {
		get;
		set;
	}

	#endregion


	#region Public Methods

	public virtual void Initialize() {
		if(this.FinishedInShops.Count == 0) {
			for (int i = 0; i < shopButtons.Length - 1; i++) {
				this.FinishedInShops.Add(false);
			}
			AudioManager.Instance.OnVoiceLineOver += VoiceOverSkipped;
		}

		bool allShopsFinished = true;
		for (int i = 0; i < this.FinishedInShops.Count; i++) {
			if (!this.FinishedInShops[i]) {
				allShopsFinished = false;
				break;
			}
		}
		this.AllShopsFinished = allShopsFinished;
		this.TimesSkipped = 0;
		shoreMenuObject.SetActive(true);
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

	public virtual void ButtonSelected(int buttonIndex) {
		this.CurrentButtonIndex = buttonIndex;
	}

	public virtual void VoiceOverSkipped(EventInstance eventInstance) {
		if (!gameObject.activeInHierarchy) {
			return;
		}
		if (this.TimesSkipped == 0) {
			this.TimesSkipped = 1;
		} else if (this.TimesSkipped == 1) {
			this.TimesSkipped = 0;
		}
	}

	#endregion
}
