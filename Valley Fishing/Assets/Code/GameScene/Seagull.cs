using System.Collections;
using UnityEngine;

public class Seagull : MonoBehaviour
{
	private float WarningTime { get; set; }

    public void Initialize(float intervalRate, float warningTime) {
		this.WarningTime = warningTime;
		InvokeRepeating("SeagullAttack", intervalRate, intervalRate + 4 * this.WarningTime);
	}

	private void SeagullAttack() {
		StartCoroutine(PerformSeagullAttack());
	}

	private IEnumerator PerformSeagullAttack() {
		for (int i = 0; i < 3; i++) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.SeagullWarning);
			yield return new WaitForSeconds(this.WarningTime);
		}
		GameManager.Instance.EventController.SeagullAttack();
	} 
}
