using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaitView : MonoBehaviour
{
	#region Serialized Fields

	[SerializeField] private EventSystem eventSystem;
	[SerializeField] private GameObject buttonsGameobject;
	[SerializeField] private Button[] baitButtons;
	[SerializeField] private float buttonMoveTime;
	[SerializeField] private float baitBoxOpeningTime;

	#endregion


	#region Properties

	private int BaitIndex {	get; set; }

	#endregion


	#region MonoBehaviours

	public void Start() {
		buttonsGameobject.SetActive(false);
		AudioManager.Instance.OnVoiceLineOver -= SkipBaitSelection;
		AudioManager.Instance.OnVoiceLineOver += SkipBaitSelection;
	}

	public void OnDestroy() {
		AudioManager.Instance.OnVoiceLineOver -= SkipBaitSelection;
	}

	#endregion


	#region Public Methods

	public void EnableBaitUI(bool enable) {
		buttonsGameobject.SetActive(enable);
		if (enable == false) {
			GameManager.Instance.LevelController.SetState(LevelController.State.IdleWithBait);
			GameManager.Instance.EventSystem.SetSelectedGameObject(null);
			return;
		} else {
			bool firstButtonSelected = false;
			for (int i = 0; i < baitButtons.Length; i++) {
				baitButtons[i].gameObject.SetActive(false);
			}
			for (int i = 0; i < InventoryManager.Instance.OwnedBaitTypeDatas.Count; i++) {
				if (InventoryManager.Instance.OwnedBaitTypeDatas[i].quantity > 0) {
					baitButtons[i].gameObject.SetActive(true);
					if (!firstButtonSelected) {
						StartCoroutine(SelectButtonAfterOneFrame(baitButtons[i].gameObject));
						GameManager.Instance.InputController.SelectButton(baitButtons[i].gameObject);
						firstButtonSelected = true;
					}
				}
			}
		}
		GameManager.Instance.InputController.SelectButton(baitButtons[0].gameObject);
		AudioManager.Instance.PlayBaitSound(false, 0);
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.BaitBoxOpen);
		StartCoroutine(WaitToOpenBaitBox());
	}

	public void BaitSelected(int baitIndex) {
		if(baitIndex == this.BaitIndex) {
			return;
		}
		AudioManager.Instance.PlayBaitSound(false, this.BaitIndex);
		this.BaitIndex = baitIndex;
		AudioManager.Instance.PlayBaitSound(true, this.BaitIndex);
	}

	public void BaitClicked(int baitIndex) {
		AudioManager.Instance.PlayBaitSound(false, 0);
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.AttatchBaitSounds[baitIndex]);
		InventoryManager.Instance.CurrentBait = InventoryManager.Instance.BaitDatas.datas[baitIndex];
		InventoryManager.Instance.OwnedBaitTypeDatas[baitIndex].quantity--;
		EnableBaitUI(false);
	}

	#endregion

	#region Private Methods

	private void SkipBaitSelection(EventReference eventReference, bool value) {
		if(GameManager.Instance.LevelController.CurrentState != LevelController.State.AttatchBait) {
			return;
		}
		if (AudioManager.Instance.VoiceLineInProgress){
			return;
		}
		int activeButtons = 0;
		for (int i = 0; i < baitButtons.Length; i++) {
			if (baitButtons[i].isActiveAndEnabled) {
				activeButtons++;
			}
		}
		if (activeButtons > 1) {
			return;
		}
		BaitClicked(this.BaitIndex);
	}

	private IEnumerator SelectButtonAfterOneFrame(GameObject button) {
		yield return new WaitForEndOfFrame();
		GameManager.Instance.InputController.SelectButton(button);
	}

	private IEnumerator WaitToOpenBaitBox() {
		yield return new WaitForSeconds(baitBoxOpeningTime);
		AudioManager.Instance.PlayBaitSound(true, this.BaitIndex);
	}

	#endregion
}
