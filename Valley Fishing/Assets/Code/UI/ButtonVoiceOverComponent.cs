using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonVoiceOverComponent : MonoBehaviour, IPointerEnterHandler, ISelectHandler 
{
	#region States

	public enum ButtonType {
		None,
		Play,
		Settings,
		MainMenu,
		FreeBait,
		RareBait
	}

	public ButtonType buttonType;

	#endregion


	#region Properties

	public Action SelectAction {
		get;
		set;
	}

	#endregion


	public void OnPointerEnter(PointerEventData eventData) {
		DoHoverEffect(); 
	}

	public void OnSelect(BaseEventData eventData) {
		if (InputTracker.LastInputWasMouse) {
			return;
		}
		SelectAction?.Invoke();
		DoHoverEffect();
	}

	private void DoHoverEffect() {
		switch (buttonType) {
			case ButtonType.None:
				break;
			case ButtonType.Play:
				AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.MenuPlay);
				break;
			case ButtonType.Settings:
				AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.MenuSettings);
				break;
			case ButtonType.MainMenu:
				AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.MainMenu);
				break;
			case ButtonType.FreeBait:
				AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitIntros[0]);
				break;
			case ButtonType.RareBait:
				AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitIntros[1]);
				break;
			default:
				break;
		}
	}
}
