using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{

    #region States

    public enum FishState {
        none,
        onHook,
        caught,
        escaped
    }

    public FishState fishState;


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
    private string fishName;

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
	private List<GameObject> reelAudioSources;

    [SerializeField]
    private List<AudioSource> caughtAudios;

	[SerializeField]
	private AudioSource strafeAudio;

    [SerializeField]
    private Rigidbody rb;

	[SerializeField]
	private GameObject visuals;

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

    public float CaughtFishTime {
        get {
            return caughtAudios[0].clip.length;
        }
    }

	#endregion


	#region Mono Behaviours

	public void Update() {
		HandleCentering();
		switch (fishState) {
            case FishState.none:
                break;
            case FishState.onHook:
				if (strafeCount == 0) {
                    return;
                }
                if(this.CurrentStrafeCount == strafeCount) {
                    return;
                }
                if (isStrafer) {
                    if (this.CurrentStrafeTime < strafeFrequency) {
                        this.CurrentStrafeTime += Time.deltaTime;
                    } else {
                        this.CurrentStrafeTime = 0;
                        switch (movementDirection) {
                            case MovementDirection.random:
                                int randomDirection = Random.Range(0, 2) * 2 - 1;
                                if (randomDirection == 1) {
                                    Strafe(true);
                                } else {
                                    Strafe(false);
                                }
                                break;
                            case MovementDirection.left:
								Strafe(false);
								break;
                            case MovementDirection.right:
								Strafe(true);
								break;
                            default:
                                break;
                        }
                        this.CurrentStrafeCount++;
                    }
                }
                break;
            case FishState.caught:
				activityLevel = ActivityLevel.none;
				PlayActivitySFX();
				break;
            case FishState.escaped:
                break;
        }
    }


    public void FixedUpdate() {
		if(fishState == FishState.onHook) {
			Reel();
		}
		if (GameManager.Instance.InputController.StrafingEnabled) {
			Move();
		}
    }
    #endregion


    #region Public Methods

    public void PlayActivitySFX() {
        switch (activityLevel) {
            case ActivityLevel.none:
				for (int i = 0; i < reelAudioSources.Count; i++) {
					reelAudioSources[i].SetActive(false);
				}
                break;
            case ActivityLevel.calm:
                reelAudioSources[0].SetActive(true);
                break;
            case ActivityLevel.medium:
                reelAudioSources[1].SetActive(true);
                break;
            case ActivityLevel.active:
                reelAudioSources[2].SetActive(true);
                break;
		}
    }

	public void EnableVisuals(bool enable) {
		visuals.SetActive(enable);
	}

    public void HookFish() {
        fishState = FishState.onHook;
		PlayActivitySFX();
	}

    public void FishCaught() {
		fishState = FishState.caught;
          //      GameManager.Instance.FishController.DisplayCaughtFish(fishName);
		//GameManager.Instance.InputController.reelState = InputController.ReelState.reelingLocked;
		//if (GameManager.Instance.CurrentLevel == 2) {
		//	GameManager.Instance.InputController.StrafingEnabled = false;
		//}
	}

    #endregion


    #region Private Methods

    private void Strafe(bool strafeRight) {
		strafeAudio.Play();
		if (strafeRight) {
            //transform.position = new Vector3(GameManager.Instance.RightFishTransform.position.x, transform.position.y, transform.position.z);
        } else {
            //transform.position = new Vector3(GameManager.Instance.LeftFishTransform.position.x, transform.position.y, transform.position.z);
        }
    }

    private void Move() {
		Debug.Log(InputController.HorizontalInput);
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
					rb.AddForce(0, 0, swimAwaySpeed);
				}
				break;
            case ActivityLevel.medium:
				if (this.InputController.reelState == InputController.ReelState.normalReeling && this.IsCentred) {
					rb.AddForce(0, 0, -reelSpeed);
				} else {
					rb.AddForce(0, 0, swimAwaySpeed);
				}
				break;
            case ActivityLevel.active:
				if(this.InputController.reelState == InputController.ReelState.fastReeling && this.IsCentred) {
					rb.AddForce(0, 0, -reelSpeed);
				} else {
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
			reelAudioSources[3].SetActive(true);
		}
		if (this.IsCentred) {
			reelAudioSources[3].SetActive(false);
		}
		this.LastIsCentred = this.IsCentred;
	}

	#endregion
}
