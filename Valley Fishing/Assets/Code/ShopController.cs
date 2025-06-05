using UnityEngine;

public class ShopController : AbstractState<ShopController.State>
{
	#region State Behaviour

	public enum State {
		Default,
		CutsceneIn,
		CutsceneOut,
		BaitShop,
		RodShop,
		StorageShop
	}

	protected override void EnterState(State state) {
		switch (state) {
			case State.Default:
				GameManager.Instance.ShopController = this;
				SetState(State.CutsceneIn);
				break;
			case State.CutsceneIn:
				SetState(State.BaitShop);
				break;
			case State.CutsceneOut:
				break;
			case State.BaitShop:
				break;
			case State.RodShop:
				break;
			case State.StorageShop:
				break;
			default:
				break;
		}
	}

	#endregion
}
