using UnityEngine;

public class Bait : MonoBehaviour
{
	[SerializeField]
	private int baitIndex;

	public int BaitIndex {
		get {
			return baitIndex;
		}
	}

	[SerializeField]
	private float[] catchChances;

	public float[] CatchChances {
		get {
			return catchChances;
		}
	}
}
