using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System;

public class AudioManager : Singleton<AudioManager>
{

	#region Properties

	public EventInstance MusicEventInstance {
		get;
		set;
	}

	public EventInstance VoiceLineEventInstance {
		get;
		set;
	}

	public EventInstance LastVoiceLineEventInstance {
		get;
		set;
	}

	public EventInstance CurrentReelInstance {
		get;
		set;
	}

	private List<EventInstance> eventInstances {
		get;
		set;
	} = new List<EventInstance>();

	public Action<EventInstance> VoiceLineOver {
		get;
		set;
	}

	#endregion


	#region Mono Behaviours

	public void OnDestroy() {
		CleanUp();
	}

	#endregion


	#region Public Methods
	public void PlayOneShot(EventReference sound, Vector3 position) {
		RuntimeManager.PlayOneShot(sound, position);
	}

	public void PlayVoiceOver(EventReference voiceLineReference) {
		if (this.VoiceLineEventInstance.isValid()) {
			this.VoiceLineEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			this.VoiceLineEventInstance.release();
			this.VoiceLineEventInstance.clearHandle();
		}
		this.VoiceLineEventInstance = CreateInstance(voiceLineReference);
		this.LastVoiceLineEventInstance = this.VoiceLineEventInstance;
		this.VoiceLineEventInstance.setParameterByName("Language", PlayerPrefsManager.Load(PlayerPrefsManager.Language));
		this.VoiceLineEventInstance.start();
		StartCoroutine(WaitForVoiceLineEnd());
	}

	public void SkipVoiceOver() {
		this.VoiceLineEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		this.VoiceLineEventInstance.release();
		this.VoiceLineEventInstance.clearHandle();
		this.VoiceLineOver?.Invoke(this.LastVoiceLineEventInstance);
		StopCoroutine(WaitForVoiceLineEnd());
	}

	public void PlayReelSound(EventReference reelSound) {
		this.CurrentReelInstance = CreateInstance(reelSound);
		int reelSpeed = 0;
		this.CurrentReelInstance.setParameterByName("ReelRate", reelSpeed);
		this.CurrentReelInstance.start();
	}

	public void SetReelRate(float reelSpeed) {
		this.CurrentReelInstance.setParameterByName("ReelRate", reelSpeed);
	}

	public EventInstance CreateInstance(EventReference eventReference) {
		EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
		eventInstances.Add(eventInstance);
		return eventInstance;
	}

	public void InitializeMusic(EventReference musicEventReference) {
		CleanUp();
		this.MusicEventInstance = CreateInstance(musicEventReference);
		this.MusicEventInstance.start();
	}

	public void SetMusicParameter(string name, float value) {
		this.MusicEventInstance.setParameterByName(name, value);
	}

	#endregion


	#region Private Methods

	private void CleanUp() {
		foreach (EventInstance eventInstance in eventInstances) {
			eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			eventInstance.release();
		}
	}

	private IEnumerator WaitForVoiceLineEnd() {
		PLAYBACK_STATE playbackState;
		do {
			if (this.VoiceLineEventInstance.isValid()) {
				this.VoiceLineEventInstance.getPlaybackState(out playbackState);
				yield return null;
			} else {
				yield break;
			}
		} while (playbackState != PLAYBACK_STATE.STOPPED);
		this.VoiceLineEventInstance.release();
		this.VoiceLineEventInstance.clearHandle();
		this.VoiceLineOver?.Invoke(this.LastVoiceLineEventInstance);
	}

		#endregion

}