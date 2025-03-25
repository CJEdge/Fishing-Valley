using UnityEngine;

public class AudioRandomizer : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private bool loop;

    [SerializeField]
    private AudioSource[] audioSources;

    [SerializeField]
    private float pitchVariation;

    #endregion


    #region Properties

    public AudioSource CurrentAudioSource {
        get;
        set;
    }

    #endregion


    #region Mono Behaviours

    public void OnEnable() {
        SetRandomAudioSource();
        this.CurrentAudioSource.Play();
    }

    public void Update() {
		if (!loop) {
			return;
		}
        if (!this.CurrentAudioSource.isPlaying) {
            SetRandomAudioSource();
            RandomizePitch();
            this.CurrentAudioSource.Play();
        }
    }

    #endregion


    #region Private Methods

    private void RandomizePitch() {
        this.CurrentAudioSource.pitch = Random.Range(1 - pitchVariation, 1 + pitchVariation);
    }

    private void SetRandomAudioSource() {
        this.CurrentAudioSource = audioSources[Random.Range(0, audioSources.Length)];
    }

    #endregion
}
