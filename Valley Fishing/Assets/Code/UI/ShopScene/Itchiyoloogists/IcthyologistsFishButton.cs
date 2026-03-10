using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class IcthyologistsFishButton : ButtonVoiceOverComponent {

	[SerializeField] private FishDatas fishDatas;
	[SerializeField] private FishDataButton fishDataButton;

	public override void OnSelect(BaseEventData eventData) {
		base.OnSelect(eventData);
		int index = ArrayUtility.IndexOf(fishDatas.Datas, fishDataButton.FishData);
		GameManager.Instance.ShopController.Icthyologists.HoverFish(index);		
	}

	public override void OnSubmit(BaseEventData eventData) {
		base.OnSubmit(eventData);
		int index = ArrayUtility.IndexOf(fishDatas.Datas, fishDataButton.FishData);
		GameManager.Instance.ShopController.Icthyologists.SellFish(index);
	}
}
