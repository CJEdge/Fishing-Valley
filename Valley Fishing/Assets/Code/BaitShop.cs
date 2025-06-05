using FMOD.Studio;
using UnityEngine;

public class BaitShop : Shop {

	#region Shop

	protected override void EnterState(State state) {
		switch (state) {
			case State.Defualt:
				AudioManager.Instance.VoiceLineOver += VoiceLineOver;
				SetState(State.Entering);
				break;
			case State.Entering:
				AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.BaitShopIntros[0]);
				break;
			case State.Selling:
				break;
			case State.Buying:
				break;
			case State.Leaving:
				break;
			default:
				break;
		}
	}

	#endregion


	#region Private Methods

	public override void VoiceLineOver(EventInstance eventInstance) {
		switch (this.CurrentState) {
			case State.Defualt:
				break;
			case State.Entering:
				SetState(State.Buying);
				break;
			case State.Selling:
				break;
			case State.Buying:
				break;
			case State.Leaving:
				break;
			default:
				break;
		}
	}

	#endregion

}
