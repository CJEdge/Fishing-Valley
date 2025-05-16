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

	#endregion


	#region Properties

	private int BaitIndex {
		get;
		set;
	}

	#endregion


	#region MonoBehaviours

	public void Start() {
		
	}

	#endregion


	#region Public Methods

	public void EnableBaitUI(bool enable) {
		buttonsGameobject.SetActive(enable);
		if (enable == false) {
			GameManager.Instance.LevelController.SetState(LevelController.State.IdleWithBait);
			return;
		}
		eventSystem.SetSelectedGameObject(baitButtons[0].gameObject);
	}

	public void BaitSelected(int baitIndex) {
		Debug.Log(baitIndex);
		this.BaitIndex = baitIndex;
	}

	public void BaitClicked() {
		Debug.Log(this.BaitIndex);
		GameManager.Instance.CurrentBait = GameManager.Instance.Baits[this.BaitIndex];
		EnableBaitUI(false);
	}

	#endregion
}
