using UnityEngine;

public class WishlistButton : ButtonVoiceOverComponent
{
	public override bool ButtonClicked(bool buttonInteractable) {
		if (base.ButtonClicked(buttonInteractable)) {
			return false;
		}
		Application.OpenURL("https://store.steampowered.com/app/3702720/Folklore_Fishing/");
		return false;
	}
}
