using FMOD.Studio;
using FMODUnity;
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

	[SerializeField]
	private List<ActivityLevel> activityLevels;

    public enum MovementDirection {
        random,
        left,
        right
    }

    public MovementDirection movementDirection;

	#endregion


	#region Serialized Fields

	[SerializeField]
	private string fishName;

	[SerializeField]
	private int fishIndex;

	[SerializeField]
	private int sellPrice;

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
	private GameObject[] activityParticles;

	[SerializeField]
	private StudioEventEmitter activitySplashSFX;

	[SerializeField]
	private float reelStart;

	[SerializeField]
	private float reelEnd;

	[SerializeField]
	private float activityLevelChangeTime;

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

	[field: SerializeField]
	private float CurrentLevelActivityChangeTime {
		get;
		set;
	}

	[field: SerializeField]
	private bool ActivityLevelChanging {
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

	public bool IsFailable {
		get;
		set;
	}

	public bool IsTutorial {
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
				if (this.FailedCatchTime > 10) {
					FailedCatch();
				}
				for (int i = this.ActivityLevelIntervals.Count - 1; i >= 0 ; i--) {
					if(transform.position.z < (this.ActivityLevelIntervals[i])){
						if (this.CurrentActivityLevel != activityLevels[i]) {
							SetActivityLevel(activityLevels[i]);
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
		if (GameManager.Instance.InputController.StrafingEnabled) {
			Move();
		}
    }
	#endregion


	#region Public Methods

	public void Initialize() {
		float activityLevelInterval = (reelStart - reelEnd) / activityLevels.Count;
		for (int i = 0; i < activityLevels.Count; i++) {
			this.ActivityLevelIntervals.Add(reelStart - (i * activityLevelInterval));
		}
		SetActivityLevel(activityLevels[0]);
		SetState(State.OnHook);
	}

	public void FishCaught() {
		GameManager.Instance.AssignNewCaughtFish(fishIndex);
		GameManager.Instance.LevelController.SetState(LevelController.State.FishCaught);
		AudioManager.Instance.PlayFishActivitySound(this, 0, true);
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
        switch (CurrentActivityLevel) {
            case ActivityLevel.none:
                break;
            case ActivityLevel.calm:
				if (this.InputController.CurrentState == InputController.State.CalmReeling && this.IsCentred) {
					rb.AddForce(0, 0, -reelSpeed);
				} else {
					IncreaseFailTime();
					rb.AddForce(0, 0, swimAwaySpeed);
				}
				break;
            case ActivityLevel.medium:
				if (this.InputController.CurrentState == InputController.State.NormalReeling && this.IsCentred) {
					rb.AddForce(0, 0, -reelSpeed);
				} else {
					IncreaseFailTime();
					rb.AddForce(0, 0, swimAwaySpeed);
				}
				break;
            case ActivityLevel.active:
				if (this.InputController.CurrentState == InputController.State.FastReeling && this.IsCentred) {
					rb.AddForce(0, 0, -reelSpeed);
				} else {
					IncreaseFailTime();
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
		Destroy(gameObject);
	}

	private void SetActivityLevel(ActivityLevel activityLevel) {
		this.CurrentActivityLevel = activityLevel;
		for (int i = 0; i < activityParticles.Length; i++) {
			activityParticles[i].SetActive(false);
		}
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

	#endregion
}
