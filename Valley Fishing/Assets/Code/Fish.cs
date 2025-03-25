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

    #endregion


    #region Serialized Fields

    [SerializeField]
    private string fishName;

	[SerializeField]
	private bool isStrafer;

	[SerializeField]
    private float fishSpeed;

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
    private Rigidbody rb;

    #endregion


    #region Properties

    private int MovementDirection {
        get;
        set;
    }

    private float CurrentStrafeTime {
        get;
        set;
    }

	private bool IsCentred {
		get {
			if (transform.position.x > -centreThreshold && transform.position.x < centreThreshold) {
				return true;
			} else {
				return false;
			}
		}
	}

	private InputController InputController {
		get {
			return GameManager.Instance.InputController;
		}
	}

	#endregion


	#region Mono Behaviours

	public void Update() {
        switch (fishState) {
            case FishState.none:
                break;
            case FishState.onHook:
                if (isStrafer) {
                    if (this.CurrentStrafeTime < strafeFrequency) {
                        this.CurrentStrafeTime += Time.deltaTime;
                    } else {
                        this.CurrentStrafeTime = 0;
                        int randomDirection = Random.Range(0, 2) * 2 - 1;
                        if (randomDirection == 1) {
                            Strafe(true);
                        } else {
                            Strafe(false);
                        }
                        AssignFishDirection(randomDirection);
                    }
                }
                break;
            case FishState.caught:
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

    public void StartFishing() {
        switch (activityLevel) {
            case ActivityLevel.none:
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

    public void HookFish() {
        fishState = FishState.onHook;
    }

    public void FishCaught() {
        VoiceOverManager.Instance.CaughtFishAudio = caughtAudios[Random.Range(0,caughtAudios.Count)];
        VoiceOverManager.Instance.PlayCaughtFish();
        GameManager.Instance.FishController.SpawnNewFish();
    }

    #endregion


    #region Private Methods

    private void AssignFishDirection(int fishDirection) {
        rb.linearVelocity = Vector3.zero;
        this.MovementDirection = fishDirection;
    }

    private void Strafe(bool strafeRight) {
        if (strafeRight) {
            transform.position = new Vector3(GameManager.Instance.RightFishTransform.position.x, transform.position.y, transform.position.z);
        } else {
            transform.position = new Vector3(GameManager.Instance.LeftFishTransform.position.x, transform.position.y, transform.position.z);
        }
    }

    private void Move() {
        rb.AddForce(this.InputController.MouseInput.x, 0, 0);
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

    #endregion
}
