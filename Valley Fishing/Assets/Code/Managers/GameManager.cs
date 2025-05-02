using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    #region Properties

    public InputController InputController {
		get;
		set;
    }

	public LevelController LevelController {
		get;
		set;
	}

	#endregion


	#region Mono Behaviours

	public void Start() {

	}

	public void Update() {

    }

    #endregion

}
