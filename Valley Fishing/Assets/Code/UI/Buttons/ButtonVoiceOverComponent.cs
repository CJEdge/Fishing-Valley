using FMODUnity;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonVoiceOverComponent : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IMoveHandler, ISubmitHandler 
{
	#region Serialised Fields

    [SerializeField] protected EventReference HoverButtonSoundEventReference;

    #endregion


    #region Properties

    public Action SelectAction { get; set; }
	private Button ButtonReference { get; set; }
	public Button Button {
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
		if (GameManager.Instance.InputController.SelectionManuallySet) {
			GameManager.Instance.InputController.SelectionManuallySet = false;
			return;
		}
		GameManager.Instance.LastSelectedButton = gameObject;
		SelectAction?.Invoke();
		DoHoverEffect();
	}

	public virtual void DoHoverEffect() {
		if (!HoverButtonSoundEventReference.IsNull) {
			AudioManager.Instance.PlayVoiceOver(HoverButtonSoundEventReference);
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

	public virtual void OnSubmit(BaseEventData eventData) {
		ButtonClicked(!InputManager.Instance.InputSwitched);
	}

	public virtual bool ButtonClicked(bool buttonInteractable) {
		return buttonInteractable;
	}
}
