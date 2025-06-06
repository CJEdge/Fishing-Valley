using UnityEngine;

public class Bait : MonoBehaviour
{
	[SerializeField]
	private bool isFailable;
	
	public bool Isfailable {
		get {
			return isFailable;
		}
	}

	[SerializeField]
	private bool isTutorial;

	public bool IsTutorial {
		get {
			return isTutorial;
		}
	}

	[SerializeField]
	private int baitIndex;

	public int BaitIndex {
		get {
			return baitIndex;
		}
	}

	[SerializeField]
	private int baitPrice;
	
	public int BaitPrice {
		get {
			return baitPrice;
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
