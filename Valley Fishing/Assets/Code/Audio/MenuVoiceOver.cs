using UnityEngine;

public class MenuVoiceOver : MonoBehaviour
{
    public void PlayGreeting()
    {
		AudioManager.Instance.PlayVoiceOver(FMODManager.Instance.MenuGreeting);
    }
}
