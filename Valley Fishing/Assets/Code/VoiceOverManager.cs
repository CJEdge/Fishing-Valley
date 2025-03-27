using UnityEngine;

public class VoiceOverManager : MonoBehaviour
{
    #region Singleton Behaviour

    public static VoiceOverManager Instance;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion


    #region States

    public enum VoiceOverState {
        none,
        menuIntro,
        inGameGreeting,
        castRodTutorial,
        reelingTutorial,
        caughtFish,
        castRod
    }

    public VoiceOverState voiceOverstate;

    #endregion


    #region Serialized Fields

    [SerializeField]
    private AudioSource menuGreeting;

    [SerializeField]
    private AudioSource[] inGameGreetings;

    [SerializeField]
    private AudioSource level1CastRodTutorial;

    [SerializeField]
    private AudioSource level2CastRodTutorial;

    [SerializeField]
    private AudioSource[] level1ReelingTutorial;

    [SerializeField]
    private AudioSource[] level2ReelingTutorial;

    [SerializeField]
    private AudioSource[] castRods;

    #endregion


    #region Properties

    public AudioSource CaughtFishAudio {
        get;
        set;
    }

    public int ReelTutorialIndex {
        get;
        set;
    }

    public int CastRodIndex {
        get;
        set;
    }

    #endregion


    #region Mono Behaviours

    public void Start() {
        PlayStartMenuGreeting();
    }

    public void Update() {
        switch (voiceOverstate) {
            case VoiceOverState.none:
                break;
            case VoiceOverState.menuIntro:
                break;
            case VoiceOverState.inGameGreeting:
                if (GameManager.Instance.CurrentLevel == 1) {
                    if (!inGameGreetings[0].isPlaying) {
                        PlayCastRodTutorial();
                    }
                }
                if (GameManager.Instance.CurrentLevel == 2) {
                    if (!inGameGreetings[1].isPlaying) {
                        PlayCastRodTutorial();
                    }
                }
                break;
            case VoiceOverState.castRodTutorial:
                break;
            case VoiceOverState.reelingTutorial:
                break;
            case VoiceOverState.caughtFish:
                break;
            case VoiceOverState.castRod:
                break;
            default:
                break;
        }
    }

    #endregion


    #region Public Methods

    public void PlayStartMenuGreeting() {
        menuGreeting.Play();
        voiceOverstate = VoiceOverState.menuIntro;
    }

    public void PlayInGameGreeting(int greetingIndex) {
        inGameGreetings[greetingIndex].Play();
        voiceOverstate = VoiceOverState.inGameGreeting;
    }

    public void PlayCastRodTutorial() {
		GameManager.Instance.InputController.DisableThrowing = false;
        if (GameManager.Instance.CurrentLevel == 1) {
            level1CastRodTutorial.Play();
        } else {
            level2CastRodTutorial.Play();
        }
        voiceOverstate = VoiceOverState.castRodTutorial;
    }

    public void PlayReelingTutorial() {
		if (GameManager.Instance.CurrentLevel == 1) {
			if (this.ReelTutorialIndex < 4) {
				level1ReelingTutorial[this.ReelTutorialIndex].Play();
				voiceOverstate = VoiceOverState.reelingTutorial;
				this.ReelTutorialIndex++;
			} else {
				voiceOverstate = VoiceOverState.reelingTutorial;
			}
		} else {
			if (this.ReelTutorialIndex < 3) {
				level2ReelingTutorial[this.ReelTutorialIndex].Play();
				voiceOverstate = VoiceOverState.reelingTutorial;
				this.ReelTutorialIndex++;
			} else {
				voiceOverstate = VoiceOverState.reelingTutorial;
			}
		}
    }

    public void PlayCaughtFish() {
        this.CaughtFishAudio.Play();
        voiceOverstate = VoiceOverState.caughtFish;
    }

    public void PlayCastRod() {
		GameManager.Instance.InputController.DisableThrowing = false;
		castRods[this.CastRodIndex].Play();
        voiceOverstate = VoiceOverState.castRod;
    }

    #endregion
}
