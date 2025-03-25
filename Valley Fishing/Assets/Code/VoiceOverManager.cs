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
    private AudioSource castRodTutorial;

    [SerializeField]
    private AudioSource[] reelingTutorial;

    [SerializeField]
    private AudioSource[] castRods;

    #endregion


    #region Properties

    public AudioSource CaughtFishAudio {
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
                if (!inGameGreetings[0].isPlaying) { // TODO Gamemanager.levelindex
                    PlayCastRodTutorial();
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
        castRodTutorial.Play();
        voiceOverstate = VoiceOverState.castRodTutorial;
    }

    public void PlayReelingTutorial(int turotialIndex) {
        reelingTutorial[turotialIndex].Play();
        voiceOverstate = VoiceOverState.reelingTutorial;
    }

    public void PlayCaughtFish() {
        this.CaughtFishAudio.Play();
        voiceOverstate = VoiceOverState.caughtFish;
    }

    public void PlayCastRod(int castIndex) {
        castRods[castIndex].Play();
    }

    #endregion
}
