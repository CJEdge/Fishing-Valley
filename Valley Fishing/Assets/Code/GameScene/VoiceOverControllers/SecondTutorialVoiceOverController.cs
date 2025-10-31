using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecondTutorialVoiceOverController : VoiceOverController
{
	public override bool PerformStateSwitch() {
		switch (this.LevelController.CurrentState) {
			case LevelController.State.Idle:
				if (GameManager.Instance.TotalBaitsLeft == 5) {
					AudioManager.Instance.PlayOneShot(FMODManager.Instance.AttatchBaitSounds[4]);
					AudioManager.Instance.PlayBaitSound(true, 4);
					GameManager.Instance.CurrentBait = GameManager.Instance.Baits[4];
					GameManager.Instance.CurrentBaits[4]--;
					PlayNextTutotialVoiceOver(this.AttatchBaitTutorialsCompleted, applyBaitTutorials);
					LevelController.SetState(LevelController.State.IdleWithBait);
					return false;
				}
				LevelController.SetState(LevelController.State.AttatchBait);
				break;
			case LevelController.State.AttatchBait:
				if (GameManager.Instance.TotalBaitsLeft == 0) {
					GameManager.Instance.LevelController.FishView.EnableFishUI(false);
					SceneManager.LoadScene(LevelManager.CatchTutorial_02);
					return false;
				}
				break;
			case LevelController.State.ReelingFish:
				this.CurrentFish.BecameCentered += BecameCentered;
				if (this.ReelTutorialsCompleted[0] == false) {
					this.CurrentFish.movementDirections.Clear(); ;
					this.CurrentFish.movementDirections.Add(Fish.MovementDirection.left);
					this.CurrentFish.CurrentActivityLevel = Fish.ActivityLevel.active;
				} else if (this.ReelTutorialsCompleted[1] == false) {
					this.CurrentFish.movementDirections.Clear(); ;
					this.CurrentFish.movementDirections.Add(Fish.MovementDirection.right);
					this.CurrentFish.CurrentActivityLevel = Fish.ActivityLevel.calm;
				} else if (this.ReelTutorialsCompleted[2] == false) {
					PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
					IncrementTutorial(this.ReelTutorialsCompleted);
				}
				break;
			case LevelController.State.FishCaught:
				if (GameManager.Instance.TotalBaitsLeft == 0) {
					LevelController.StateLocked = true;
					List<EventReference> voiceOverChain = new List<EventReference>();
					voiceOverChain.Add(this.CurrentFish.CaughtVoiceLine);
					voiceOverChain.Add(FMODManager.Instance.LeaveBoatPrompts[0]);
					AudioManager.Instance.PlayVoiceOverChain(voiceOverChain);
				} else {
					if (this.CurrentFish.IsTutorial) {
						//AudioManager.Instance.PlayVoiceOver(this.CurrentFish.TutorialCaughtVoiceLine);
					} else {
						AudioManager.Instance.PlayVoiceOver(this.CurrentFish.CaughtVoiceLine);
					}
				}
				break;
			default:
				break;
		}
		if (!base.PerformStateSwitch()) {
			return false;
		}
		return true;
	}

	public override void FishStrafed(Fish.MovementDirection movementDirection) {
		if (AllTutorialsCompleted(this.ReelTutorialsCompleted)) {
			return;
		}
		PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
		IncrementTutorial(this.ReelTutorialsCompleted);
	}

	public override void FishSpawned() {
		if (this.ReelTutorialsCompleted[0] == false) {
			this.CurrentFish.ActivityLevels.Clear();
			this.CurrentFish.ActivityLevels.Add(Fish.ActivityLevel.active);
		} else if (this.ReelTutorialsCompleted[1] == false) {
			this.CurrentFish.ActivityLevels.Clear();
			this.CurrentFish.ActivityLevels.Add(Fish.ActivityLevel.calm);
		}
	}

	public override void VoiceOverFinished(EventReference eventReference, bool skipped) {

		base.VoiceOverFinished(eventReference, skipped);
		if (LevelController.CurrentState == LevelController.State.ReelingFish) {
			GameManager.Instance.InputController.SetState(InputController.State.NotReeling);
		}
	}

	private void BecameCentered() {
		GameManager.Instance.InputController.SetState(InputController.State.NotReeling);
	}
}
