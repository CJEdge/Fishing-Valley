using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System;
using UnityEngine.SceneManagement;

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

	[field:SerializeField]
	public StudioEventEmitter FishActivityLevelInstance {
		get;
		set;
	}

	private List<EventInstance> SFXEventInstances {
		get;
		set;
	} = new List<EventInstance>();

	private List<EventInstance> MusicEventInstances {
		get;
		set;
	} = new List<EventInstance>();

	public Action<EventInstance> VoiceLineOver {
		get;
		set;
	}

	#endregion


	#region Mono Behaviours

	public void Start() {
		if (this.MusicEventInstances.Count == 0) {
			PlayMusic(FMODManager.Instance.LevelOneMusic);
		}
		SceneManager.sceneLoaded += PlayMusicOnSceneLoad;
	}

	public void OnDestroy() {
		CleanUpSFX();
		CleanUpMusic();
		SceneManager.sceneLoaded -= PlayMusicOnSceneLoad;
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
		this.VoiceLineEventInstance = CreateSFXInstance(voiceLineReference);
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

	public void PlayFishActivitySound(Fish fish, int activityLevel, bool play) {
		this.FishActivityLevelInstance = fish.ActivitySplashSFX;
		this.FishActivityLevelInstance.SetParameter("ActivityLevel", activityLevel);
		if (play && !this.FishActivityLevelInstance.IsPlaying()) {
			this.FishActivityLevelInstance.Play();
		} else if(!play) {
			this.FishActivityLevelInstance.Stop();
		}
	}

	public void PlayReelSound(EventReference reelSound) {
		this.CurrentReelInstance = CreateSFXInstance(reelSound);
		int reelSpeed = 0;
		this.CurrentReelInstance.setParameterByName("ActivityLevel", reelSpeed);
		this.CurrentReelInstance.start();
	}

	public void SetReelRate(float reelSpeed) {
		this.CurrentReelInstance.setParameterByName("ActivityLevel", reelSpeed);
	}

	public EventInstance CreateSFXInstance(EventReference eventReference) {
		EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
		SFXEventInstances.Add(eventInstance);
		return eventInstance;
	}

	public EventInstance CreateMusicInstance(EventReference eventReference) {
		EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
		MusicEventInstances.Add(eventInstance);
		return eventInstance;
	}

	public void InitializeMusic(EventReference musicEventReference) {
		this.MusicEventInstance = CreateMusicInstance(musicEventReference);
		this.MusicEventInstance.start();
	}

	public void SetMusicParameter(string name, float value) {
		this.MusicEventInstance.setParameterByName(name, value);
	}

	#endregion


	#region Private Methods

	private void CleanUpSFX() {
		foreach (EventInstance eventInstance in SFXEventInstances) {
			eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			eventInstance.release();
		}
	}

	private void CleanUpMusic() {
		foreach (EventInstance eventInstance in MusicEventInstances) {
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

	private void PlayMusicOnSceneLoad(Scene scene, LoadSceneMode mode) {
		CleanUpMusic();
		PlayMusic(FMODManager.Instance.LevelOneMusic);
	}

	private void PlayMusic(EventReference musicReference) {
		InitializeMusic(FMODManager.Instance.LevelOneMusic);
	}

	#endregion

}