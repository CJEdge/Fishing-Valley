using System.Collections;
using TMPro;
using UnityEngine;

public class FishController : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private Fish[] level1Fish;

	[SerializeField]
	private Fish[] level2Fish;

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
		if (GameManager.Instance.CurrentLevel == 1) {
			level1Fish[this.CurrentFishIndex].gameObject.SetActive(true);
		} else {
			level2Fish[this.CurrentFishIndex].gameObject.SetActive(true);
		}
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
		if (GameManager.Instance.CurrentLevel == 1) {
			level1Fish[this.CurrentFishIndex].HookFish();
		} else {
			level2Fish[this.CurrentFishIndex].HookFish();
		}
		GameManager.Instance.InputController.BeginReel();
    }

    private IEnumerator CloseCaughtFish(GameObject fishUiImage) {
		if (this.CurrentFishIndex == 4) {
        yield return new WaitForSeconds(caughtFishDisplayTime);
			if (GameManager.Instance.CurrentLevel == 1) {
				level1Fish[this.CurrentFishIndex].gameObject.SetActive(false);
			} else {
				yield return new WaitForSeconds(caughtFishDisplayTime/2);
				level2Fish[this.CurrentFishIndex].gameObject.SetActive(false);
				FadeManager.Instance.FadeToBlack();
				yield break;
			}
			FadeManager.Instance.FadeToBlack();
        fishUiImage.SetActive(false);
        fishTextObject.SetActive(false);
        this.CurrentFishIndex = 0;
        GameManager.Instance.NextLevel();
        } else {
            yield return new WaitForSeconds(caughtFishDisplayTime);
			if (GameManager.Instance.CurrentLevel == 1) {
				level1Fish[this.CurrentFishIndex].gameObject.SetActive(false);
			} else {
				level2Fish[this.CurrentFishIndex].gameObject.SetActive(false);
			}
			fishUiImage.SetActive(false);
            fishTextObject.SetActive(false);
            this.CurrentFishIndex++;
            GameManager.Instance.InputController.reelState = InputController.ReelState.reelingLocked;
            VoiceOverManager.Instance.PlayCastRod();
        }
		
	}

    #endregion
}
