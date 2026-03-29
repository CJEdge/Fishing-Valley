using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopController : MonoBehaviour
{
	#region Serialized Fields

	[SerializeField] private EventSystem eventSystem;
	[SerializeField] private EventReference levelMusic;
	[SerializeField] private bool isTutorial;

	#endregion


	#region Properties
	[field: SerializeField] public Shore Shore { get; set; }
    [field: SerializeField] public BaitShop BaitShop { get; set; }
    [field: SerializeField] public RodShop RodShop { get; set; }
    [field: SerializeField] public Icthyologists Icthyologists { get; set; }
    [field: SerializeField] public InventorsLab InventorsLab { get; set; }

    #endregion


    #region Mono Behaviours

    public void Awake()
    {
        GameManager.Instance.ShopController = this;
        GameManager.Instance.EventSystem = eventSystem;
        AudioManager.Instance.PlayMusic(levelMusic);
		if (isTutorial) {
			return;
		}
		if (this.Shore != null) {
			this.Shore.gameObject.SetActive(true);
			this.BaitShop.gameObject.SetActive(false);
		}        
    }

	#endregion
}
