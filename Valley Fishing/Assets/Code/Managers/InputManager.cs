using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class InputManager : Singleton<InputManager>
{

	#region Input Swithching
	public InputDevice CurrentDevice { get; set; }
	public bool InputRecieved;
	public bool InputSwitched;

	public event System.Action OnInputSwitched;

	public override void Awake() {
		base.Awake();
		InputSystem.onEvent += OnAnyInputEvent;
	}

	void OnDestroy() {
		if (Instance == this)
			InputSystem.onEvent -= OnAnyInputEvent;
	}

	void OnAnyInputEvent(InputEventPtr eventPtr, InputDevice device) {	
		if (device == CurrentDevice) {			
			InputSwitched = false;
			this.InputRecieved = false;
			return;
		}
		this.InputSwitched = true;
		this.InputRecieved = true;
		CurrentDevice = device;		

		OnInputSwitched?.Invoke();
	}

	#endregion


	#region Public Methods

	public void SelectButton(GameObject button) {
		StartCoroutine(SelectButtonAfterOneFrame(button));
	}

	#endregion


	#region Private Methods

	private static IEnumerator SelectButtonAfterOneFrame(GameObject button) {
		yield return new WaitForEndOfFrame();
		GameManager.Instance.EventSystem.SetSelectedGameObject(button);
	}

	#endregion

}
