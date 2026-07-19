using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class IcthyologistsFishButton : ButtonVoiceOverComponent {

	[SerializeField] private FishDatas fishDatas;
	[SerializeField] private ItemDataButton itemDataButton;

	public override void OnSelect(BaseEventData eventData) {
		base.OnSelect(eventData);
        int index = System.Array.IndexOf(fishDatas.Datas, itemDataButton.ItemData);
        GameManager.Instance.ShopController.Icthyologists.HoverFish(index);
	}

<<<<<<< HEAD
	public override void OnSubmit(BaseEventData eventData) {
		base.OnSubmit(eventData);
		int index = System.Array.IndexOf(fishDatas.Datas, itemDataButton.ItemData);
=======
	public override bool ButtonClicked(bool buttonInteractable) {
		if (base.ButtonClicked(buttonInteractable)) {
			return false;
		}
		int index = System.Array.IndexOf(fishDatas.Datas, itemDataButton.itemData);
>>>>>>> d94375aaa1f0ba69beae991508906d2a5be009e9
		GameManager.Instance.ShopController.Icthyologists.SellFish(index);
		return false;
	}
}
