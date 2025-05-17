using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODManager : Singleton<FMODManager>
{
	[Header("VoiceOver")]

	public EventReference MenuGreeting;

	public EventReference MenuPlay;

	public EventReference MenuSettings;

	public EventReference MainMenu;

	public EventReference Level1IntroCutscene;

	public EventReference[] ApplyBaitTutorials;
	
	public EventReference[] CastRodTutorials;

	public EventReference[] ReelTutorials;

	[Header("SFX")]

	public EventReference ThrowRod;

	public EventReference LandRod;

	public EventReference ReelSound;

	public EventReference ActivityLevelSplash;
}
