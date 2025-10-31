using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	[SerializeField]
	private List<EventReference> list;

	public void Start() {
		AudioManager.Instance.PlayVoiceOverChain(list);
	}
}
