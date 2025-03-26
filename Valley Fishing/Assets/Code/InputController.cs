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
    private float reelResetRate;

	[SerializeField]
	private float completeReelResetTime;

    [SerializeField]
    private float maxReelRate;

	[SerializeField]
	private List<float> reelStateThresholds;

	[SerializeField]
	private AudioRandomizer[] reelAudio;

	[SerializeField]
	private PlayerArms playerArms;


    #endregion


    #region Properties

	[field:SerializeField]
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

    #endregion


    #region Mono Behaviours

    public void Update() {
		if(reelState == ReelState.reelingLocked) {
			return;
        }
		CheckToResetReelInput();
		ReduceReelRate();
		SetReelState();
	}

	#endregion


	#region Public Methods

	public void Click(InputAction.CallbackContext context) {
        if (context.performed) {
			this.ClickTrigger = true;
			CastRod();

		} else {
			this.ClickTrigger = false;
        }
	}

	public void CastRod() {
		if(VoiceOverManager.Instance.voiceOverstate == VoiceOverManager.VoiceOverState.castRodTutorial ||
			VoiceOverManager.Instance.voiceOverstate == VoiceOverManager.VoiceOverState.castRod) {
			playerArms.ThrowRod();
		}	
	}

	public void BeginReel() {
		reelState = ReelState.notReeling;
		VoiceOverManager.Instance.PlayReelingTutorial();
		playerArms.BeginReel();
	}

	public void MoveRod(InputAction.CallbackContext context) {
        this.MouseInput = context.ReadValue<Vector2>() * rodSpeed;
    }

    public void Reel(InputAction.CallbackContext context) {
		if(reelState == ReelState.reelingLocked) {
			return;
        }
        this.ReelInput -= context.ReadValue<Vector2>().y;
		StartCoroutine(SetLastReelInput());

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

	private void SetReelState() {
		if(this.ReelInput == reelStateThresholds[0]) {
			reelState = ReelState.notReeling;
			SetReelAudio(null);
		} else if(this.ReelInput < reelStateThresholds[1]) {
			reelState = ReelState.calmReeling;
			SetReelAudio(reelAudio[0]);
		} else if (this.ReelInput < reelStateThresholds[2]) {
			reelState = ReelState.normalReeling;
			SetReelAudio(reelAudio[1]);
		} else{
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
