using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class Fish : AbstractState<Fish.State>
{

    #region States

    public enum State {
        none,
        onHook,
        caught,
        escaped
    }


    public enum ActivityLevel {
        none,
        calm,
        medium,
        active
    }

    public ActivityLevel activityLevel;

    public enum MovementDirection {
        random,
        left,
        right
    }

    public MovementDirection movementDirection;

    #endregion


    #region Serialized Fields

	[SerializeField]
	private bool isStrafer;

	[SerializeField]
    private float fishSpeed;

    [SerializeField]
    private int strafeCount;

    [SerializeField]
    private float strafeFrequency;

	[SerializeField]
	private float centreThreshold;

	[SerializeField]
	private float reelSpeed;

	[SerializeField]
	private float swimAwaySpeed;

    [SerializeField]
    private Rigidbody rb;

	[SerializeField]
	private GameObject visuals;

	[SerializeField]
	private StudioEventEmitter activitySplashSFX;

    #endregion


    #region Properties

    private int CurrentStrafeCount {
        get;
        set;
    }

    private float CurrentStrafeTime {
        get;
        set;
    }

	private bool IsCentred {
		get;
		set;
	}

	private bool LastIsCentred {
		get;
		set;
	}

	private InputController InputController {
		get {
			return GameManager.Instance.InputController;
		}
	}

	[field:SerializeField]
	private float FailedCatchTime {
		get;
		set;
	}

	[field:SerializeField]
	private bool CatchStarted {
		get;
		set;
	}

	public StudioEventEmitter ActivitySplashSFX {
		get {
			return activitySplashSFX;
		}
	}

	#endregion


	#region Mono Behaviours

	public void Start() {
		SetState(State.onHook);
		switch (activityLevel) {
			case ActivityLevel.none:
				AudioManager.Instance.PlayFishActivitySound(this, 0, true);
				break;
			case ActivityLevel.calm:
				AudioManager.Instance.PlayFishActivitySound(this, 1, true);
				break;
			case ActivityLevel.medium:
				AudioManager.Instance.PlayFishActivitySound(this, 2, true);
				break;
			case ActivityLevel.active:
				AudioManager.Instance.PlayFishActivitySound(this, 3, true);
				break;
			default:
				break;
		}
	}

	public override void Update() {
		HandleCentering();
		switch (this.CurrentState) {
			case State.none:
				break;
			case State.onHook:
				if(this.InputController.ReelSpeed > 0) {
					this.CatchStarted = true;
				}
				if (this.FailedCatchTime > 10) {
					FailedCatch();
				}
				if (strafeCount == 0) {
					return;
				}
				if (this.CurrentStrafeCount == strafeCount) {
					return;
				}
				if (isStrafer) {
					if (this.CurrentStrafeTime < strafeFrequency) {
						this.CurrentStrafeTime += Time.deltaTime;
					} else {
						this.CurrentStrafeTime = 0;
						switch (movementDirection) {
							case MovementDirection.random:
								break;
							case MovementDirection.left:
								break;
							case MovementDirection.right:
								break;
							default:
								break;
						}
						this.CurrentStrafeCount++;
					}
				}
				break;
			case State.caught:
				activityLevel = ActivityLevel.none;
				break;
			case State.escaped:
				break;
		}
	}


    public void FixedUpdate() {
		if(this.CurrentState == State.onHook) {
			Reel();
		}
		if (GameManager.Instance.InputController.StrafingEnabled) {
			Move();
		}
    }
    #endregion


    #region Public Methods

	public void EnableVisuals(bool enable) {
		visuals.SetActive(enable);
	}

	public void FishCaught() {
		GameManager.Instance.LevelController.SetState(LevelController.State.FishCaught);
		AudioManager.Instance.PlayFishActivitySound(this, 1, false);
		Destroy(gameObject);
	}

	#endregion


	#region Private Methods

	private void Move() {
        rb.AddForce(this.InputController.HorizontalInput.x * fishSpeed, 0, 0);
    }

    private void Reel() {
        switch (activityLevel) {
            case ActivityLevel.none:
                break;
            case ActivityLevel.calm:
				if (this.InputController.reelState == InputController.ReelState.calmReeling && this.IsCentred) {
					rb.AddForce(0, 0, -reelSpeed);
				} else {
					if (this.CatchStarted) {
						this.FailedCatchTime += Time.deltaTime;
					}
					rb.AddForce(0, 0, swimAwaySpeed);
				}
				break;
            case ActivityLevel.medium:
				if (this.InputController.reelState == InputController.ReelState.normalReeling && this.IsCentred) {
					rb.AddForce(0, 0, -reelSpeed);
				} else {
					if (this.CatchStarted) {
						this.FailedCatchTime += Time.deltaTime;
					}
					rb.AddForce(0, 0, swimAwaySpeed);
				}
				break;
            case ActivityLevel.active:
				if (this.InputController.reelState == InputController.ReelState.fastReeling && this.IsCentred) {
					rb.AddForce(0, 0, -reelSpeed);
				} else {
					if (this.CatchStarted) {
						this.FailedCatchTime += Time.deltaTime;
					}
					rb.AddForce(0, 0, swimAwaySpeed);
				}
				break;
        }
    }

	private void HandleCentering() {
		if (transform.position.x > -centreThreshold && transform.position.x < centreThreshold) {
			this.IsCentred = true;
		} else {
			this.IsCentred = false;
		}
		if (this.IsCentred != this.LastIsCentred && !this.IsCentred) {
		}
		if (this.IsCentred) {
		}
		this.LastIsCentred = this.IsCentred;
	}

	private void FailedCatch() {
		GameManager.Instance.LevelController.SetState(LevelController.State.AttatchBait);
		AudioManager.Instance.PlayFishActivitySound(this, 0, false);
		Destroy(gameObject);
	}

	#endregion
}
