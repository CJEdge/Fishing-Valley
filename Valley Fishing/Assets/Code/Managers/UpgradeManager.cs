using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    public int ReelSpeed { get; set; }
    public int StrafeSpeed { get; set; }
    public int FailSpeed { get; set; }
    public int AIRod { get; set; }
    public int InstaWin { get; set; }
}
