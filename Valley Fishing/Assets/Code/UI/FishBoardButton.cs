using UnityEngine;

public class FishBoardButton : ButtonVoiceOverComponent
{
	[SerializeField]
	private FishBoard fishBoard;

	public override void DoHoverEffect() {
		Debug.Log("hover");
		AudioManager.Instance.PlayVoiceOver(fishBoard.fishNames[transform.GetSiblingIndex()]);
	}
}
