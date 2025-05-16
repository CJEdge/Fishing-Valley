using UnityEngine;

public class BaitButton : MonoBehaviour
{

	[SerializeField]
	private int baitIndex;

	[SerializeField]
	private BaitView baitView;

	[SerializeField]
	private ButtonVoiceOverComponent buttonVoiceOverComponent;

	public void Start() {
		buttonVoiceOverComponent.SelectAction -= SelectBait;
		buttonVoiceOverComponent.SelectAction += SelectBait;
	}

	public void OnDestroy() {
		buttonVoiceOverComponent.SelectAction -= SelectBait;
	}

	public void SelectBait() {
		baitView.BaitSelected(baitIndex);
	}

	public void ClickBait() {
		baitView.BaitClicked();
	}
}
