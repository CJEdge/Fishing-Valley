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

	public EventInstance MusicEventInstance;
	public EventInstance VoiceLineEventInstance;
	public EventReference LastVoiceLineEventReference;
	public EventInstance CurrentReelInstance;
	public EventInstance BaitEventInstance;
	public EventInstance UnspoolEventInstance;

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

	public Action<EventReference,bool> OnVoiceLineOver {
		get;
		set;
	}

	public Action OnVoiceLineStarted { get; set; }

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

	public void PlayUnspoolSound(bool play, int speed) {
		if (play) {
			this.UnspoolEventInstance = CreateSFXInstance(FMODManager.Instance.Unspool);
			this.UnspoolEventInstance.start();
		} else {
			this.UnspoolEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			this.UnspoolEventInstance.release();
		}
	}

	public void PlayVoiceOver(EventReference voiceLineReference) {
		if (this.VoiceLineEventInstance.isValid()) {
			this.VoiceLineEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			this.VoiceLineEventInstance.release();
			this.VoiceLineEventInstance.clearHandle();
		}
		this.VoiceLineEventInstance = CreateSFXInstance(voiceLineReference);
		this.LastVoiceLineEventReference = voiceLineReference;
		this.VoiceLineEventInstance.setParameterByName("Language", PlayerPrefsManager.Load(PlayerPrefsManager.Language));
		this.VoiceLineEventInstance.start();
		this.VoiceLineInProgress = true;
		this.OnVoiceLineStarted?.Invoke();
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
		this.VoiceLineInProgress = false;
		this.OnVoiceLineOver?.Invoke(this.LastVoiceLineEventReference, true);
		StopCoroutine(WaitForVoiceLineEnd());
	}

	public void PlayVoiceOverChain(List<EventReference> voiceOverChain) {
		this.VoiceOverChainPosition = 0;
		this.VoiceOverChain = voiceOverChain;
		this.InVoiceOverChain = true;
		PlayVoiceOver(voiceOverChain[0]);
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
		while (true) {
			if (!this.VoiceLineEventInstance.isValid()) {
				this.VoiceLineInProgress = false;
				yield break;
			}

			this.VoiceLineEventInstance.getPlaybackState(out playbackState);
			if (playbackState == PLAYBACK_STATE.STOPPED)
				break;

			yield return null;
		}

		this.VoiceLineEventInstance.release();
		this.VoiceLineEventInstance.clearHandle();

		this.VoiceLineInProgress = false;
		this.OnVoiceLineOver?.Invoke(this.LastVoiceLineEventReference, false);

		// Proceed to next in chain
		if (this.InVoiceOverChain) {
			this.VoiceOverChainPosition++;
			if (this.VoiceOverChainPosition < this.VoiceOverChain.Count) {
				yield return new WaitForEndOfFrame(); // ensures previous event is fully released
				PlayVoiceOver(this.VoiceOverChain[this.VoiceOverChainPosition]);
			} else {
				this.InVoiceOverChain = false;
			}
		}
	}

	private IEnumerator ContinueVoiceOverChain() {
		yield return new WaitForEndOfFrame();
		if (this.VoiceOverChainPosition < this.VoiceOverChain.Count) {
			Debug.Log(this.VoiceOverChain[this.VoiceOverChainPosition]);
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