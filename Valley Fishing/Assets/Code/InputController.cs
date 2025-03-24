using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{

	#region States

	public enum ReelState {
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


    #endregion


    #region Properties

    public Vector2 MouseInput {
        get;
        set;
    }

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

    #endregion


    #region Mono Behaviours

    public void Update() {
		CheckToResetReelInput();
		ReduceReelRate();
		SetReelState();
	}

	#endregion


	#region Public Methods

	public void MoveRod(InputAction.CallbackContext context) {
        this.MouseInput = context.ReadValue<Vector2>() * rodSpeed;
    }

    public void Reel(InputAction.CallbackContext context) {
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
		} else if(this.ReelInput < reelStateThresholds[1]) {
			reelState = ReelState.calmReeling;
		} else if (this.ReelInput < reelStateThresholds[2]) {
			reelState = ReelState.normalReeling;
		} else{
			reelState = ReelState.fastReeling;
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
