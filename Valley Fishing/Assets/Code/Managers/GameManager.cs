using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
	#region Serialized Fields

	[SerializeField]
	private List<Fish> fish;

	[SerializeField]
	private List<Bait> baits;

	#endregion


	#region Properties

	public InputController InputController {
		get;
		set;
	}

	public LevelController LevelController {
		get;
		set;
	}

	public MainMenuController MainMenuController {
		get;
		set;
	}

	public ShopController ShopController {
		get;
		set;
	}

	public List<Fish> Fish {
		get {
			return fish;
		}
	}

	public List<Bait> Baits {
		get {
			return baits;
		}
	}

	[field: SerializeField]
	public List<int> CurrentBaits {
		get;
		set;
	} = new List<int>();

	public Fish CurrentFish {
		get;
		set;
	}

	[field: SerializeField]
	public List<int> CaughtFish {
		get;
		set;
	} = new List<int>();

	public Bait CurrentBait {
		get;
		set;
	}

	[field: SerializeField]
	public int Money {
		get;
		set;
	}

	public int TotalCaughtFish {
		get {
			int totalCaughtFish = 0;
			for (int i = 0; i < this.CaughtFish.Count; i++) {
				totalCaughtFish += this.CaughtFish[i];
			}
			return totalCaughtFish;
		}
	}

	public int TotalBaitsLeft {
		get {
			int totalBaits = 0;
			for (int i = 0; i < this.CurrentBaits.Count; i++) {
				totalBaits += this.CurrentBaits[i];
			}
			return totalBaits;
		}
}

	#endregion


	#region Mono Behaviours

	public override void Awake() {
		base.Awake();
		for (int i = 0; i < this.Baits.Count; i++) {
			this.CurrentBaits.Add(0);
		}
	}

	#endregion


	#region Public Methods

	public void AssignNewCaughtFish(int index) {
		while (this.CaughtFish.Count <= index) {
			this.CaughtFish.Add(0);
		}
		this.CaughtFish[index]++;
	} 

	#endregion

}
