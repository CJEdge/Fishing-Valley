using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonInteractionComponent : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{

	#region Button Animation

	[System.Serializable]
	public class ButtonAnimation {
		public AnimationCurve EnableCurve;
		public bool EnableAnimationInProgress;
		public AnimationCurve IdleCurve;
		public AnimationCurve SelectCurve;
		public bool SelectAnimationInProgress;
		public bool EnalbeIdleAnimation;
		public AnimationCurve IdleSelectCurve;
		public AnimationCurve ClickCurve;
		public bool ClickAnimationInProgress;
		public AnimationCurve IdleClickCurve;
		public bool AnimationInProgress { get => this.EnableAnimationInProgress || SelectAnimationInProgress || ClickAnimationInProgress; }
	}

	#endregion


	#region Serialized Fields

	[SerializeField] private bool startSelected;
	[SerializeField] private Button button;
	[SerializeField] private GameObject selectObject;
	[SerializeField] private GameObject clickObject;

	#endregion


	#region Properties
	[field: SerializeField] public Image ButtonImage { get; set; }
	[field: SerializeField] public ButtonAnimation ScaleAnimation { get; set; }
	private bool Selected { get; set; }
	private bool Clicked { get; set; }
	private Vector3 InitialScale { get; set; }

	#endregion


	#region Mono Behaviours

	public void Start() {
		if (startSelected) {
			OnSelect(null);
		}
	}

	public void OnEnable() {
		this.InitialScale = transform.localScale;
		StartCoroutine(PlayEnableAnimation());
	}

	public void OnDisable() {
		transform.localScale = this.InitialScale;
	}

	public void Update() {
		PlayIdleAnimation();
		PlayIdleSelectAnimation();
		PlayIdleClickAnimation();
	}

	public void OnDestroy() {
		if (button == null) {
			return;
		} 
	}


	#endregion


	#region Public Methods

	public void OnSubmit(BaseEventData eventData) {
		if (clickObject != null) {
			clickObject.SetActive(!clickObject.activeSelf);
		}
		this.Clicked = !this.Clicked;
		if (ScaleAnimation.ClickCurve.length == 0) {
			return;
		}
		StartCoroutine(PlayClickAnimation());
	}

	public void OnSelect(BaseEventData eventData) {
		if (InputManager.Instance.InputSwitched) {
			return;
		}
		if (selectObject != null) {
			selectObject.SetActive(true);
		}
		StartCoroutine(PlaySelectAnimation());
		this.Selected = true;
	}

	public void OnDeselect(BaseEventData eventData) {
		if (selectObject != null) {
			selectObject.SetActive(false);
		}
		transform.localScale = this.InitialScale;
		this.Selected = false;
	}

	public void ResetButton() {
		transform.localScale = this.InitialScale;
		if (selectObject != null) {
			selectObject.SetActive(false);
		}
		this.Selected = false;
	}

	#endregion


	#region Private Methods

	private IEnumerator PlayEnableAnimation() {
		if (ScaleAnimation.EnableCurve.length > 0) {
			ScaleAnimation.EnableAnimationInProgress = true;
			float time = 0f;
			float duration = ScaleAnimation.EnableCurve.keys[ScaleAnimation.EnableCurve.length - 1].time;
			while (time < duration) {
				float scale = ScaleAnimation.EnableCurve.Evaluate(time);
				transform.localScale = Vector3.one * scale;
				time += Time.unscaledDeltaTime;
				yield return null;
			}
			float finalScale = ScaleAnimation.EnableCurve.Evaluate(duration);
			transform.localScale = Vector3.one * finalScale;
			ScaleAnimation.EnableAnimationInProgress = false;
		}
	}

	private void PlayIdleAnimation() {
		if (!ScaleAnimation.EnalbeIdleAnimation) {
			return;
		}
		if (ScaleAnimation.AnimationInProgress) {
			return;
		}
		if (ScaleAnimation.IdleCurve.length > 0) {
			float pingPongTime = Mathf.PingPong(Time.unscaledTime, ScaleAnimation.IdleCurve.keys[ScaleAnimation.IdleCurve.length - 1].time);
			float scale = ScaleAnimation.IdleCurve.Evaluate(pingPongTime);
			transform.localScale = new Vector3(scale, scale, scale);
		}
	}

	private IEnumerator PlaySelectAnimation() {		
		if (ScaleAnimation.SelectCurve.length > 0) {
			ScaleAnimation.SelectAnimationInProgress = true;
			float time = 0f;
			float duration = ScaleAnimation.SelectCurve.keys[ScaleAnimation.SelectCurve.length - 1].time;
			while (time < duration) {
				float scale = ScaleAnimation.SelectCurve.Evaluate(time);
				transform.localScale = Vector3.one * scale;
				time += Time.unscaledDeltaTime;
				yield return null;
			}
			float finalScale = ScaleAnimation.SelectCurve.Evaluate(duration);
			transform.localScale = Vector3.one * finalScale;
			ScaleAnimation.SelectAnimationInProgress = false;
		}		
	}

	private void PlayIdleSelectAnimation() {
		if (!this.Selected) {
			return;
		}
		if (ScaleAnimation.AnimationInProgress) {
			return;
		}
		if (ScaleAnimation.IdleSelectCurve.length > 0) {
			float pingPongTime = Mathf.PingPong(Time.unscaledTime, ScaleAnimation.IdleSelectCurve.keys[ScaleAnimation.IdleSelectCurve.length - 1].time);
			float scale = ScaleAnimation.IdleSelectCurve.Evaluate(pingPongTime);
			transform.localScale = new Vector3(scale, scale, scale);
		}
	}

	private IEnumerator PlayClickAnimation() {
		if (ScaleAnimation.ClickCurve.length > 0) {
			ScaleAnimation.ClickAnimationInProgress = true;
			float time = 0f;
			float duration = ScaleAnimation.ClickCurve.keys[ScaleAnimation.ClickCurve.length - 1].time;
			while (time < duration) {
				float scale = ScaleAnimation.ClickCurve.Evaluate(time);
				transform.localScale = Vector3.one * scale;
				time += Time.unscaledDeltaTime;
				yield return null;
			}
			float finalScale = ScaleAnimation.ClickCurve.Evaluate(duration);
			transform.localScale = Vector3.one * finalScale;
			ScaleAnimation.ClickAnimationInProgress = true;
		}
	}

	private void PlayIdleClickAnimation() {
		if (!this.Clicked) {
			return;
		}
		if (ScaleAnimation.AnimationInProgress) {
			return;
		}
		if (ScaleAnimation.IdleClickCurve.length > 0) {
			float pingPongTime = Mathf.PingPong(Time.unscaledTime, ScaleAnimation.IdleClickCurve.keys[ScaleAnimation.IdleClickCurve.length - 1].time);
			float scale = ScaleAnimation.IdleClickCurve.Evaluate(pingPongTime);
			transform.localScale = new Vector3(scale, scale, scale);
		}
	}

	#endregion

}
