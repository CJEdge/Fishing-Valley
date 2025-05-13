using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonVoiceOverComponent : MonoBehaviour, IPointerEnterHandler, ISelectHandler 
{
	public enum ButtonType {
		Play,
		Settings,
		MainMenu,
		WormBait,
	}

	public ButtonType buttonType;

	public void OnPointerEnter(PointerEventData eventData) {
		DoHoverEffect(); 
	}

	public void OnSelect(BaseEventData eventData) {
		if (InputTracker.LastInputWasMouse) {
			return;
		}
		DoHoverEffect();
	}

	private void DoHoverEffect() {
		switch (buttonType) {
			case ButtonType.Play:
				AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.MenuPlay);
				break;
			case ButtonType.Settings:
				AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.MenuSettings);
				break;
			case ButtonType.MainMenu:
				AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.MainMenu);
				break;
			case ButtonType.WormBait:
				AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.MenuSettings);
				break;
			default:
				break;
		}
	}
}
