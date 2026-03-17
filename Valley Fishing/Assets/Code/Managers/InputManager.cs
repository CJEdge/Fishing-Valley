using System.Collections;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public void SelectButton(GameObject button)
    {
        StartCoroutine(SelectButtonAfterOneFrame(button));
    }

    private static IEnumerator SelectButtonAfterOneFrame(GameObject button)
    {
        yield return new WaitForEndOfFrame();
        GameManager.Instance.EventSystem.SetSelectedGameObject(button);
    }
}
