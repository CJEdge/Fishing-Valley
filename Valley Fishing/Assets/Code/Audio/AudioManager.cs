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
	public EventInstance BaitEventInstance {
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

	public Action<EventInstance,bool> OnVoiceLineOver {
		get;
		set;
	}

	public bool VoiceLineInProgress {
		get;
		set;
	}

	public List<EventReference> VoiceOverChain {
		get;
		set;
	} = new List<EventReference>();

	public int VoiceOverChainPosition {
		get;
		set;
	}

	[field:SerializeField]
	public bool InVoiceOverChain {
		get;
		set;
	}

	#endregion


	#region Mono Behaviours

	public void Start() {
		CleanUpMusic();
		if (SceneManager.GetActiveScene().name == LevelManager.BossTutorial_00) {
			PlayMusic(FMODManager.Instance.BossMusic);
		} else {
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
	public void PlayOneShot(EventReference sound, Vector3 position = default) {
		RuntimeManager.PlayOneShot(sound, position);
	}

	public void PlayBaitSound(bool play, int index) {
		if (play) {
			this.BaitEventInstance = CreateSFXInstance(FMODManager.Instance.BaitSounds[index]);
			this.BaitEventInstance.start();
		} else {
			this.BaitEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			this.BaitEventInstance.release();
		}
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
		this.VoiceLineInProgress = true;
		StartCoroutine(WaitForVoiceLineEnd());
	}

	public void SkipVoiceOver() {
		if (this.InVoiceOverChain) {
			this.VoiceOverChainPosition = this.VoiceOverChain.Count;
			this.InVoiceOverChain = false;
		}
		this.VoiceLineEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		this.VoiceLineEventInstance.release();
		this.VoiceLineEventInstance.clearHandle();
		this.OnVoiceLineOver?.Invoke(this.LastVoiceLineEventInstance,true);
		StopCoroutine(WaitForVoiceLineEnd());
	}

	public void PlayVoiceOverChain(List<EventReference> voiceOverChain) {
		this.VoiceOverChainPosition = 0;
		this.VoiceOverChain = voiceOverChain;
		this.InVoiceOverChain = true;
		PlayVoiceOver(voiceOverChain[this.VoiceOverChainPosition]);
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

	public void CleanUpEverything() {
		CleanUpSFX();
		CleanUpMusic();
		CleanUpVoiceOver();
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

	private void CleanUpVoiceOver() {
		this.VoiceLineEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		this.VoiceLineEventInstance.release();
	}

	private IEnumerator WaitForVoiceLineEnd() {
		PLAYBACK_STATE playbackState;
		do {
			if (this.VoiceLineEventInstance.isValid()) {
				this.VoiceLineEventInstance.getPlaybackState(out playbackState);
				yield return null;
			} else {
				this.VoiceLineInProgress = false;
				yield break;
			}
		} while (playbackState != PLAYBACK_STATE.STOPPED);
		if (this.VoiceLineInProgress) {
			if (this.VoiceOverChainPosition < this.VoiceOverChain.Count) {
				this.VoiceOverChainPosition++;
				StartCoroutine(ContinueVoiceOverChain());
			}
		}
		this.VoiceLineEventInstance.release();
		this.VoiceLineEventInstance.clearHandle();
		this.OnVoiceLineOver?.Invoke(this.LastVoiceLineEventInstance, false);
		this.VoiceLineInProgress = false;
	}

	private IEnumerator ContinueVoiceOverChain() {
		yield return new WaitForEndOfFrame();
		if (this.VoiceOverChainPosition < this.VoiceOverChain.Count) {
			this.VoiceOverChain = this.VoiceOverChain;
			this.InVoiceOverChain = true;
			PlayVoiceOver(this.VoiceOverChain[this.VoiceOverChainPosition]);
		}
		if(this.VoiceOverChainPosition == this.VoiceOverChain.Count - 1) {
			this.InVoiceOverChain = false;
		}
	}

	private void PlayMusicOnSceneLoad(Scene scene, LoadSceneMode mode) {
		CleanUpMusic();
		if (scene.name == LevelManager.BossTutorial_00) {
			PlayMusic(FMODManager.Instance.BossMusic);
		} else {
			PlayMusic(FMODManager.Instance.LevelOneMusic);
		}
	}

	private void PlayMusic(EventReference musicReference) {
		InitializeMusic(musicReference);
	}

	#endregion

}