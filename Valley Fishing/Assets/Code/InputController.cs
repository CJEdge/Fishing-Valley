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
	private int keyboardInputListLength;

	

   

	[Header("Components")]

	[SerializeField]
	private PlayerInput playerInput;

	[SerializeField]
	private AudioRandomizer[] reelAudio;

	[SerializeField]
	private GameObject blurObject;

	[SerializeField]
	private GameObject blackObject;

	[SerializeField]
	private PlayerArms playerArms;

	[Header("Variables")]

	[Header("Keyboard Input")]

	[SerializeField]
	private List<float> keyboardReelStateThresholds;

	[SerializeField]
	private float keyboardReelResetTime;

	[Header("Mouse Input")]

	[SerializeField]
	private List<float> mouseReelStateThresholds;

	[SerializeField]
	private float mouseReelResetRate;

	[SerializeField]
	private float mouseCompleteReelResetTime;

	[SerializeField]
	private float mouseHorizontalSpeed;

	[SerializeField]
	private float maxMouseReelRate;

	[Header("Controller Input")]

	[SerializeField]
	private List<float> controllerReelStateThresholds;

	[SerializeField]
	private float controllerReelMultiplier;

	[SerializeField]
	private float controllerReelResetRate;

	[SerializeField]
	private float controllerCompleteReelResetTime;

	[SerializeField]
	private float controllerHorizontalSpeed;

	[SerializeField]
	private float maxControllerReelRate;

	#endregion


	#region Properties

	public bool ClickTrigger {
		get;
		set;
    }

	[field:SerializeField]
    public Vector2 HorizontalInput {
        get;
        set;
    }

	[field:SerializeField]
    public float MouseReelInput {
        get;
        set;
    }

	[field: SerializeField]
	public float ControllerReelInput {
		get;
		set;
	}

	private float LastControllerReelInput {
		get;
		set;
	}

	private float LastMouseReelInput {
		get;
		set;
	}

	private float CurrentKeyboardReelInput {
		get;
		set;
	}

	private float AverageKeyboardReelInput {
		get {
			if (this.ReelInputs.Count > 0) {
				float total = 0;
				for (int i = 0; i < this.ReelInputs.Count; i++) {
					total += this.ReelInputs[i];
				}
				return total / this.ReelInputs.Count;
			} else {
				return 0;
			}
		}
	}

	[field: SerializeField]
	private List<float> ReelInputs {
		get;
		set;
	} = new List<float>();

	private Vector2 PreviousRightStickInput {
		get;
		set;
	}

	private float CurrentMouseReelResetRate {
        get;
        set;
    }
	private float CurrentControllerReelResetRate {
		get;
		set;
	}

	private float CurrentMouseCompleteReelResetTime {
		get;
		set;
	}
	private float CurrentControllerCompleteReelResetTime {
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
		if(GameManager.Instance.CurrentLevel >= 1) {
			this.StrafingEnabled = true;
		}
		this.DisableThrowing = false;
		reelState = ReelState.notReeling;
		VoiceOverManager.Instance.PlayReelingTutorial();
		playerArms.BeginReel();
	}

	public void MoveHorizontalMouse(InputAction.CallbackContext context) {
        this.HorizontalInput = context.ReadValue<Vector2>() * mouseHorizontalSpeed;
    }

    public void ReelMouse(InputAction.CallbackContext context) {
		if(reelState == ReelState.reelingLocked) {
			return;
        }
        this.MouseReelInput -= context.ReadValue<Vector2>().y;
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

	public void ReelController(InputAction.CallbackContext context) {
		Vector2 currentStick = context.ReadValue<Vector2>();
		if (currentStick.magnitude < 0.5f) {
			return;
		}
		Vector2 from = this.PreviousRightStickInput.normalized;
		Vector2 to = currentStick.normalized;
		float angle = Vector2.SignedAngle(from, to);
		float rotationMagnitude = Mathf.Abs(angle);

		if (angle > 0) {
			this.ControllerReelInput = Mathf.Clamp(this.ControllerReelInput + rotationMagnitude / 360f, 0, maxControllerReelRate);
		}

		this.PreviousRightStickInput = currentStick;
		StartCoroutine(SetLastReelInput());
	}

	public void MoveHorizontalController(InputAction.CallbackContext context) {
		this.HorizontalInput = context.ReadValue<Vector2>() * controllerHorizontalSpeed;
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

		if (this.LastMouseReelInput > this.MouseReelInput) {
			if (this.CurrentMouseCompleteReelResetTime < mouseCompleteReelResetTime) {
				this.CurrentMouseCompleteReelResetTime += Time.deltaTime;
			} else {
				this.CurrentMouseCompleteReelResetTime = 0;
				this.MouseReelInput = 0;
			}
		} else {
			this.CurrentMouseCompleteReelResetTime = 0;
		}

		if (this.LastControllerReelInput > this.ControllerReelInput) {
			if (this.CurrentControllerCompleteReelResetTime <controllerCompleteReelResetTime) {
				this.CurrentControllerCompleteReelResetTime += Time.deltaTime;
			} else {
				this.CurrentControllerCompleteReelResetTime = 0;
				this.ControllerReelInput = 0;
			}
		} else {
			this.CurrentControllerCompleteReelResetTime = 0;
		}
	}

	private void ReduceReelRate() {
		if (this.CurrentMouseReelResetRate < mouseReelResetRate) {
			this.CurrentMouseReelResetRate += Time.deltaTime;
		} else {
			this.CurrentMouseReelResetRate = 0;
			this.MouseReelInput = Mathf.Clamp(this.MouseReelInput - 1, 0, maxMouseReelRate);
		}

		if (this.CurrentControllerReelResetRate < controllerReelResetRate) {
			this.CurrentControllerReelResetRate += Time.deltaTime;
		} else {
			this.CurrentControllerReelResetRate = 0;
			this.ControllerReelInput = Mathf.Clamp(this.ControllerReelInput - 1, 0, maxControllerReelRate);
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

		if (this.MouseReelInput == 0 && this.ControllerReelInput == 0) {
			if(this.ControllerReelInput > 0) {
				return;
			}
			reelState = ReelState.notReeling;
			SetReelAudio(null);
		}

		// Mouse
		if (this.MouseReelInput < mouseReelStateThresholds[0] && this.MouseReelInput > 0) {
			reelState = ReelState.calmReeling;
			SetReelAudio(reelAudio[0]);
		}
		if (this.MouseReelInput < mouseReelStateThresholds[1] && this.MouseReelInput > mouseReelStateThresholds[0]) {
			reelState = ReelState.normalReeling;
			SetReelAudio(reelAudio[1]);
		}
		if (this.MouseReelInput > mouseReelStateThresholds[1]) {
				reelState = ReelState.fastReeling;
			SetReelAudio(reelAudio[2]);
		}

		//Controller
		if (this.ControllerReelInput < controllerReelStateThresholds[0] && this.ControllerReelInput > 0) {
			reelState = ReelState.calmReeling;
			SetReelAudio(reelAudio[0]);
		}
		if (this.ControllerReelInput < controllerReelStateThresholds[1] && this.ControllerReelInput > controllerReelStateThresholds[0]) {
			reelState = ReelState.normalReeling;
			SetReelAudio(reelAudio[1]);
		}
		if (this.ControllerReelInput > controllerReelStateThresholds[1]) {
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
		this.LastMouseReelInput = this.MouseReelInput;
		this.LastControllerReelInput = this.ControllerReelInput;
	}

	#endregion

}
