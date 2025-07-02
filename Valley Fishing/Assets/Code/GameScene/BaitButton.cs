using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
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


	#region Mono Behaviours
	public void Awake() {
		buttonVoiceOverComponent.SelectAction -= SelectBait;
		buttonVoiceOverComponent.SelectAction += SelectBait;
	}

	public void OnDestroy() {
		buttonVoiceOverComponent.SelectAction -= SelectBait;
	}

	public void OnEnable() {
		baitCountText.text = "x" + GameManager.Instance.CurrentBaits[baitIndex];
	}

	#endregion


	#region Public Methods

	public void SelectBait() {
		baitView.BaitSelected(baitIndex);
		if (!GameManager.Instance.Baits[baitIndex].IsTutorial) {
			if (GameManager.Instance.CurrentBaits[baitIndex] <= FMODManager.Instance.BaitNumbers.Length) {
				List<EventReference> voiceOverChain = new List<EventReference>();
				voiceOverChain.Add(FMODManager.Instance.YouHave);
				voiceOverChain.Add(FMODManager.Instance.BaitNumbers[GameManager.Instance.CurrentBaits[baitIndex] - 1]);
				voiceOverChain.Add(FMODManager.Instance.Left);
				AudioManager.Instance.PlayVoiceOverChain(voiceOverChain);
			}
		}
	}

	public void ClickBait() {
		baitView.BaitClicked(baitIndex);
	}

	#endregion

}
