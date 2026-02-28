using System.Collections;
using UnityEngine;

public class EventController : MonoBehaviour
{
	#region Seagull

	[Header("Seagull")]

	[SerializeField] private Seagull seagull;
	[SerializeField] private float seagullIntervalTime;
	[SerializeField] private float seagullWarningTime;
	[SerializeField] private float seagullFailTime;
	[SerializeField] private float duckingTime;
	[SerializeField] private float duckCooldown;

	[field: SerializeField] public float SeagullAvoidanceThreshold;
	public bool Ducking { get; set; }
	private bool CanDuck { get; set; } = true;
	private Seagull CurrentSeagull { get; set; }
	
	public void Duck() {
		if (!this.CanDuck) {
			return;
		}
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.Duck);
		StartCoroutine(PerformDuck());
	}

	private IEnumerator PerformDuck() {
		this.Ducking = true;
		this.CanDuck = false;
		yield return new WaitForSeconds(duckingTime);
		this.Ducking = false;
		yield return new WaitForSeconds(duckCooldown);
		this.CanDuck = true;
	}

	#endregion


	#region Flies

	[Header("Flies")]

	[SerializeField] private Flies flies;
	[SerializeField] private float fliesIntervalTime;
	[SerializeField] private int fliesToSwat;
	[SerializeField] private float fliesEffectTime;
	[SerializeField] private float fliesFailTime;
	private Flies CurrentFlies { get; set; }

	#endregion


	#region Mono Behaviours

	public void Start() {
		GameManager.Instance.EventController = this;
	}

	#endregion


	#region Public Methods

	public void NewFishSpawned() {
		if (seagullIntervalTime != 0) {
			Seagull seagullInstance = Instantiate(seagull);
			seagullInstance.Initialize(seagullIntervalTime, seagullWarningTime, seagullFailTime);
		}
		if (fliesIntervalTime != 0) {
			Flies fliesInstance = Instantiate(flies);
			fliesInstance.Initialize(fliesIntervalTime, fliesToSwat, fliesEffectTime, fliesFailTime);
			this.CurrentFlies = fliesInstance;
		}
	}

	public void FishCaught() {
		if (this.CurrentFlies != null) {
			Destroy(this.CurrentFlies);
		}
		if (this.CurrentSeagull != null) {
			Destroy(this.CurrentSeagull);
		}
	}

	#endregion

}
