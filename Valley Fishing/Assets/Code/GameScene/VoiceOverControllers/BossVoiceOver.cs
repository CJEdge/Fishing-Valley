using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BossVoiceOver : VoiceOverController
{
	[SerializeField] private GameObject endScreenUI;
	[SerializeField] private GameObject initialButton;
	[SerializeField] private EventReference thanksForPlayingEvent;

	public override bool PerformStateSwitch() {
		if (!base.PerformStateSwitch()) {
			return false;
		}
		switch (this.LevelController.CurrentState) {
			case LevelController.State.Idle:
				InventoryManager.Instance.OwnedBaitTypeDatas[7].quantity = 1;
				if (!AllTutorialsCompleted(this.AttatchBaitTutorialsCompleted)) {
					PlayNextTutotialVoiceOver(this.AttatchBaitTutorialsCompleted, applyBaitTutorials);
					IncrementTutorial(this.AttatchBaitTutorialsCompleted);
				}
				break;
			case LevelController.State.AttatchBait:
				if (InventoryManager.Instance.TotalOwnedBaits == 0) {
                    endScreenUI.SetActive(true);
					InputManager.Instance.SelectButton(initialButton);
					return false;
				}
				break;
			case LevelController.State.ReelingFish:				
				PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
				IncrementTutorial(this.ReelTutorialsCompleted);
				break;
			case LevelController.State.FishCaught:
				AudioManager.Instance.PlayVoiceOver(tutorialCatchVoices[0]);
				break;
			default:
				break;
		}
		return true;
	}

	public override void VoiceOverFinished(EventReference eventReference, bool skipped) {
		base.VoiceOverFinished(eventReference, skipped);
		if (LevelController.CurrentState == LevelController.State.ReelingFish) {
			GameManager.Instance.InputController.SetState(InputController.State.NotReeling);
		}
		if(LevelController.CurrentState == LevelController.State.FishCaught)
		{
            endScreenUI.SetActive(true);
			AudioManager.Instance.PlayVoiceOver(thanksForPlayingEvent);
            InputManager.Instance.SelectButton(initialButton);
        }
	}

	public void LoadMenu()
	{
		SceneManager.LoadScene("Menu");
	}
}
