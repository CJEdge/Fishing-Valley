using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class ListenEventSO : ScriptableObject {

	public Action OnEventTriggered { get; set; }

	public virtual void RunEvent(MonoBehaviour runner) {
		runner.StartCoroutine(StartListening());
	}

	public virtual IEnumerator StartListening() {
		yield return null;
	}
}
