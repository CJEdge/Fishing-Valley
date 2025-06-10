using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODManager : Singleton<FMODManager>
{
	[Header("MenuVoiceOver")]

	public EventReference MenuGreeting;

	public EventReference MenuPlay;

	public EventReference MenuSettings;

	public EventReference MainMenu;

	[Header("LevelVoiceOver")]

	public EventReference Level1IntroCutscene;

	public EventReference[] ApplyBaitTutorials;
	
	public EventReference[] CastRodTutorials;

	public EventReference[] ReelTutorials;

	public EventReference[] TutorialCatchVoices;

	public EventReference[] CatchVoices;

	[Header("ShopVoiceOver")]

	public EventReference[] ShoreIntros;

	public EventReference[] BaitShopIntros;

	public EventReference[] BaitShopTutorialItemIntros;

	public EventReference[] BaitShopItemIntros;

	public EventReference[] BaitShopSellYourItems;

	public EventReference[] BaitShopTutorialThanks;

	public EventReference[] BaitShopThanks;

	public EventReference[] BaitIntros;


	[Header("Prices")]

	public EventReference price;

	[Header("SFX")]

	public EventReference ThrowRod;

	public EventReference LandRod;

	public EventReference ReelSound;

	public EventReference ActivityLevelSplash;

	public EventReference[] BaitSounds;

	[Header("Music")]

	public EventReference LevelOneMusic;
}
