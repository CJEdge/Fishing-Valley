using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utilities
{
	public static void DisableUnusedButtons(List<bool> data, List<Button> buttons) {
		for (int i = 0; i < buttons.Count; i++) {
			if (data[i]) {
				buttons[i].gameObject.SetActive(true);
			} else {
				buttons[i].gameObject.SetActive(false);
			}
		}
	}

	public static void LinkHorizontalButtons(List<Button> buttons, Button leaveShopButton) {
		for (int i = 0; i < buttons.Count; i++) {
			if (buttons[i].gameObject.activeSelf) {
				Navigation navigation = new Navigation();
				navigation.mode = Navigation.Mode.Explicit;
				if (i != 0) {
					navigation.selectOnLeft = buttons[i - 1];
				}
				if (i != buttons.Count - 1) {
					navigation.selectOnRight = buttons[i + 1];
				} else {
					navigation.selectOnRight = leaveShopButton;
				}
				buttons[i].navigation = navigation;
			}
		}
		if (leaveShopButton != null) {
			Navigation leaveShopNavigation = new Navigation();
			leaveShopNavigation.mode = Navigation.Mode.Explicit;
			leaveShopNavigation.selectOnLeft = buttons[buttons.Count - 1];
			leaveShopButton.navigation = leaveShopNavigation;
		}
	}

	public static void LinkVerticalButtons(Button[] buttons, Button leaveShopButton) {
		List<Button> activeButtons = new List<Button>();
		for (int i = 0; i < buttons.Length; i++) {
			if (buttons[i].gameObject.activeSelf) {
				activeButtons.Add(buttons[i]);
			}
		}
		for (int i = 0; i < activeButtons.Count; i++) {
			Navigation navigation = new Navigation();
			navigation.mode = Navigation.Mode.Explicit;
			if (i != 0) {
				navigation.selectOnUp = activeButtons[i - 1];
			}
			if (i != activeButtons.Count - 1) {
				navigation.selectOnDown = activeButtons[i + 1];
			} else {
				navigation.selectOnRight = leaveShopButton;
			}
			activeButtons[i].navigation = navigation;
		}
		if (leaveShopButton != null) {
			Navigation leaveShopNavigation = new Navigation();
			leaveShopNavigation.selectOnUp = activeButtons[activeButtons.Count - 1];
			leaveShopButton.navigation = leaveShopNavigation;
		}
	}
}
