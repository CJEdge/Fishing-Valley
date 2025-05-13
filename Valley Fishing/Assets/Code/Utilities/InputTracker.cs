using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

public static class InputTracker {
	public static bool LastInputWasMouse { get; private set; }

	static InputTracker() {
		InputSystem.onAnyButtonPress.Call(OnAnyInput);
		InputSystem.onEvent += OnInputEvent;
	}

	private static void OnAnyInput(InputControl control) {
		var device = control.device;

		if (device is Mouse || device is Pointer || device is Keyboard) {
			LastInputWasMouse = true;
		} else if (device is Gamepad) {
			LastInputWasMouse = false;
		}
	}

	private static void OnInputEvent(InputEventPtr eventPtr, InputDevice device) {
		if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
			return;

		if (device is Mouse || device is Pointer || device is Keyboard) {
			LastInputWasMouse = true;
		} else if (device is Gamepad) {
			LastInputWasMouse = false;
		}
	}
}
