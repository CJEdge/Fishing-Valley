using System.Collections;
using UnityEngine;

public class FishController : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private Fish[] level1Fish;

    [SerializeField]
    private float fishHookTime;

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

    #endregion


    #region Private Methods

    private IEnumerator StartHookFish() {
        yield return new WaitForSeconds(fishHookTime);
        level1Fish[this.CurrentFishIndex].HookFish();
		GameManager.Instance.InputController.BeginReel();
    }

    #endregion
}
