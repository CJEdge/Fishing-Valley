using System.Collections;
using UnityEngine;

public class Seagull : MonoBehaviour
{
	private float WarningTime { get; set; }

    public void Initialize(float intervalRate, float warningTime) {
		this.WarningTime = warningTime;
		InvokeRepeating("SeagullAttack", intervalRate, intervalRate );
	}

	private void SeagullAttack() {
		StartCoroutine(PerformSeagullAttack());
	}

	private IEnumerator PerformSeagullAttack() {
		for (int i = 0; i < 3; i++) {
			AudioManager.Instance.PlayOneShot(FMODManager.Instance.SeagullWarning);
			yield return new WaitForSeconds(this.WarningTime);
		}
		AudioManager.Instance.PlayOneShot(FMODManager.Instance.SeagullAttack);
		GameManager.Instance.CurrentFish.SeagullAttack();
	} 
}
