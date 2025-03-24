using UnityEngine;

public class Fish : MonoBehaviour
{

    #region States

    public enum FishState {
        none,
        onHook,
        caught,
        escaped
    }

    public FishState fishState;


    public enum ActivityLevel {
        none,
        calm,
        medium,
        active
    }

    public ActivityLevel activityLevel;

    #endregion


    #region Serialized Fields

    [SerializeField]
    private string fishName;

    [SerializeField]
    private float fishSpeed;

    [SerializeField]
    private float strafeFrequency;

    [SerializeField]
    private Rigidbody rb;

    #endregion


    #region Properties

    private int MovementDirection {
        get;
        set;
    }

    private float CurrentStrafeTime {
        get;
        set;
    }

    #endregion


    #region Mono Behaviours

    public void Update() {
        switch (fishState) {
            case FishState.none:
                break;
            case FishState.onHook:
                if (this.CurrentStrafeTime < strafeFrequency) {
                    this.CurrentStrafeTime += Time.deltaTime;
                } else {
                    this.CurrentStrafeTime = 0;
                    int randomDirection = Random.Range(0, 3) - 1;
                    AssignFishDirection(randomDirection);
                }
                Reel();
                break;
            case FishState.caught:
                break;
            case FishState.escaped:
                break;
        }
    }


    public void FixedUpdate() {
        Move();
    }
    #endregion


    #region Private Methods

    private void AssignFishDirection(int fishDirection) {
        rb.linearVelocity = Vector3.zero;
        this.MovementDirection = fishDirection;
    }

    private void Move() {
        float horizontalSpeed = (fishSpeed * this.MovementDirection) + GameManager.Instance.InputController.MouseInput.x;
        rb.AddForce(horizontalSpeed, 0, 0);
    }

    private void Reel() {
        switch (activityLevel) {
            case ActivityLevel.none:
                break;
            case ActivityLevel.calm:
                break;
            case ActivityLevel.medium:
                break;
            case ActivityLevel.active:
                break;
        }
    }

    #endregion
}
