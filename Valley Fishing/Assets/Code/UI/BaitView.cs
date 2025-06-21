using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaitView : MonoBehaviour
{
	#region Serialized Fields

	[SerializeField]
	private EventSystem eventSystem;

	[SerializeField]
	private GameObject buttonsGameobject;

	[SerializeField]
	private Button[] baitButtons;

	[SerializeField]
	private float buttonMoveTime;

	[SerializeField]
	private float baitBoxOpeningTime;

	#endregion


	#region Properties

	private int BaitIndex {
		get;
		set;
	}

	#endregion


	#region MonoBehaviours

	public void Start() {
		buttonsGameobject.SetActive(false);
	}

	#endregion


	#region Public Methods

	public void EnableBaitUI(bool enable) {
		buttonsGameobject.SetActive(enable);
		if (enable == false) {
			GameManager.Instance.LevelController.SetState(LevelController.State.IdleWithBait);
			return;
		} else {
			bool firstButtonSelected = false;
			for (int i = 0; i < baitButtons.Length; i++) {
				baitButtons[i].gameObject.SetActive(false);
			}
			for (int i = 0; i < GameManager.Instance.CurrentBaits.Count; i++) {
				if (GameManager.Instance.CurrentBaits[i] > 0) {
					baitButtons[i].gameObject.SetActive(true);
					if (!firstButtonSelected) {
						StartCoroutine(SelectButtonAfterOneFrame(baitButtons[i].gameObject));
						eventSystem.SetSelectedGameObject(baitButtons[i].gameObject);
						firstButtonSelected = true;
					}
				}
			}
		}
		eventSystem.SetSelectedGameObject(baitButtons[0].gameObject);
		AudioManager.Instance.PlayBaitSound(false, 0);
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.BaitBoxOpen);
		StartCoroutine(WaitToOpenBaitBox());
	}

	public void BaitSelected(int baitIndex) {
		this.BaitIndex = baitIndex;
		
	}

	public void BaitClicked(int baitIndex) {
		AudioManager.Instance.PlayBaitSound(false, 0);
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.AttatchBaitSounds[baitIndex]);
		GameManager.Instance.CurrentBait = GameManager.Instance.Baits[baitIndex];
		GameManager.Instance.CurrentBaits[GameManager.Instance.CurrentBait.BaitIndex]--;
		EnableBaitUI(false);
	}

	#endregion

	#region Private Methods

	private IEnumerator SelectButtonAfterOneFrame(GameObject button) {
		yield return new WaitForEndOfFrame();
		eventSystem.SetSelectedGameObject(button);
	}

	private IEnumerator WaitToOpenBaitBox() {
		yield return new WaitForSeconds(baitBoxOpeningTime);
		AudioManager.Instance.PlayBaitSound(true, this.BaitIndex);
	}

	#endregion
}
