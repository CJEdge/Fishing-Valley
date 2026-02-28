using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class AudioManager : Singleton<AudioManager>
{

	#region Properties

	public EventInstance MusicEventInstance;
	public EventInstance VoiceLineEventInstance;
	public EventReference LastVoiceLineEventReference;
	public EventInstance CurrentReelInstance;
	public EventInstance BaitEventInstance;
	public EventInstance UnspoolEventInstance;
	public EventInstance FliesEventInstance;

	[field:SerializeField]
	public StudioEventEmitter FishActivityLevelInstance { get; set;	}
	private List<EventInstance> SFXEventInstances {	get; set; } = new List<EventInstance>();
	private List<EventInstance> MusicEventInstances { get; set; } = new List<EventInstance>();
	public Action<EventReference,bool> OnVoiceLineOver { get; set; }
	public Action OnVoiceLineStarted { get; set; }
	public bool VoiceLineInProgress { get; set;	}
	public bool LastVoiceLineWasInChain { get; set; }
	public List<EventReference> VoiceOverChain { get; set; } = new List<EventReference>();
	public int VoiceOverChainPosition {	get; set; }
	[field:SerializeField] public bool InVoiceOverChain { get; set;	}
	private bool CanSkip { get; set; } = true;

	#endregion


	#region Mono Behaviours

	public void OnDestroy() {
		CleanUpSFX();
		CleanUpMusic();
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

	public void PlayUnspoolSound(bool play, float pitch) {
		if (play) {
			this.UnspoolEventInstance = CreateSFXInstance(FMODManager.Instance.Unspool);
			this.UnspoolEventInstance.setParameterByName("Pitch", pitch);
			this.UnspoolEventInstance.start();
		} else {
			this.UnspoolEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			this.UnspoolEventInstance.release();
		}
	}

	public void PlayFliesSound(bool play) {
		if (play) {
			this.FliesEventInstance = CreateSFXInstance(FMODManager.Instance.FliesWarning);
			this.FliesEventInstance.start();
		} else {
			this.FliesEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			this.FliesEventInstance.release();
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
		if(Gamepad.current.layout == "XInputController" || Gamepad.current.layout == "XInputControllerWindows" || Gamepad.current.layout == "XboxGamepadMacOS" || Gamepad.current.layout == "XboxOneGamepadMacOSWireless" || Gamepad.current.layout == "XboxOneGamepadiOS") {
			this.VoiceLineEventInstance.setParameterByName("ControlScheme", 0);
		} else if (Gamepad.current.layout == "DualShock3GamepadHID" || Gamepad.current.layout == "DualShock4GamepadHID" || Gamepad.current.layout == "DualShock4GamepadiOS" || Gamepad.current.layout == "DualSenseGamepadHID" ) {
			this.VoiceLineEventInstance.setParameterByName("ControlScheme", 1);
		} else if (Gamepad.current.layout == "SwitchProControllerHID") {
			this.VoiceLineEventInstance.setParameterByName("ControlScheme", 1);
		}
		this.VoiceLineEventInstance.start();
		this.VoiceLineInProgress = true;
		this.OnVoiceLineStarted?.Invoke();
		StartCoroutine(WaitForVoiceLineEnd());
	}

	public void SkipVoiceOver() {
		if (!this.CanSkip) {
			return;
		}
		this.VoiceLineEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		this.VoiceLineEventInstance.release();
		this.VoiceLineEventInstance.clearHandle();
		this.VoiceLineInProgress = false;
		this.OnVoiceLineOver?.Invoke(this.LastVoiceLineEventReference, true);
		if (this.InVoiceOverChain) {
			this.VoiceOverChainPosition = this.VoiceOverChain.Count;
			this.InVoiceOverChain = false;
		}
		StopCoroutine(WaitForVoiceLineEnd());
	}

	public void DisableSkipping() {
		StartCoroutine(RunDisableSkipping());
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
		CleanUpMusic();
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

		if (this.InVoiceOverChain) {
			this.VoiceOverChainPosition++;
			if (this.VoiceOverChainPosition < this.VoiceOverChain.Count) {
				yield return new WaitForEndOfFrame();
				PlayVoiceOver(this.VoiceOverChain[this.VoiceOverChainPosition]);
			} else {				
				this.OnVoiceLineOver?.Invoke(this.LastVoiceLineEventReference, false);
				this.InVoiceOverChain = false;
			}
		} else { 
			this.OnVoiceLineOver?.Invoke(this.LastVoiceLineEventReference, false);
		}
	}

	private IEnumerator RunDisableSkipping() {
		this.CanSkip = false;
		yield return new WaitForEndOfFrame();
		this.CanSkip = true;
	}

	public void PlayMusic(EventReference musicReference) {
		InitializeMusic(musicReference);
	}

	#endregion

}