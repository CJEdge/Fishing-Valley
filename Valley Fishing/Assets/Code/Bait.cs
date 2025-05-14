using UnityEngine;

public class Bait : MonoBehaviour
{
	[SerializeField]
	private float[] catchChances;

	public float[] CatchChances {
		get {
			return catchChances;
		}
	}
}
