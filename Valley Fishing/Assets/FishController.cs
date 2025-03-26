using System.Collections;
using TMPro;
using UnityEngine;

public class FishController : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private Fish[] level1Fish;

    [SerializeField]
    private float fishHookTime;

    [SerializeField]
    private float caughtFishDisplayTime;

    [SerializeField]
    private GameObject[] uiFishImages;

    [SerializeField]
    private GameObject fishTextObject;

    [SerializeField]
    private TMP_Text fishNameText;

    #endregion


    #region Properties

	public Fish[] Level1Fish {
		get {
			return level1Fish;
		}
	}

    public int CurrentFishIndex {
        get;
        set;
    }

    #endregion


    #region Public Methods

    public void SpawnNewFish() {
        if(this.CurrentFishIndex == level1Fish.Length - 1) {
            Debug.Log("level fin");
            return;
        }
        level1Fish[this.CurrentFishIndex].gameObject.SetActive(false);
        this.CurrentFishIndex++;
        level1Fish[this.CurrentFishIndex].gameObject.SetActive(true);
    }

	public void LandRod() {
		level1Fish[this.CurrentFishIndex].EnableVisuals(true);
		StartCoroutine(StartHookFish());
	}

    public void DisplayCaughtFish(string fishName) {
        fishNameText.text = fishName;
        fishTextObject.SetActive(true);
        GameObject uiImage = null;
        for (int i = 0; i < uiFishImages.Length; i++) {
            if (uiFishImages[i].name == fishName) {
                uiImage = uiFishImages[i];
                uiImage.SetActive(true);
            }
        }
        StartCoroutine(CloseCaughtFish(uiImage));
    }

    #endregion


    #region Private Methods

    private IEnumerator StartHookFish() {
        yield return new WaitForSeconds(fishHookTime);
        level1Fish[this.CurrentFishIndex].HookFish();
		GameManager.Instance.InputController.BeginReel();
    }

    private IEnumerator CloseCaughtFish(GameObject fishUiImage) {
        if (this.CurrentFishIndex == 4) {
            yield return new WaitForSeconds(caughtFishDisplayTime);
            FadeManager.Instance.FadeToBlack();
            fishUiImage.SetActive(false);
            fishTextObject.SetActive(false);
            this.CurrentFishIndex = 0;
            GameManager.Instance.NextLevel();
        } else {
            yield return new WaitForSeconds(caughtFishDisplayTime);
            fishUiImage.SetActive(false);
            fishTextObject.SetActive(false);
            this.CurrentFishIndex++;
            GameManager.Instance.InputController.reelState = InputController.ReelState.reelingLocked;
            VoiceOverManager.Instance.PlayCastRod();
        }
    }

    #endregion
}
