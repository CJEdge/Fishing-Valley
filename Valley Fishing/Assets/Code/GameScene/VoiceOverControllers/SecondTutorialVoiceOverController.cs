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
				LevelController.SetState(LevelController.State.AttatchBait);
				break;
			case LevelController.State.IdleWithBait:
				PlayNextTutotialVoiceOver(this.CastRodTutorialsCompleted, castRodTutorials);
				IncrementTutorial(this.CastRodTutorialsCompleted);
				break;
			case LevelController.State.ReelingFish:
				this.CurrentFish.BecameCentered += BecameCentered;
				if (this.ReelTutorialsCompleted[0] == false) {
					this.CurrentFish.FishData.MovementDirections.Clear(); ;
					this.CurrentFish.FishData.MovementDirections.Add(FishDatas.FishData.MovementDirection.left);
					this.CurrentFish.CurrentActivityLevel = FishDatas.FishData.ActivityLevel.active;
				} else if (this.ReelTutorialsCompleted[1] == false) {
					this.CurrentFish.FishData.MovementDirections.Clear(); ;
					this.CurrentFish.FishData.MovementDirections.Add(FishDatas.FishData.MovementDirection.right);
					this.CurrentFish.CurrentActivityLevel = FishDatas.FishData.ActivityLevel.calm;
				} else if (this.ReelTutorialsCompleted[2] == false) {
					PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
					IncrementTutorial(this.ReelTutorialsCompleted);
				}
				break;
			case LevelController.State.FishCaught:
				if (GameManager.Instance.TotalBaitsLeft == 0) {
					this.LevelController.StateLocked = true;
					List<EventReference> voiceOverChain = new List<EventReference>();
					voiceOverChain.Add(this.CurrentFish.CaughtVoiceLine);
					voiceOverChain.Add(FMODManager.Instance.LeaveBoatPrompts[0]);
					AudioManager.Instance.PlayVoiceOverChain(voiceOverChain);
				} else {
					AudioManager.Instance.PlayVoiceOver(this.CurrentFish.CaughtVoiceLine);
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

	public override void FishStrafed(FishDatas.FishData.MovementDirection movementDirection) {
		if (AllTutorialsCompleted(this.ReelTutorialsCompleted)) {
			return;
		}
		PlayNextTutotialVoiceOver(this.ReelTutorialsCompleted, reelTutorials);
		IncrementTutorial(this.ReelTutorialsCompleted);
	}

	public override void FishSpawned() {
		if (this.ReelTutorialsCompleted[0] == false) {
			this.CurrentFish.FishData.ActivityLevels.Clear();
			this.CurrentFish.FishData.ActivityLevels.Add(FishDatas.FishData.ActivityLevel.active);
		} else if (this.ReelTutorialsCompleted[1] == false) {
			this.CurrentFish.FishData.ActivityLevels.Clear();
			this.CurrentFish.FishData.ActivityLevels.Add(FishDatas.FishData.ActivityLevel.calm);
		}
	}

	public override void VoiceOverFinished(EventReference eventReference, bool skipped) {

		base.VoiceOverFinished(eventReference, skipped);
		if (LevelController.CurrentState == LevelController.State.ReelingFish) {
			GameManager.Instance.InputController.SetState(InputController.State.NotReeling);
		}
		if(this.LevelController.CurrentState == LevelController.State.FishCaught) {
			if (this.LevelController.StateLocked) {
				SceneManager.LoadScene(LevelManager.ShopTutorial_01);
			}
		}
	}

	private void BecameCentered() {
		GameManager.Instance.InputController.SetState(InputController.State.NotReeling);
	}
}
