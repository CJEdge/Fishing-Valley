using UnityEngine;

public class SelectButtonUtility : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.Instance.SelectButton(gameObject);
    }
}
