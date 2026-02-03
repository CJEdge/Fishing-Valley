using System.Collections;
using UnityEngine;

public class EventController : MonoBehaviour
{
	#region Serialized Fields

	[SerializeField] private Seagull seagull;
	[SerializeField] private float seagullIntervalTime;
	[SerializeField] private float seagullWarningTime;
	[SerializeField] private float seagullFailTime;
	[SerializeField] private float duckingTime;
	[SerializeField] private float duckCooldown;
	[SerializeField] private float avoidanceThreshold;

	#endregion


	#region Properties

	[field: SerializeField] private bool Ducking { get; set; }
	[field: SerializeField] private bool CanDuck { get; set; } = true;

	#endregion


	#region Mono Behaviours

	public void Start() {
		GameManager.Instance.EventController = this;
	}

	#endregion


	#region Public Methods

	public void Duck() {
		if (!this.CanDuck) {
			return;
		}
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.Duck);
		StartCoroutine(PerformDuck());
	}

	public void SeagullAttack() {
		StartCoroutine(PerformSeagullAttack());
	}

	#endregion


	#region Private Methods

	public void NewFishSpawned() {
		if (seagullIntervalTime != 0) {
			Seagull seagullInstance = Instantiate(seagull);
			seagullInstance.Initialize(seagullIntervalTime, seagullWarningTime);
		}
	}

	private IEnumerator PerformDuck() {
		this.Ducking = true;
		this.CanDuck = false;
		yield return new WaitForSeconds(duckingTime);
		this.Ducking = false;
		yield return new WaitForSeconds(duckCooldown);
		this.CanDuck = true;
	}

	private IEnumerator PerformSeagullAttack() {
		float currentTime = 0;
		while (currentTime < avoidanceThreshold) {
			currentTime += Time.deltaTime;
			yield return new();
			if (this.Ducking) {
				AudioManager.Instance.PlayOneShot(FMODManager.Instance.SeagullMiss);
				yield break;
			}
		}
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.SeagullAttack);
		GameManager.Instance.CurrentFish.HitBySeagull = true;
		yield return new WaitForSeconds(seagullFailTime);
		GameManager.Instance.CurrentFish.HitBySeagull = false;
	}

	#endregion

}
