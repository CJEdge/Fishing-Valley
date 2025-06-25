using FMOD.Studio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FourthTutorialVoiceOver : VoiceOverController
{
	public override void VoiceOverFinished(EventInstance finishedEvent) {
		base.VoiceOverFinished(finishedEvent);
		switch (this.LevelController.CurrentState) {
			case LevelController.State.FishCaught:
				if (GameManager.Instance.TotalBaitsLeft == 0) {
					GameManager.Instance.LevelController.FishView.EnableFishUI(false);
					SceneManager.LoadScene(LevelManager.ShopTutorial_03);
				}
				break;
			default:
				break;
		}
	}
}
