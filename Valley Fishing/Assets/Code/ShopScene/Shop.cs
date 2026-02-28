using FMOD.Studio;
using FMODUnity;
using System.Collections;
using UnityEngine;

public abstract class Shop : MonoBehaviour
{

	#region Serialized Fields

	[SerializeField] private float shopEnterTime;

    #endregion


    #region Properties

    public Coroutine RunEnterShop { get; set; }

    #endregion


    #region Protected Variables

    protected EventInstance CurrentTutorialEventInstance;

	#endregion


	#region Mono Behaviours

	public virtual void Awake()
	{

	}

	public virtual void Start() {
		GameManager.Instance.InputController.OnSkip += Skip;
		AudioManager.Instance.OnVoiceLineOver += VoiceLineOver;		
	}

	public virtual void OnDestroy() {
		AudioManager.Instance.OnVoiceLineOver -= VoiceLineOver;
		GameManager.Instance.InputController.OnSkip -= Skip;
	}

    public virtual IEnumerator EnterShop(bool enter)
    {
        if (enter)
        {
            AudioManager.Instance.PlayOneShot(FMODManager.Instance.ShopEnter);
            yield return new WaitForSeconds(shopEnterTime);
        }
        else
        {
            AudioManager.Instance.PlayOneShot(FMODManager.Instance.ShopEnter);
            GameManager.Instance.ShopController.Shore.FinishedInShops[0] = true;
            GameManager.Instance.ShopController.Shore.gameObject.SetActive(true);
        }
        gameObject.SetActive(enter);
    }

    #endregion


    #region Public Methods

    public abstract void VoiceLineOver(EventReference eventReference, bool skipped);

	public bool PlayNextTutotialVoiceOver(bool[] tutorialsCompleted, EventReference[] tutorialVoiceLines) {
		if(tutorialsCompleted == null) {
			return false;
		}
		for (int i = 0; i < tutorialsCompleted.Length; i++) {
			if (!tutorialsCompleted[i]) {
				AudioManager.Instance.PlayVoiceOver(tutorialVoiceLines[i]);
				this.CurrentTutorialEventInstance = AudioManager.Instance.VoiceLineEventInstance;
				return true;
			}
		}
		return false;
	}

	public void IncrementTutorial(bool[] tutorial) {
		if(tutorial == null) {
			return;
		}
		for (int i = 0; i < tutorial.Length; i++) {
			if (!tutorial[i]) {
				tutorial[i] = true;
				break;
			}
		}
	}

	public virtual void Skip() {

	}
    public virtual void LeaveShop()
    {
        this.RunEnterShop = StartCoroutine(EnterShop(false));
    }

    #endregion
}
