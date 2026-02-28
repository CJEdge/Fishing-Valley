using FMODUnity;
using UnityEngine;

public class RodShop : Shop
{
    #region Serialized Fields

    [Header("Reel Speed")]

    [SerializeField] private int[] reelSpeedPrices;
    [SerializeField] private EventReference[] reelSpeedHoverEvents;
    [SerializeField] private EventReference[] reelSpeedBoughtEvents;

    [Header("Strafe Speed")]

    [SerializeField] private int[] strafeSpeedPrices;
    [SerializeField] private EventReference[] strafeSpeedHoverEvents;
    [SerializeField] private EventReference[] strafeSpeedBoughtEvents;

    [Header("Fail Speed")]
    
    [SerializeField] private int[] failSpeedPrices;
    [SerializeField] private EventReference[] failSpeedHoverEvents;
    [SerializeField] private EventReference[] failSpeedBoughtvents;

    #endregion


    #region Public Methods

    public void ReelSpeedButtonHovered()
    {
        AudioManager.Instance.PlayVoiceOver(reelSpeedHoverEvents[UpgradeManager.Instance.ReelSpeed + 1]);
    }
    public void StrafeSpeedButtonHovered()
    {
        AudioManager.Instance.PlayVoiceOver(strafeSpeedHoverEvents[UpgradeManager.Instance.StrafeSpeed + 1]);
    }
    public void FailSpeedButtonHovered()
    {
        AudioManager.Instance.PlayVoiceOver(failSpeedHoverEvents[UpgradeManager.Instance.FailSpeed + 1]);
    }

    public void UpgradeReelSpeed()
    {
        int upgradeprice = reelSpeedPrices[UpgradeManager.Instance.ReelSpeed + 1];
        if (GameManager.Instance.Money >= upgradeprice)
        {
            UpgradeManager.Instance.ReelSpeed++;
            AudioManager.Instance.PlayOneShot(FMODManager.Instance.ItemBuy);
        }
        else
        {
            AudioManager.Instance.PlayOneShot(FMODManager.Instance.ClickError);
        }
    }

    public void UpgradeStrafeSpeed()
    {
        int upgradeprice = strafeSpeedPrices[UpgradeManager.Instance.StrafeSpeed + 1];
        if (GameManager.Instance.Money >= upgradeprice)
        {
            UpgradeManager.Instance.StrafeSpeed++;
            AudioManager.Instance.PlayOneShot(FMODManager.Instance.ItemBuy);
        }
        else
        {
            AudioManager.Instance.PlayOneShot(FMODManager.Instance.ClickError);
        }
    }

    public void UpgradeFailSpeed()
    {
        int upgradeprice = failSpeedPrices[UpgradeManager.Instance.FailSpeed + 1];
        if (GameManager.Instance.Money >= upgradeprice)
        {
            UpgradeManager.Instance.FailSpeed++;
            AudioManager.Instance.PlayOneShot(FMODManager.Instance.ItemBuy);
        }
        else
        {
            AudioManager.Instance.PlayOneShot(FMODManager.Instance.ClickError);
        }
    }

    #endregion

    public override void VoiceLineOver(EventReference eventReference, bool skipped)
    {
        
    }
}
