using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDataButton : MonoBehaviour
{
	public Button Button;
	public TMP_Text ItemName;
	public Image ItemImage;
	public TMP_Text ItemSellPrice;
	public BaseItemData itemData;
	public void AssignData(BaseItemData data) {
		if (ItemName != null) {
			ItemName.text = data.ItemName;
		}
		if (ItemImage != null) {
			ItemImage.sprite = data.ItemImage;
		}
		if(ItemSellPrice != null) {
			ItemSellPrice.text = data.ItemSellPrice.ToString();
		}
	}
}
