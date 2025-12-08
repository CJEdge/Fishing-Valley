using UnityEngine;

public class HacksManager : MonoBehaviour
{
    #region Player State Changes
    [ContextMenu("ChangePlayerStateDefault")]
    public void ChangePlayerStateDefault()
    {
        GameManager.Instance.InputController.SetState(InputController.State.Default);
    }

    [ContextMenu("ChangePlayerStateReelLocked")]
    public void ChangePlayerStateReelLocked()
    {
        GameManager.Instance.InputController.SetState(InputController.State.ReelingLocked);
    }

    [ContextMenu("ChangePlayerStateNotReeling")]
    public void ChangePlayerStateNotReeling()
    {
        GameManager.Instance.InputController.SetState(InputController.State.NotReeling);
    }

    [ContextMenu("ChangePlayerStateCalmReeling")]
    public void ChangePlayerStateCalmReeling()
    {
        GameManager.Instance.InputController.SetState(InputController.State.CalmReeling);
    }

    [ContextMenu("ChangePlayerStateNormalReeling")]
    public void ChangePlayerStateNormalReeling()
    {
        GameManager.Instance.InputController.SetState(InputController.State.NormalReeling);
    }

    [ContextMenu("ChangePlayerStateFastReeling")]
    public void ChangePlayerStateFastReeling()
    {
        GameManager.Instance.InputController.SetState(InputController.State.FastReeling);
    }
    #endregion

    #region Level State Changes
    [ContextMenu("ChangeLevelStateDefault")]
    public void ChangeLevelStateDefault()
    {
        GameManager.Instance.LevelController.SetState(LevelController.State.Default);
    }

    [ContextMenu("ChangeLevelStateCutscene")]
    public void ChangeLevelStateCutscene()
    {
        GameManager.Instance.LevelController.SetState(LevelController.State.Cutscene);
    }

    [ContextMenu("ChangeLevelStateIdle")]
    public void ChangeLevelStateIdle()
    {
        GameManager.Instance.LevelController.SetState(LevelController.State.Idle);
    }

    [ContextMenu("ChangeLevelStateAttatchBait")]
    public void ChangeLevelStateAttatchBait()
    {
        GameManager.Instance.LevelController.SetState(LevelController.State.AttatchBait);
    }

    [ContextMenu("ChangeLevelStateIdleWithBait")]
    public void ChangeLevelStateIdleWithBait()
    {
        GameManager.Instance.LevelController.SetState(LevelController.State.IdleWithBait);
    }

    [ContextMenu("ChangeLevelStateWaitingForBite")]
    public void ChangeLevelStateWaitingForBite()
    {
        GameManager.Instance.LevelController.SetState(LevelController.State.WaitingForBite);
    }

    [ContextMenu("ChangeLevelStateReelingFish")]
    public void ChangeLevelStateReelingFish()
    {
        GameManager.Instance.LevelController.SetState(LevelController.State.ReelingFish);
    }

    [ContextMenu("ChangeLevelStateFishCaught")]
    public void ChangeLevelStateFishCaught()
    {
        GameManager.Instance.LevelController.SetState(LevelController.State.FishCaught);
    }
    #endregion

    #region Adding Bait
    //[ContextMenu("AddBait1")]
    //public void AddBait1()
    //{
    //    GameManager.Instance.Baits.Add();
    //}
    #endregion
}
