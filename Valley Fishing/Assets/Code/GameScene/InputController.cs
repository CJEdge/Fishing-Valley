using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputController : AbstractState<InputController.State> {

	#region States

	public enum State {
		Default,
		ReelingLocked,
		NotReeling,
		CalmReeling,
		NormalReeling,
		FastReeling
	}

	protected override void EnterState(State state) {
		switch (state) {
			case State.Default:
				GameManager.Instance.InputController = this;
				this.InputTypes = new List<InputType> { mouse, controller };
				StartCoroutine(EnableSceneSwitching());
				SetState(State.ReelingLocked);
				break;
			case State.ReelingLocked:
				this.ReelLevel = 0;
				break;
			case State.NotReeling:
				this.ReelLevel = 0;
				break;
			case State.CalmReeling:
				this.ReelLevel = 1;
				break;
			case State.NormalReeling:
				this.ReelLevel = 2;
				break;
			case State.FastReeling:
				this.ReelLevel = 3;
				break;
			default:
				break;
		}
	}

	protected override void UpdateState(State state) {
		switch (state) {
			case State.ReelingLocked:
				break;
			case State.NotReeling:
				Reel();
				break;
			case State.CalmReeling:
				Reel();
				break;
			case State.NormalReeling:
				Reel();
				break;
			case State.FastReeling:
				Reel();
				break;
			default:
				break;
		}
	}

	#endregion


	#region Serialized Fields

	[Header("Variables")]

	[SerializeField]
	private Vector2 clickVibration;

	[SerializeField]
	private float reelCalculationTime;

	[Header("Components")]

	[SerializeField]
	private PlayerInput playerInput;

	[SerializeField]
	private GameObject blurObject;

	[SerializeField]
	private GameObject blackObject;

	[SerializeField]
	private PlayerArms playerArms;

	[SerializeField]
	private RodLineComponent rodLineComponent;

	[Header("ReelTypes")]

	[SerializeField] private InputType mouse = new InputType();

	[SerializeField] private InputType controller = new InputType();

	#endregion


	#region ReelType

	[System.Serializable]
	public class InputType {

		public List<float> reelStateThresholds;

		public float HorizontalSpeed { get; set; }

		public float ReelInput;

		public float LastReelInput { get; set; }

		public float CurrentReelResetRate { get; set; }

		public float CurrentCompleteReelResetTime { get; set; }

		public float ReelSpeed;
	}

	#endregion


	#region Properties

	private List<InputType> InputTypes { get; set; }

	public Action OnClick { get; set; }

	public Action OnSkip { get; set; }

	public Action OnPause { get; set; }

	public bool SelectionManuallySet { get; set; }

	public Vector2 HorizontalInput { get; set; }

	private Vector2 PreviousRightStickInput { get; set; }

	public bool StrafingEnabled { get; set; }

	public int ReelLevel { get; set; }

	private bool IsSwitchingScenes { get;set; } = false;

	private bool CanSwitchScenes { get; set; } = false;

	public float CurrentReelResetTime { get; set; }

	#endregion


	#region Public Methods

	public void Click(InputAction.CallbackContext context) {
		if (context.performed) {
			OnClick?.Invoke();
		}
	}

	public void MoveHorizontalMouse(InputAction.CallbackContext context) {
		this.HorizontalInput = context.ReadValue<Vector2>() * mouse.HorizontalSpeed;
	}

	public void ReelMouse(InputAction.CallbackContext context) {
		if (this.CurrentState == State.ReelingLocked) {
			return;
		}
		mouse.ReelInput -= context.ReadValue<Vector2>().y;
		StartCoroutine(SetLastReelInput());
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

		controller.ReelInput = controller.ReelInput + (rotationMagnitude / 360f);

		this.PreviousRightStickInput = currentStick;
		//TODO set previous magnitude
		StartCoroutine(SetLastReelInput());
	}

	public void MoveHorizontalController(InputAction.CallbackContext context) {
		this.HorizontalInput = context.ReadValue<Vector2>() * controller.HorizontalSpeed;
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
		if (context.performed) {
			AudioManager.Instance.SkipVoiceOver();
			OnSkip?.Invoke();
		}
	}

	public void SelectButton(GameObject button) {
		this.SelectionManuallySet = true;
		GameManager.Instance.EventSystem.SetSelectedGameObject(button);
	}

	public void ResetReelInput() {
		for (int i = 0; i < this.InputTypes.Count; i++) {
			this.InputTypes[i].ReelInput = 0;
		}
	}

	#endregion


	#region Private Methods

	private IEnumerator EnableSceneSwitching() {
		yield return new WaitForSeconds(1f);
		this.CanSwitchScenes = true;
	}

	private void Reel() {
		SetReelState();
		if(CurrentReelResetTime < reelCalculationTime) {
			CurrentReelResetTime += Time.deltaTime;
		} else {
			CurrentReelResetTime = 0;
			controller.ReelSpeed = controller.ReelInput / reelCalculationTime;
			controller.ReelInput = 0;
		}
	}

	private void SetReelState() {
		bool allInputTypesNotReeling = true;
		for (int i = 0; i < this.InputTypes.Count; i++) {
			if(controller.ReelSpeed > 0) {
				allInputTypesNotReeling = false;
			}
			if (controller.ReelSpeed < controller.reelStateThresholds[0] && controller.ReelInput > 0) {
				SetState(State.CalmReeling);
			}
			if (controller.ReelSpeed < controller.reelStateThresholds[1] && controller.ReelInput > controller.reelStateThresholds[0]) {
				SetState(State.NormalReeling);
			}
			if (controller.ReelSpeed > controller.reelStateThresholds[1]) {
				SetState(State.FastReeling);
			}
		}
		if (allInputTypesNotReeling) {
			SetState(State.NotReeling);
		}
	}

	#endregion


	#region Coroutines

	private IEnumerator SetLastReelInput() {
		yield return new WaitForEndOfFrame();
		for (int i = 0; i < this.InputTypes.Count; i++) {
			this.InputTypes[i].LastReelInput = this.InputTypes[i].ReelInput;
		}
	}

	#endregion
}
