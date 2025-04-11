using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{

	#region States

	public enum ReelState {
		reelingLocked,
		notReeling,
		calmReeling,
		normalReeling,
		fastReeling
	}

	public ReelState reelState;

	#endregion


	#region Serialized Fields

	[SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    private float rodSpeed;

	[SerializeField]
	private float keyboardReelResetTime;

	[SerializeField]
	private int keyboardInputListLength;

	[SerializeField]
    private float reelResetRate;

	[SerializeField]
	private float completeReelResetTime;

    [SerializeField]
    private float maxReelRate;

	[SerializeField]
	private List<float> scrollReelStateThresholds;

	[SerializeField]
	private List<float> keyboardReelStateThresholds;

	[SerializeField]
	private AudioRandomizer[] reelAudio;

	[SerializeField]
	private PlayerArms playerArms;

	[SerializeField]
	private GameObject blurObject;

	[SerializeField]
	private GameObject blackObject;


    #endregion


    #region Properties

	public bool ClickTrigger {
		get;
		set;
    }

    public Vector2 MouseInput {
        get;
        set;
    }


	[field:SerializeField]
    public float ReelInput {
        get;
        set;
    }

	private float LastReelInput {
		get;
		set;
	}

	private float CurrentKeyboardReelInput {
		get;
		set;
	}

	[field:SerializeField]
	private float AverageKeyboardReelInput {
		get {
			float total = 0;
			for (int i = 0; i < this.ReelInputs.Count; i++) {
				total += this.ReelInputs[i];
			}
			return total / this.ReelInputs.Count;
		}
	}

	[field: SerializeField]
	private List<float> ReelInputs {
		get;
		set;
	} = new List<float>();

    private float CurrentReelResetRate {
        get;
        set;
    }

	private float CurrentCompleteReelResetTime {
		get;
		set;
	}

	public bool StrafingEnabled {
		get;
		set;
	}

	public AudioRandomizer CurrentReelAudio {
		get;
		set;
	}

	[field:SerializeField]
	public bool DisableThrowing {
		get;
		set;
	}

    #endregion


    #region Mono Behaviours

	public void Start() {
		this.DisableThrowing = false;
	}

	public void Update() {
		Debug.Log(AverageKeyboardReelInput);
		SetKeyboardReeling();
		SetReelState();
		if (reelState == ReelState.reelingLocked) {
			return;
		}
		CheckToResetReelInput();
		ReduceReelRate();
	}

	#endregion


	#region Public Methods

	public void Click(InputAction.CallbackContext context) {
		if (context.performed) {
			this.ClickTrigger = true;
			CastRod();
			this.DisableThrowing = true;

		} else {
			this.ClickTrigger = false;
        }
	}

	public void CastRod() {
		if (this.DisableThrowing) {
			return;
		}
		if (VoiceOverManager.Instance.voiceOverstate == VoiceOverManager.VoiceOverState.castRodTutorial ||
			VoiceOverManager.Instance.voiceOverstate == VoiceOverManager.VoiceOverState.castRod) {
			playerArms.ThrowRod();
		}	
	}

	public void BeginReel() {
		if(GameManager.Instance.CurrentLevel == 2) {
			this.StrafingEnabled = true;
		}
		this.DisableThrowing = false;
		reelState = ReelState.notReeling;
		VoiceOverManager.Instance.PlayReelingTutorial();
		playerArms.BeginReel();
	}

	public void MoveRod(InputAction.CallbackContext context) {
        this.MouseInput = context.ReadValue<Vector2>() * rodSpeed;
    }

    public void ReelMouse(InputAction.CallbackContext context) {
		if(reelState == ReelState.reelingLocked) {
			return;
        }
        this.ReelInput -= context.ReadValue<Vector2>().y;
		StartCoroutine(SetLastReelInput());
	}

	public void ReelKeyboard(InputAction.CallbackContext context) {
		if (context.performed) {
			if (this.ReelInputs.Count == keyboardInputListLength) {
				this.ReelInputs.RemoveAt(0);
			}
			this.ReelInputs.Add(this.CurrentKeyboardReelInput);
			this.CurrentKeyboardReelInput = 0;
		}
	}

	public void Blur(InputAction.CallbackContext context) {
		blurObject.SetActive(!blurObject.activeSelf);
	}

	public void Black(InputAction.CallbackContext context) {
		blackObject.SetActive(!blackObject.activeSelf);
	}

	#endregion


	#region Private Methods

	private void CheckToResetReelInput() {
		if (this.LastReelInput > this.ReelInput) {
			if (this.CurrentCompleteReelResetTime < completeReelResetTime) {
				this.CurrentCompleteReelResetTime += Time.deltaTime;
			} else {
				this.CurrentCompleteReelResetTime = 0;
				this.ReelInput = 0;
			}
		} else {
			this.CurrentCompleteReelResetTime = 0;
		}
	}

    private void ReduceReelRate() {
		if (this.CurrentReelResetRate < reelResetRate) {
			this.CurrentReelResetRate += Time.deltaTime;
		} else {
			this.CurrentReelResetRate = 0;
			this.ReelInput = Mathf.Clamp(this.ReelInput - 1, 0, maxReelRate);
		}
	}

	private void SetKeyboardReeling() {
		if (this.CurrentKeyboardReelInput < keyboardReelResetTime) {
			this.CurrentKeyboardReelInput += Time.deltaTime;
		} else {
			this.CurrentKeyboardReelInput = 0;
			this.ReelInputs.Clear();
		}
	}

	private void SetReelState() {
		if(this.ReelInput == scrollReelStateThresholds[0] && this.AverageKeyboardReelInput < keyboardReelStateThresholds[0]) {
			reelState = ReelState.notReeling;
			SetReelAudio(null);
		} else if(this.ReelInput < scrollReelStateThresholds[1] || this.AverageKeyboardReelInput < keyboardReelStateThresholds[1]) {
			reelState = ReelState.calmReeling;
			SetReelAudio(reelAudio[0]);
		} else if (this.ReelInput < scrollReelStateThresholds[2] || this.AverageKeyboardReelInput < keyboardReelStateThresholds[2]) {
			reelState = ReelState.normalReeling;
			SetReelAudio(reelAudio[1]);
		} else if(this.ReelInput < scrollReelStateThresholds[3] || this.AverageKeyboardReelInput < keyboardReelStateThresholds[3]){
			reelState = ReelState.fastReeling;
			SetReelAudio(reelAudio[2]);
		}
	}

	private void SetReelAudio(AudioRandomizer audio) {
		this.CurrentReelAudio = audio;
		if (audio != null) {
			if (audio.CurrentAudioSource != null) {
				if (audio.CurrentAudioSource.isPlaying) {
					return;
				}
			}
		}
		for (int i = 0; i < reelAudio.Length; i++) {
			reelAudio[i].gameObject.SetActive(false);
        }
		if (audio != null) {
			if (!audio.gameObject.activeSelf) {
				audio.gameObject.SetActive(true);
			}
		}
    }

	#endregion


	#region Coroutines

	private IEnumerator SetLastReelInput() {
		yield return new WaitForEndOfFrame();
		this.LastReelInput = this.ReelInput;
	}

	#endregion

}
