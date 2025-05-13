using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	#region Serialized Fields

	[SerializeField]
	private List<Fish> fish;

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

	public string CurrentFishName {
		get;
		set;
	}

	#endregion

}
