using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Fish : AbstractState<Fish.State>
{

    #region States

    public enum State {
        Default,
        OnHook,
        Caught,
        Escaped
    }


    public enum ActivityLevel {
        none,
        calm,
        medium,
        active
    }

	[field:SerializeField]
    public ActivityLevel CurrentActivityLevel {
		get;
		set;
	}

	public List<ActivityLevel> ActivityLevels;

    public enum MovementDirection {
        none,
        left,
        right
    }

	public List<MovementDirection> movementDirections;

	#endregion


	#region Serialized Fields

	[SerializeField]
	private string fishName;

	[SerializeField]
	private int fishIndex;

	[SerializeField]
	private EventReference caughtVoiceLine;

	[SerializeField]
	private int sellPrice;

	[SerializeField]
	private bool failable;

	[SerializeField]
    private float fishSpeed;

	[SerializeField]
	private float centreThreshold;

	[SerializeField]
	private float reelSpeed;

	[SerializeField]
	private float swimAwaySpeed;

	[SerializeField]
	private float strafedSwimAwaySpeed;

    [SerializeField]
    private Rigidbody rb;

	[SerializeField]
	private GameObject visuals;

	[SerializeField]
	private GameObject[] activityParticles;

	[SerializeField]
	private StudioEventEmitter activitySplashSFX;

	[SerializeField]
	private float reelStart;

	[SerializeField]
	private float reelEnd;

	[SerializeField]
	private float activityLevelChangeTime;

	[SerializeField]
	private float movementChangeTime;

	[SerializeField]
	private GameObject strafeAudio;

	[SerializeField]
	private Transform lineEnd;

	#endregion


	#region Properties

	public EventReference CaughtVoiceLine {
		get {
			return caughtVoiceLine;
		}
	}

	private bool IsStrafer {
		get;
		set;
	}

	private bool IsCentred {
		get;
		set;
	}

	private InputController InputController {
		get {
			return GameManager.Instance.InputController;
		}
	}

	private float FailedCatchTime {
		get;
		set;
	}
	private bool CatchStarted {
		get;
		set;
	}

	private List<float> ActivityLevelIntervals {
		get;
		set;
	} = new List<float>();

	private List<float> MovementIntervals {
		get;
		set;
	} = new List<float>();

	[field:SerializeField]
	private int MovementIntervalsCompleted {
		get;
		set;
	}

	private float CurrentLevelActivityChangeTime {
		get;
		set;
	}

	private bool ActivityLevelChanging {
		get;
		set;
	}
	private float CurrentMovemetChangeTime {
		get;
		set;
	}

	private bool MovementDirectionChanging {
		get;
		set;
	}

	public StudioEventEmitter ActivitySplashSFX {
		get {
			return activitySplashSFX;
		}
	}

	public string FishName {
		get {
			return fishName;
		}
	}

	public int FishIndex {
		get {
			return fishIndex;
		}
	}

	public int SellPrice {
		get {
			return sellPrice;
		}
	}


	public Transform LineEnd {
		get {
			return lineEnd;
		}
	}

	public bool IsFailable {
		get;
		set;
	}

	public bool IsTutorial {
		get;
		set;
	}

	public Action<MovementDirection> Strafe {
		get;
		set;
	}

	public RodLineComponent RodLineComponent {
		get;
		set;
	}

	#endregion


	#region Mono Behaviours

	public override void Update() {
		HandleCentering();
		switch (this.CurrentState) {
			case State.Default:
				break;
			case State.OnHook:
				if(this.InputController.ReelSpeed > 0) {
					this.CatchStarted = true;
				}
				if (failable) {
					if (this.FailedCatchTime > 10) {
						FailedCatch();
					}
				}
				ChangeActivityLevels();
				ChangeMovementDirection();
				break;
			case State.Caught:
				SetActivityLevel(ActivityLevel.none);
				break;
			case State.Escaped:
				break;
		}
	}


    public void FixedUpdate() {
		if(this.CurrentState == State.OnHook) {
			Reel();
		}
		if (this.IsStrafer) {
			Move();
		}
    }
	#endregion


	#region Public Methods

	public void Initialize() {
		float activityLevelInterval = (reelStart - reelEnd) / this.ActivityLevels.Count;
		for (int i = 0; i < this.ActivityLevels.Count; i++) {
			this.ActivityLevelIntervals.Add(reelStart - (i * activityLevelInterval));
		}
		float movementInterval = (reelStart - reelEnd) / movementDirections.Count;
		bool isStrafer = false;
		for (int i = 0; i < movementDirections.Count; i++) {
			if(movementDirections[i]!= MovementDirection.none) {
				isStrafer = true;
			}
			this.MovementIntervals.Add(reelStart - (i * movementInterval));
		}
		this.IsStrafer = isStrafer;
		SetActivityLevel(this.ActivityLevels[0]);
		SetState(State.OnHook);
	}

	public void FishCaught() {
		GameManager.Instance.AssignNewCaughtFish(fishIndex);
		GameManager.Instance.LevelController.SetState(LevelController.State.FishCaught);
		AudioManager.Instance.PlayFishActivitySound(this, 0, true);
		VibrationManager.Instance.SetVibrationFrequency(true, 0, Mathf.Infinity);
		VibrationManager.Instance.SetVibrationFrequency(false, 0, Mathf.Infinity);
		Destroy(gameObject);
	}

	#endregion


	#region Private Methods

	private void Move() {
        rb.AddForce(this.InputController.HorizontalInput.x * fishSpeed, 0, 0);
    }

    private void Reel() {
		if (this.ActivityLevelChanging) {
			return;
		}
		if (this.MovementDirectionChanging) {
			return;
		}
        switch (CurrentActivityLevel) {
            case ActivityLevel.none:
                break;
            case ActivityLevel.calm:
				ReelingSuccesfully(this.InputController.CurrentState == InputController.State.CalmReeling);
				break;
            case ActivityLevel.medium:
				ReelingSuccesfully(this.InputController.CurrentState == InputController.State.NormalReeling);
				break;
            case ActivityLevel.active:
				ReelingSuccesfully(this.InputController.CurrentState == InputController.State.FastReeling);
				break;
        }
    }

	private void ReelingSuccesfully(bool reelingSuccesfully) {
		this.RodLineComponent.IsStraight = reelingSuccesfully;
		if (reelingSuccesfully) {
			this.RodLineComponent.CurveAmount = Mathf.MoveTowards(this.RodLineComponent.CurveAmount, 0f, Time.deltaTime);
			if (!this.IsCentred) {
				return;
			}
			VibrationManager.Instance.SetVibrationFrequency(true, DistanceAsPercentage(), Mathf.Infinity);
			VibrationManager.Instance.SetVibrationFrequency(false, 0, Mathf.Infinity);
			rb.AddForce(0, 0, -reelSpeed);
		} else {
			this.RodLineComponent.CurveAmount = Mathf.MoveTowards(this.RodLineComponent.CurveAmount, 1, Time.deltaTime);
			IncreaseFailTime();
			VibrationManager.Instance.SetVibrationFrequency(true, 0, Mathf.Infinity);
			if (this.IsCentred) {
				rb.AddForce(0, 0, swimAwaySpeed);
			} else {
				rb.AddForce(0, 0, strafedSwimAwaySpeed);
			}
		}
	}

	private void ChangeActivityLevels() {
		for (int i = this.ActivityLevelIntervals.Count - 1; i >= 0; i--) {
			if (transform.position.z < (this.ActivityLevelIntervals[i])) {
				if (this.CurrentActivityLevel != ActivityLevels[i]) {
					SetActivityLevel(ActivityLevels[i]);
					if (i != 0) {
						this.ActivityLevelChanging = true;
					}
				}
				break;
			}
		}
		if (this.ActivityLevelChanging) {
			if (this.CurrentLevelActivityChangeTime <= activityLevelChangeTime) {
				this.CurrentLevelActivityChangeTime += Time.deltaTime;
			} else {
				this.CurrentLevelActivityChangeTime = 0;
				this.ActivityLevelChanging = false;
			}
		}
	}

	private void ChangeMovementDirection() {
		for (int i = this.MovementIntervals.Count - 1; i >= 0; i--) {
			if (transform.position.z < (this.MovementIntervals[i])) {
				if (this.MovementIntervalsCompleted > i) {
					break;
				}
				if(movementDirections[i] != MovementDirection.none) {
					SetStrafePosition(movementDirections[i]);
					this.MovementDirectionChanging = true;
				}
				this.MovementIntervalsCompleted++;
			}
		}
		if (this.MovementDirectionChanging) {
			if (this.CurrentMovemetChangeTime <= movementChangeTime) {
				this.CurrentMovemetChangeTime += Time.deltaTime;
			} else {
				this.CurrentMovemetChangeTime = 0;
				this.MovementDirectionChanging = false;
			}
		}
	}

	private void HandleCentering() {
		if (transform.position.x > -centreThreshold && transform.position.x < centreThreshold) {
			this.IsCentred = true;
		} else {
			this.IsCentred = false;
		}
		if(strafeAudio.activeSelf == this.IsCentred) {
			strafeAudio.SetActive(!this.IsCentred);
		}
	}

	private void IncreaseFailTime() {
		if (!this.IsFailable) {
			return;
		}
		if (this.CatchStarted) {
			this.FailedCatchTime += Time.deltaTime;
		}
	}

	private void FailedCatch() {
		GameManager.Instance.LevelController.SetState(LevelController.State.AttatchBait);
		AudioManager.Instance.PlayFishActivitySound(this, 0, false);
		VibrationManager.Instance.SetVibrationFrequency(true, 0, Mathf.Infinity);
		VibrationManager.Instance.SetVibrationFrequency(false, 0, Mathf.Infinity);
		Destroy(gameObject);
	}

	private void SetActivityLevel(ActivityLevel activityLevel) {
		this.CurrentActivityLevel = activityLevel;
		for (int i = 0; i < activityParticles.Length; i++) {
			activityParticles[i].SetActive(false);
		}
		GameManager.Instance.InputController.ResetReelInput();
		switch (CurrentActivityLevel) {
			case ActivityLevel.none:
				AudioManager.Instance.PlayFishActivitySound(this, 0, true);
				break;
			case ActivityLevel.calm:
				activityParticles[0].SetActive(true);
				AudioManager.Instance.PlayFishActivitySound(this, 1, true);
				break;
			case ActivityLevel.medium:
				activityParticles[1].SetActive(true);
				AudioManager.Instance.PlayFishActivitySound(this, 2, true);
				break;
			case ActivityLevel.active:
				activityParticles[2].SetActive(true);
				AudioManager.Instance.PlayFishActivitySound(this, 3, true);
				break;
			default:
				break;
		}
	}

	private void SetStrafePosition(MovementDirection movementDirection) {
		switch (movementDirection) {
			case MovementDirection.none:
				break;
			case MovementDirection.left:
				Strafe?.Invoke(movementDirection);
				transform.position = new Vector3(GameManager.Instance.LevelController.LeftStrafeTransform.position.x,transform.position.y,transform.position.z);
				break;
			case MovementDirection.right:
				Strafe?.Invoke(movementDirection);
				transform.position = new Vector3(GameManager.Instance.LevelController.RightStrafeTransform.position.x, transform.position.y, transform.position.z);
				break;
			default:
				break;
		}
	}

	private float DistanceAsPercentage() {
		float totalLength = reelStart - reelEnd;
		float distanceAlongLength = transform.position.z - reelStart;
		return -(distanceAlongLength / totalLength)/2;
	}

	#endregion
}
