using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODManager : Singleton<FMODManager>
{
	public EventReference MenuGreeting;

	public EventReference Level1IntroCutscene;

	public EventReference ApplyBaitTutorial;
	
	public EventReference CastRodTutorial;
}
