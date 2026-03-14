using UnityEngine;

public class BaitShopUI : MonoBehaviour
{
    [SerializeField]
    public GameObject individualFishSellingBoard;
    [SerializeField]
    public GameObject baitBuyingBoard;
    [SerializeField]
    public GameObject baitShopButtons;

    private GameObject currentEnteredMenu;

    public void GoBack()
    {
        currentEnteredMenu.gameObject.SetActive(false);
        baitShopButtons.gameObject.SetActive(true);
    }

    public void EnterMenu()
    {
        currentEnteredMenu.gameObject.SetActive(true);
        baitShopButtons.SetActive(false);
    }

    public void EnterIndividualSellingBoard()
    {
        currentEnteredMenu = individualFishSellingBoard;
        EnterMenu();
    }

    public void EnterBaitBuyingBoard()
    {
        currentEnteredMenu = baitBuyingBoard;
        EnterMenu();
    }
}
