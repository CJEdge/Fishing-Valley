using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
	public static T Instance {
		get;
		private set;
	}

	public virtual void Awake() {
		if (Instance != null && Instance != this) {
			DestroyImmediate(gameObject);
			return;
		}

		Instance = this as T;
		DontDestroyOnLoad(gameObject);
	}
}
