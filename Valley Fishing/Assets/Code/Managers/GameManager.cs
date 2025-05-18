using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
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

	[field:SerializeField]
	public List<int> CurrentBaits {
		get;
		set;
	}

	public Fish CurrentFish {
		get;
		set;
	}

	[field:SerializeField]
	public Bait CurrentBait {
		get;
		set;
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

}
