using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonVoiceOverComponent : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IMoveHandler 
{
	#region States

	public enum ButtonType {
		None,
		Play,
		Settings,
		MainMenu,
		LevelBaitButton
	}

	public ButtonType buttonType;

	#endregion


	#region Properties

	public Action SelectAction { get; set; }
	private Button ButtonReference { get; set; }
	private Button Button {
		get {
			if (this.ButtonReference == null)
				this.ButtonReference = GetComponent<Button>();

			return this.ButtonReference;
		}
	}

	#endregion


	public virtual void OnPointerEnter(PointerEventData eventData) {
		GameManager.Instance.LastSelectedButton = gameObject;
		DoHoverEffect(); 
	}

	public virtual void OnSelect(BaseEventData eventData) {
		if (buttonType == ButtonType.LevelBaitButton) {
			this.SelectAction?.Invoke();
		}
		if (GameManager.Instance.InputController.SelectionManuallySet) {
			GameManager.Instance.InputController.SelectionManuallySet = false;
			return;
		}
		GameManager.Instance.LastSelectedButton = gameObject;
		SelectAction?.Invoke();
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
			default:
				break;
		}
	}

	public void OnMove(AxisEventData eventData) {
		if (eventData.moveDir == MoveDirection.Down) {
			if (this.Button.navigation.selectOnDown == null) {
				AudioManager.Instance.PlayOneShot(FMODManager.Instance.NavigationError);
			}else if(!this.Button.navigation.selectOnDown.gameObject.activeSelf) {
				AudioManager.Instance.PlayOneShot(FMODManager.Instance.NavigationError);
			}
		}
		if (eventData.moveDir == MoveDirection.Up) {
			if (this.Button.navigation.selectOnUp == null) {
				AudioManager.Instance.PlayOneShot(FMODManager.Instance.NavigationError);
			} else if (!this.Button.navigation.selectOnUp.gameObject.activeSelf) {
				AudioManager.Instance.PlayOneShot(FMODManager.Instance.NavigationError);
			}
		}
		if (eventData.moveDir == MoveDirection.Left) {
			if (this.Button.navigation.selectOnLeft == null) {
				AudioManager.Instance.PlayOneShot(FMODManager.Instance.NavigationError);
			} else if (!this.Button.navigation.selectOnLeft.gameObject.activeSelf) {
				AudioManager.Instance.PlayOneShot(FMODManager.Instance.NavigationError);
			}
		}
		if (eventData.moveDir == MoveDirection.Right) {
			if (this.Button.navigation.selectOnRight == null) {
				AudioManager.Instance.PlayOneShot(FMODManager.Instance.NavigationError);
			} else if (!this.Button.navigation.selectOnRight.gameObject.activeSelf) {
				AudioManager.Instance.PlayOneShot(FMODManager.Instance.NavigationError);
			}
		}
	}
}
