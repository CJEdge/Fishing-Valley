using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : Singleton<AudioManager>
{

	private EventInstance musicEventInstance;

	private List<EventInstance> eventInstances;

	public override void Awake() {
		base.Awake();
		eventInstances = new List<EventInstance>();
	}

	public void OnDestroy() {
		CleanUp();
	}

	public void PlayOneShot(EventReference sound, Vector3 position) {
		RuntimeManager.PlayOneShot(sound, position);
	}

	public EventInstance CreateInstance(EventReference eventReference) {
		EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
		eventInstances.Add(eventInstance);
		return eventInstance;
	}

	private void CleanUp() {
		foreach (EventInstance eventInstance in eventInstances) {
			eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			eventInstance.release();
		}
	}

	public void InitializeMusic(EventReference musicEventReference) {
		CleanUp();
		musicEventInstance = CreateInstance(musicEventReference);
		musicEventInstance.start();
	}

	public void SetMusicParameter(string name, float value) {
		musicEventInstance.setParameterByName(name, value);
	}
}
