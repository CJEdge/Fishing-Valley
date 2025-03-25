using UnityEngine;

public class FlipThroughObjects : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private float flipTime;

    [SerializeField]
    private GameObject[] frames;

    #endregion


    #region Properties

    private float CurrentFlipTime {
        get;
        set;
    }

    [field:SerializeField]
    private int CurrentFrame {
        get;
        set;
    }

    #endregion


    #region Mono Behaviours

    public void Start() {
        for (int i = 0; i < frames.Length; i++) {
            frames[i].SetActive(false);
        }
        frames[this.CurrentFrame].SetActive(true);
    }

    public void Update() {
        if (this.CurrentFlipTime < flipTime) {
            this.CurrentFlipTime += Time.deltaTime;
        } else {
            if (this.CurrentFrame == frames.Length - 1) {
                this.CurrentFrame = 0;
            } else {
                this.CurrentFrame++;
                this.CurrentFlipTime = 0;
            }
            for (int i = 0; i < frames.Length; i++) {
                frames[i].SetActive(false);
            }
            frames[this.CurrentFrame].SetActive(true);
        }
    }

    #endregion

}
