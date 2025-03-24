using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    private float rodSpeed;

    [SerializeField]
    private float reelResetRate;

    [SerializeField]
    private float maxReelRate;


    #endregion


    #region Properties

    public Vector2 MouseInput {
        get;
        set;
    }

    [field:SerializeField]
    public float ReelInput {
        get;
        set;
    }

    private float CurrentResetRate {
        get;
        set;
    }

    #endregion


    #region Mono Behaviours

    public void Update() {
        if(this.CurrentResetRate < reelResetRate) {
            this.CurrentResetRate += Time.deltaTime;
        } else {
            this.CurrentResetRate = 0;
            ReduceReelRate();
        }
    }

    #endregion


    #region Public Methods

    public void MoveRod(InputAction.CallbackContext context) {
        this.MouseInput = context.ReadValue<Vector2>() * rodSpeed;
    }

    public void Reel(InputAction.CallbackContext context) {
        this.ReelInput += context.ReadValue<Vector2>().y;
    }

    #endregion

    #region Private Methods

    private void ReduceReelRate() {
        if (this.ReelInput > 0 && this.ReelInput < maxReelRate) {
            this.ReelInput--;
        }
    }

    #endregion
}
