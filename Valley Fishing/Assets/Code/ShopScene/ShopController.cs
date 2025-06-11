using UnityEngine;

public class ShopController : AbstractState<ShopController.State>
{
	#region Serialized Fields

	[SerializeField]
	private Shore shoreMenu;

	[SerializeField]
	private BaitShop baitShop;

	#endregion


	#region Properties

	public Shore ShoreMenu {
		get {
			return shoreMenu;
		}
	}

	#endregion


	#region State Behaviour

	public enum State {
		Default,
		CutsceneIn,
		Shore,
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
				SetState(State.Shore);
				break;
			case State.Shore:
				shoreMenu.Initialize();
				break;
			case State.CutsceneOut:
				break;
			case State.BaitShop:
				baitShop.SetState(Shop.State.Entering);
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
