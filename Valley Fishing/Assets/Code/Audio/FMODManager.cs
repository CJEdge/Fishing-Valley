using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODManager : Singleton<FMODManager>
{
	[Header("VoiceOver")]

	public EventReference MenuGreeting;

	public EventReference Level1IntroCutscene;

	public EventReference ApplyBaitTutorial;
	
	public EventReference CastRodTutorial;

	[Header("SFX")]

	public EventReference ThrowRod;

	public EventReference LandRod;

	public EventReference ReelSound;
}
