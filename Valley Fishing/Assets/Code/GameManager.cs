using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton Behaviour

    public static GameManager Instance;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion


    #region Serialized Fields

    [SerializeField]
    private InputController inputController;

    [SerializeField]
    private FishController fishController;

    [SerializeField]
    private Transform leftFishTransform;

    [SerializeField]
    private Transform rightFishTransform;

    [SerializeField]
    private GameObject menuUI;

    [SerializeField]
    private float levelLoadWaitTime;

    [SerializeField]
    private GameObject level1Ambience;

    [SerializeField]
    private GameObject level2Ambience;

    #endregion


    #region Properties

    public InputController InputController {
        get {
            return inputController;
        }
    }

    public FishController FishController {
        get { 
            return fishController;
        }
    }

    public Transform LeftFishTransform {
        get {
            return leftFishTransform;
        }
    }

    public Transform RightFishTransform {
        get {
            return rightFishTransform;
        }
    }

    public int CurrentLevel {
        get;
        set;
    } = 1;

    #endregion


    #region Public Methods

    public void Update() {
        if (inputController.ClickTrigger) {
            if (menuUI.activeSelf) {
                menuUI.SetActive(false);
				VoiceOverManager.Instance.voiceOverstate = VoiceOverManager.VoiceOverState.inGameGreeting;
            }
        }
    }

    public void NextLevel() {
        this.CurrentLevel++;
        StartCoroutine(StartNextLevel());
    }

    #endregion


    #region Private Methods

    private IEnumerator StartNextLevel() {
        InputController.reelState = InputController.ReelState.reelingLocked;
        VoiceOverManager.Instance.CastRodIndex = 0;
        VoiceOverManager.Instance.ReelTutorialIndex = 0;
        yield return new WaitForSeconds(levelLoadWaitTime);
        FadeManager.Instance.FadeToClear();
        VoiceOverManager.Instance.voiceOverstate = VoiceOverManager.VoiceOverState.inGameGreeting;
        level1Ambience.SetActive(false);
        level2Ambience.SetActive(true);
    }

    #endregion

}
