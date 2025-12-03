using FMODUnity;
using UnityEngine;

public class FishBoard : MonoBehaviour
{
	[SerializeField] private GameObject fishBoardObject;
	[SerializeField] private GameObject initialButton;
	[SerializeField] private GameObject[] baitshopComponents;

	public void OpenFishBoard() {
		if (!fishBoardObject.activeSelf) {
			fishBoardObject.SetActive(true);
			for (int i = 0; i < baitshopComponents.Length; i++) {
				baitshopComponents[i].SetActive(!fishBoardObject.activeSelf);
			}
			GameManager.Instance.EventSystem.SetSelectedGameObject(initialButton);
		} else {
			for (int i = 0; i < baitshopComponents.Length; i++) {
				baitshopComponents[i].SetActive(!fishBoardObject.activeSelf);
			}
			GameManager.Instance.EventSystem.SetSelectedGameObject(baitshopComponents[0]);
			fishBoardObject.SetActive(false);
		}
	}
}
