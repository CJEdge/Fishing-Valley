using FMODUnity;
using UnityEngine;

public class FishBoard : MonoBehaviour
{
	[SerializeField]
	private GameObject fishBoardObject;

	[SerializeField]
	private GameObject initialButton;

	[SerializeField]
	private GameObject[] baitshopComponents;

	public EventReference[] fishNames;

	public void OpenFishBoard() {
		fishBoardObject.SetActive(!fishBoardObject.activeSelf);
		for (int i = 0; i < baitshopComponents.Length; i++) {
			baitshopComponents[i].SetActive(!fishBoardObject.activeSelf);
		}
		if (fishBoardObject.activeSelf) {
			GameManager.Instance.EventSystem.SetSelectedGameObject(initialButton);
		}
	}
}
