using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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

	[SerializeField] private int lastShopIndex = -1;
	[SerializeField] protected GameObject[] shopButtons;
	[SerializeField] protected EventSystem eventSystem;

	#endregion


	#region Properties

	[field:SerializeField] public List<bool> FinishedInShops { get;	set; } = new List<bool>();
	[field: SerializeField] public bool AllShopsFinished { get; set; }
	public int CurrentButtonIndex {	get; set; }
	[field: SerializeField]	public int TimesSkipped { get; set; }
	private ShopController ShopController { get => GameManager.Instance.ShopController; }

	#endregion


	#region Public Methods

	public virtual void OnEnable() {
		if (this.FinishedInShops.Count == 0) {
			for (int i = 0; i < shopButtons.Length - 1; i++) {
				this.FinishedInShops.Add(false);
			}
			AudioManager.Instance.OnVoiceLineOver += VoiceLineOver;
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
		this.ShopController.Shore.gameObject.SetActive(true);
	}

	public void OnDestroy() {
		AudioManager.Instance.OnVoiceLineOver -= VoiceLineOver;
	}

	public void EnterBaitShop() {
        this.ShopController.Shore.gameObject.SetActive(false);
		this.ShopController.BaitShop.gameObject.SetActive(true);
	}

	public void EnterRodShop()
	{
        this.ShopController.Shore.gameObject.SetActive(false);
		this.ShopController.RodShop.gameObject.SetActive(true);
	}
    public void EnterIcthyologists()
    {
        this.ShopController.Shore.gameObject.SetActive(false);
		this.ShopController.Icthyologists.gameObject.SetActive(true);
    }
    public void EneterInventorsLab()
    {
        this.ShopController.Shore.gameObject.SetActive(false);
		this.ShopController.InventorsLab.gameObject.SetActive(true);

	}

	public void LeaveShore() {
		SceneManager.LoadScene("04_GameScene");
	}

    public void FinishedInShop(Shop shopType) {
		if(shopType is BaitShop) {
			this.FinishedInShops[0] = true;
		}
	}

	public virtual void ButtonSelected(int buttonIndex) {
		this.CurrentButtonIndex = buttonIndex;
	}

	public virtual void VoiceLineOver(EventReference eventReference, bool skipped) {
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
