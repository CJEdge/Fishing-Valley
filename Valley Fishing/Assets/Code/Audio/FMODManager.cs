using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODManager : Singleton<FMODManager>
{
	[SerializeField]
	private EventReference leafSound;

	[SerializeField]
	private EventReference mainMenuMusic;

	[SerializeField]
	private EventReference grassRootsMusic;

	public EventReference LeafSound {
		get {
			return leafSound;
		}
	}

	public EventReference MainMenuMusic {
		get {
			return mainMenuMusic;
		}
	}

	public EventReference GrassRootsMusic {
		get {
			return grassRootsMusic;
		}
	}
}
