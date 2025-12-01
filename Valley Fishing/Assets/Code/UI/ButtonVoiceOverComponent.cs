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
		LevelBaitButton,
		FreeBait,
		RareBait,
		FishBoard
	}

	public ButtonType buttonType;

	#endregion


	#region Properties

	public Action SelectAction {
		get;
		set;
	}

	#endregion


	public virtual void OnPointerEnter(PointerEventData eventData) {
		GameManager.Instance.LastSelectedButton = gameObject;
		Debug.Log("pointer enter");
		DoHoverEffect(); 
	}

	public virtual void OnSelect(BaseEventData eventData) {
		Debug.Log("select");
		if (buttonType == ButtonType.LevelBaitButton) {
			SelectAction?.Invoke();
		}
		if (GameManager.Instance.InputController.SelectionManuallySet) {
			GameManager.Instance.InputController.SelectionManuallySet = false;
			return;
		}
		GameManager.Instance.LastSelectedButton = gameObject;
		if (InputTracker.LastInputWasMouse) {
			return;
		}
		SelectAction?.Invoke();
		Debug.Log("select end");
		DoHoverEffect();
	}

	public virtual void DoHoverEffect() {
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
