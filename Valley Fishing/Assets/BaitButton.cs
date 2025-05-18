using TMPro;
using UnityEngine;

public class BaitButton : MonoBehaviour
{

	#region Serialized Fields

	[SerializeField]
	private int baitIndex;

	[SerializeField]
	private BaitView baitView;

	[SerializeField]
	private ButtonVoiceOverComponent buttonVoiceOverComponent;

	[SerializeField]
	private TMP_Text baitCountText;

	#endregion



	public void Start() {
		buttonVoiceOverComponent.SelectAction -= SelectBait;
		buttonVoiceOverComponent.SelectAction += SelectBait;
	}

	public void OnDestroy() {
		buttonVoiceOverComponent.SelectAction -= SelectBait;
	}

	public void OnEnable() {
		baitCountText.text = "x" + GameManager.Instance.CurrentBaits[baitIndex];
	}

	public void SelectBait() {
		baitView.BaitSelected(baitIndex);
	}

	public void ClickBait() {
		baitView.BaitClicked(baitIndex);
	}
}
