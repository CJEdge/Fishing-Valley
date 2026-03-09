using UnityEngine;

public class FinalShore : Shore
{
    public void OnEnable()
    {		
		eventSystem.SetSelectedGameObject(shopButtons[0]);
		Initialize();
	}
}
