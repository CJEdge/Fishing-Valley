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

    #endregion


    #region Public Methods

    public void Update() {
        if (inputController.ClickTrigger) {
            if (menuUI.activeSelf) {
                menuUI.SetActive(false);
            }
        }
    }

    #endregion

}
