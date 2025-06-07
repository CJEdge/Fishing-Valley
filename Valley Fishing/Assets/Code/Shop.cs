using FMOD.Studio;
using UnityEngine;

public abstract class Shop : AbstractState<Shop.State>
{
	#region States

	public enum State {
		Defualt,
		Entering,
		Trading,
		Leaving
	}

	#endregion

	#region Private Methods

	public abstract void VoiceLineOver(EventInstance eventInstance);

	#endregion
}
