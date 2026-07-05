using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class IcthyologistsFishButton : ButtonVoiceOverComponent {

	[SerializeField] private FishDatas fishDatas;
	[SerializeField] private ItemDataButton itemDataButton;

	public override void OnSelect(BaseEventData eventData) {
		base.OnSelect(eventData);
        int index = System.Array.IndexOf(fishDatas.Datas, itemDataButton.itemData);
        GameManager.Instance.ShopController.Icthyologists.HoverFish(index);
	}

	public override bool ButtonClicked(bool buttonInteractable) {
		if (base.ButtonClicked(buttonInteractable)) {
			return false;
		}
		int index = System.Array.IndexOf(fishDatas.Datas, itemDataButton.itemData);
		GameManager.Instance.ShopController.Icthyologists.SellFish(index);
		return false;
	}
}
