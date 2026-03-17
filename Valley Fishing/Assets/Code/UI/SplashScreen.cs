using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private EventReference eventReference;

    public void Start()
    {
        if(eventReference.IsNull)
        {
            return;
        }
        AudioManager.Instance.PlayOneShot(eventReference);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}
