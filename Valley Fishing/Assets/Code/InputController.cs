using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

	public ReelState lastReelState;

	#endregion


	#region Serialized Fields

    

	[SerializeField]
	private int keyboardInputListLength;

	

   

	[Header("Components")]

	[SerializeField]
	private PlayerInput playerInput;

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

	public Action OnClick {
		get;
		set;
	}

	public Action OnSkip {
		get;
		set;
	}

	public Action OnPause {
		get;
		set;
	}

	public Action OnReelStateChanged {
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

	[field:SerializeField]
	public bool DisableThrowing {
		get;
		set;
	}

	public int ReelSpeed {
		get;
		set;
	}	

	private bool IsSwitchingScenes {
		get;
		set;
	} = false;

	private bool CanSwitchScenes {
		get;
		set;
	} = false;

	#endregion


	#region Mono Behaviours

	public void Awake() {
		GameManager.Instance.InputController = this;
	}


	public void Start() {
		this.DisableThrowing = true;
		StartCoroutine(EnableSceneSwitching());
	}

	public void Update() {
		if(SceneManager.GetActiveScene().name == "Shop") {
			return;
		}
		SetKeyboardReeling();
		if (reelState == ReelState.reelingLocked) {
			if(lastReelState != ReelState.reelingLocked) {
				this.ReelSpeed = 0;
				OnReelStateChanged?.Invoke();
			}
			return;
		}
		SetReelState();
		CheckToResetReelInput();
		ReduceReelRate();
	}

	#endregion


	#region Public Methods

	public void Click(InputAction.CallbackContext context) {
		if (context.performed) {
			OnClick?.Invoke();
		}
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

		this.ControllerReelInput = Mathf.Clamp(this.ControllerReelInput + (rotationMagnitude / 360f), 0f, maxControllerReelRate);

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

	public void Pause(InputAction.CallbackContext context) {
		if (this.OnPause != null) {
			this.OnPause.Invoke();
		}
	}

	public void Shop(InputAction.CallbackContext context) {
		if (context.performed) {
			if (this.IsSwitchingScenes || !this.CanSwitchScenes) {
				return;
			}
			this.IsSwitchingScenes = true;
			if (SceneManager.GetActiveScene().name == "Shop") {
				SceneManager.LoadScene("Game");
			} else {
				SceneManager.LoadScene("Shop");
			}
		}
	}

	public void Skip(InputAction.CallbackContext context) {
		if (!context.performed) {
			return;
		}
		AudioManager.Instance.SkipVoiceOver();
		GameManager.Instance.LevelController.Skip();
		OnSkip?.Invoke();
	}

	#endregion


	#region Private Methods

	private IEnumerator EnableSceneSwitching() {
		yield return new WaitForSeconds(1f);
		this.CanSwitchScenes = true;
	}

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
		//if (this.CurrentKeyboardReelInput < keyboardReelResetTime) {
		//	this.CurrentKeyboardReelInput += Time.deltaTime;
		//} else {
		//	this.CurrentKeyboardReelInput = 0;
		//	this.ReelInputs.Clear();
		//}
	}

	private void SetReelState() {

		if (this.MouseReelInput == 0 && this.ControllerReelInput == 0) {
			reelState = ReelState.notReeling;
		}

		// Mouse
		if (this.MouseReelInput < mouseReelStateThresholds[0] && this.MouseReelInput > 0) {
			reelState = ReelState.calmReeling;
		}
		if (this.MouseReelInput < mouseReelStateThresholds[1] && this.MouseReelInput > mouseReelStateThresholds[0]) {
			reelState = ReelState.normalReeling;
		}
		if (this.MouseReelInput > mouseReelStateThresholds[1]) {
			reelState = ReelState.fastReeling;
		}

		//Controller
		if (this.ControllerReelInput < controllerReelStateThresholds[0] && this.ControllerReelInput > 0) {
			reelState = ReelState.calmReeling;
		}
		if (this.ControllerReelInput < controllerReelStateThresholds[1] && this.ControllerReelInput > controllerReelStateThresholds[0]) {
			reelState = ReelState.normalReeling;
		}
		if (this.ControllerReelInput > controllerReelStateThresholds[1]) {
			reelState = ReelState.fastReeling;
		}

		switch (reelState) {
			case ReelState.reelingLocked:
				this.ReelSpeed = 0;
				break;
			case ReelState.notReeling:
				this.ReelSpeed = 0;
				break;
			case ReelState.calmReeling:
				this.ReelSpeed = 1;
				break;
			case ReelState.normalReeling:
				this.ReelSpeed = 2;
				break;
			case ReelState.fastReeling:
				this.ReelSpeed = 3;
				break;
			default:
				break;
		}

		if(lastReelState != reelState) {
			this.OnReelStateChanged?.Invoke();
		}

		lastReelState = reelState;

		if (reelState == ReelState.notReeling) {
			return;
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
