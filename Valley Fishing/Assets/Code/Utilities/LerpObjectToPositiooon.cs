using System.Collections;
using UnityEngine;

public class LerpObjectToPosition : MonoBehaviour
{
	[SerializeField] private string lerpName;
	[SerializeField] private float lerpDuration;
	[SerializeField] private Transform destination;

	public void BeginLerp() {
		StartCoroutine(RunBeginLerp());
	}

	private IEnumerator RunBeginLerp() {
		float elapsedTime = 0;
		while (elapsedTime < lerpDuration) {
			transform.position = Vector3.Lerp(transform.position, destination.position, (elapsedTime / lerpDuration));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
	}
}
