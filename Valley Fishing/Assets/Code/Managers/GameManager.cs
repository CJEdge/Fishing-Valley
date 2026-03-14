using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : Singleton<GameManager> {

	#region Properties
	[field:SerializeField] public EventSystem EventSystem {	get; set; }
	public GameObject LastSelectedButton { get; set; }
	public InputController InputController { get; set; }
	public LevelController LevelController { get; set; }
	public EventController EventController { get; set; }
	public MainMenuController MainMenuController { get;	set; }
	public ShopController ShopController { get;	set; }
    [field: SerializeField] public Fish FishPrefab { get; set; }
	[field:SerializeField] public Bait BaitPrefab { get; set; }
	public Fish CurrentFish { get; set; }
	[field: SerializeField]	public int Money { get;	set; }

    #endregion

}
