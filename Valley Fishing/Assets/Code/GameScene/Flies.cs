using System.Collections;
using UnityEngine;

public class Flies : MonoBehaviour
{
	#region Enums

	private enum LastSwingDirection {
		none,
		left,
		right
	}

	private LastSwingDirection lastSwingDirection;

	#endregion


	#region Properties

	[field: SerializeField] private bool Swarming { get; set; }
	[field: SerializeField] private int SwingsToKill { get; set; }
	[field: SerializeField] private int CurrentSwingsLeft { get; set; }
	[field: SerializeField] private float CurrentSwarmTime { get; set; }
	[field: SerializeField] private float MaxSwarmTime { get; set; }
	[field: SerializeField] private float IntervalRate { get; set; }
	[field: SerializeField] private float FailTime { get; set; }


	#endregion


	#region Mono Behaviours

	public void Update() {
		if (!this.Swarming) {
			return;
		}
		if (this.CurrentSwarmTime < this.MaxSwarmTime) {
			this.CurrentSwarmTime += Time.deltaTime;
			RegisterInput();
		} else {
			this.CurrentSwarmTime = 0;
			this.Swarming = false;
			StartCoroutine(FailedFlies());
		}
	}

	public void OnDestroy() {
		AudioManager.Instance.PlayFliesSound(false);
	}

	#endregion


	#region Public Methods

	public void Initialize(float intervalRate, int swingsToKill, float maxSwarmTime, float failTime) {
		this.IntervalRate = intervalRate;
		this.SwingsToKill = swingsToKill;
		this.MaxSwarmTime = maxSwarmTime;
		this.FailTime = failTime;
		AudioManager.Instance.PlayFliesSound(false);
		StartCoroutine(PerformFlySwarm());
	}

	#endregion


	#region Private Methods

	private IEnumerator PerformFlySwarm() {
		yield return new WaitForSeconds(this.IntervalRate);
		AudioManager.Instance.PlayFliesSound(true);
		this.CurrentSwingsLeft = this.SwingsToKill;
		this.Swarming = true;
	}

	private void SwatFlies() {
		this.CurrentSwarmTime = 0;
		this.Swarming = false;
		AudioManager.Instance.PlayFliesSound(false);
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.FliesMiss);
		StartCoroutine(PerformFlySwarm());
	}

	private IEnumerator FailedFlies() {
		AudioManager.Instance.PlayFliesSound(false);
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.FliesAttack);
		GameManager.Instance.CurrentFish.HitByFlies = true;
		yield return new WaitForSeconds(this.FailTime);
		GameManager.Instance.CurrentFish.HitByFlies = false;
		StartCoroutine(PerformFlySwarm());
	}

	private void RegisterInput() {
		if (GameManager.Instance.InputController.HorizontalInput.x >= 0.8) {
			if (lastSwingDirection == LastSwingDirection.right) {
				return;
			}
			lastSwingDirection = LastSwingDirection.right;
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.Duck);
			this.CurrentSwingsLeft--;
		}
		if (GameManager.Instance.InputController.HorizontalInput.x <= -0.8) {
			if (lastSwingDirection == LastSwingDirection.left) {
				return;
			}
			lastSwingDirection = LastSwingDirection.left;
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.Duck);
			this.CurrentSwingsLeft--;
		}
		if (this.CurrentSwingsLeft == 0) {
			SwatFlies();
		}
	}

	#endregion
	
}
