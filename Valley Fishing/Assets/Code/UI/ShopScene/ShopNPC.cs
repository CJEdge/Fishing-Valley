using UnityEngine;

public class ShopNPC : MonoBehaviour
{

	#region Serialized Fields

	[SerializeField] private Animator animator;
	[SerializeField] private Shop shop;

	#endregion


	#region Mono Behaviours

	public void Awake() {
		shop.OnGreeting += Greeting;
		shop.OnSaleMade += SaleMade;
	}

	public void OnDestroy() {
		shop.OnGreeting -= Greeting;
		shop.OnSaleMade -= SaleMade;
	}

	#endregion


	#region Private Methods

	private void Greeting() {
		Debug.Log("Greeting");
		animator.Play("Greeting");
	}

	private void SaleMade() {
		Debug.Log("ddalse");
		animator.Play("Sale");
	}


	#endregion

}
