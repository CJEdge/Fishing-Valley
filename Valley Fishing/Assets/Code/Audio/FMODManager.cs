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

	public EventReference[] CatchVoices;

	[Header("ShopVoiceOver")]

	public EventReference[] ShoreIntros;

	public EventReference[] LeaveShorePrompts;

	public EventReference[] BaitShopIntros;

	public EventReference[] BaitShopTutorialItemIntros;

	public EventReference[] BaitShopItemIntros;

	public EventReference[] BaitShopSellYourItems;

	public EventReference[] BaitShopTutorialThanks;

	public EventReference[] BaitShopThanks;

	public EventReference[] BaitIntros;


	[Header("Prices")]

	public EventReference price;

	[Header("LevelSFX")]

	public EventReference ThrowRod;

	public EventReference LandRod;

	public EventReference ReelSound;

	public EventReference ActivityLevelSplash;

	public EventReference BaitBoxOpen;

	public EventReference[] BaitSounds;

	public EventReference[] AttatchBaitSounds;

	public EventReference FishCatch;

	[Header("ShopSFX")]

	public EventReference MoneyEarnt;

	public EventReference ItemBuy;

	public EventReference ShopEnter;

	[Header("Music")]

	public EventReference LevelOneMusic;
}
