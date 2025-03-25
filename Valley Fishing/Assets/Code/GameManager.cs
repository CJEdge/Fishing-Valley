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

    #endregion


    #region Properties

    public InputController InputController {
        get {
            return inputController;
        }
    }

    #endregion

}
