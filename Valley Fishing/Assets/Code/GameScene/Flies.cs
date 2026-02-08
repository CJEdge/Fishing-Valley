using System.Collections;
using UnityEngine;

public class Flies : MonoBehaviour
{
	private enum LastSwingDirection {
		none,
		left,
		right
	}

	private LastSwingDirection lastSwingDirection;

	private bool Swarming { get; set; }
	private int SwingsLeft { get; set; }
	private float CurrentSwarmTime { get; set; }
	private float MaxSwarmTime { get; set; }
	private float IntervalRate { get; set; }

	public void Initialize(float intervalRate) {
		this.IntervalRate = intervalRate;
		StartCoroutine(PerformFlySwarm());
	}

	private IEnumerator PerformFlySwarm() {
		yield return new WaitForSeconds(this.IntervalRate);
		this.Swarming = true;
	}

	private void SwatFlies() {
		this.CurrentSwarmTime = 0;
		this.Swarming = false;
		StartCoroutine(PerformFlySwarm());
	}

	private void RegisterInput() {
		if (GameManager.Instance.InputController.HorizontalInput.x == 1) {
			if (lastSwingDirection == LastSwingDirection.right) {
				return;
			}
			lastSwingDirection = LastSwingDirection.right;
			this.SwingsLeft--;
		}
		if (GameManager.Instance.InputController.HorizontalInput.x == -1) {
			if (lastSwingDirection == LastSwingDirection.left) {
				return;
			}
			lastSwingDirection = LastSwingDirection.left;
			this.SwingsLeft--;
		}
		if (this.SwingsLeft == 0) {
			SwatFlies();
		}
	}

	public void Update() {
		if (!this.Swarming) {
			return;
		}
		if(this.CurrentSwarmTime < this.MaxSwarmTime) {
			this.CurrentSwarmTime += Time.deltaTime;
			RegisterInput();
		} else {
			this.CurrentSwarmTime = 0;
			this.Swarming = false;
		}
	}
}
