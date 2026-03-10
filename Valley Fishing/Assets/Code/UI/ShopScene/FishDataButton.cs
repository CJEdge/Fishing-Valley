using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishDataButton : MonoBehaviour
{
	public Button Button;
	public TMP_Text FishName;
	public Image FishImage;
	public TMP_Text SellPrice;
	public FishDatas.FishData FishData;

	public void AssignData(FishDatas.FishData fishData) {
		FishData = fishData;
		if (FishName != null) {
			FishName.text = FishData.FishName;
		}
		if (FishImage != null) {
			FishImage.sprite = FishData.Sprite;
		}
		if(SellPrice != null) {
			SellPrice.text = FishData.SellPrice.ToString();
		}
	}
}
