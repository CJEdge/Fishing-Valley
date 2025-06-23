using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class VibrationManager : Singleton<VibrationManager>
{
	private Vector2 Vibrations {
		get;
		set;
	}

    public void Update()
    {
		Gamepad.current.SetMotorSpeeds(this.Vibrations.x, this.Vibrations.y);
    }

	public void OnDestroy() {
		Gamepad.current.SetMotorSpeeds(0, 0);
	}

	public void SetVibrationFrequency(bool high, float vibrationAmount, float duration) {
		if(duration == Mathf.Infinity) {
			if (high) {
				this.Vibrations = new Vector2(this.Vibrations.x, vibrationAmount);
			} else {
				this.Vibrations = new Vector2(vibrationAmount, this.Vibrations.y);
			}
		} else {
			StartCoroutine(RunHighFrequencyVibration(high, vibrationAmount, duration));
		}
	}

	private IEnumerator RunHighFrequencyVibration(bool high, float vibrationAmount, float duration){
		float elapsedTime = 0;
		if (high) {
			this.Vibrations = new Vector2(this.Vibrations.x, vibrationAmount);
		} else {
			this.Vibrations = new Vector2(vibrationAmount, this.Vibrations.y);
		}
		while (elapsedTime < duration) {
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		if (high) {
			this.Vibrations = new Vector2(this.Vibrations.x, 0);
		} else {
			this.Vibrations = new Vector2(0, this.Vibrations.y);
		}
	}
}
