using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    #region Singleton Behaviour

    public static FadeManager Instance;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion


    #region Serialized Fields

    [SerializeField]
    private Image image;

    [SerializeField]
    private float fadeDuration;

    #endregion


    #region Public Methods

    public void FadeToBlack() {
        StartCoroutine(FadeImageToBlack());
    }

    public void FadeToClear() {
        StartCoroutine(FadeImageToClear());
    }

    #endregion


    #region Private Methods

    private IEnumerator FadeImageToBlack() {
        float elapsedTime = 0f;
        Color startColor = new Color(0, 0, 0, 0);
        Color endColor = Color.black;

        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            image.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            yield return null;
        }

        image.color = endColor;
    }

    private IEnumerator FadeImageToClear() {
        float elapsedTime = 0f;
        Color startColor = Color.black;
        Color endColor = new Color(0, 0, 0, 0);

        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            image.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            yield return null;
        }

        image.color = endColor;
    }

    #endregion

}
