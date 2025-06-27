using UnityEngine;

public class RodLineComponent : MonoBehaviour
{
	[SerializeField]
	private Transform startPoint;

	[SerializeField]
	private int segmentCount = 20;

	[SerializeField]
	private float curveHeight = 2f;

	[SerializeField]
	private float peakPosition;

	[SerializeField]
	private LineRenderer lineRenderer;

	[SerializeField]
	private Vector3 curveDirection = Vector3.right;

	[SerializeField, Range(0f, 1f)]
	private float curveAmount = 1f;

	public float CurveAmount {
		get => curveAmount;
		set => curveAmount = Mathf.Clamp01(value);
	}

	public bool IsStraight {
		get;
		set;
	}

	public bool IsVisible {
		get;
		set;
	}

	public void Start() {
		GameManager.Instance.LevelController.OnFishSpawned -= AssignToFish;
		GameManager.Instance.LevelController.OnFishSpawned += AssignToFish;
		lineRenderer.positionCount = segmentCount;
	}

	public void OnDestroy() {
		GameManager.Instance.LevelController.OnFishSpawned -= AssignToFish;
	}

	public void Update() {
		if (GameManager.Instance.CurrentFish == null) {
			this.IsVisible = false;
		} else {
			this.IsVisible = true;
		}
		if (this.IsVisible) {
			DrawCurve();
		} else {
			lineRenderer.enabled = false;
		}
	}

	private void DrawCurve() {
		lineRenderer.enabled = true;

		Vector3 p0 = startPoint.position;
		Vector3 p2 = GameManager.Instance.CurrentFish.LineEnd.transform.position;

		Vector3 control = Vector3.Lerp(p0, p2, peakPosition) + curveDirection.normalized * curveHeight;

		for (int i = 0; i < segmentCount; i++) {
			float t = i / (float)(segmentCount - 1);

			Vector3 curved = CalculateQuadraticBezierPoint(t, p0, control, p2);
			Vector3 straight = Vector3.Lerp(p0, p2, t);

			// Blend based on curveAmount (0 = straight, 1 = fully curved)
			Vector3 blended = Vector3.Lerp(straight, curved, curveAmount);

			lineRenderer.SetPosition(i, blended);
		}
	}

	private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2) {
		return Mathf.Pow(1 - t, 2) * p0 + 2 * (1 - t) * t * p1 + Mathf.Pow(t, 2) * p2;
	}

	private void AssignToFish() {
		GameManager.Instance.CurrentFish.RodLineComponent = this;
		lineRenderer.enabled = true;

		Vector3 p0 = startPoint.position;
		Vector3 p2 = GameManager.Instance.CurrentFish.LineEnd.transform.position;

		Vector3 control = Vector3.Lerp(p0, p2, peakPosition) + curveDirection.normalized * curveHeight;

		for (int i = 0; i < segmentCount; i++) {
			float t = i / (float)(segmentCount - 1);

			Vector3 curved = CalculateQuadraticBezierPoint(t, p0, control, p2);
			Vector3 straight = Vector3.Lerp(p0, p2, t);

			// Blend based on curveAmount (0 = straight, 1 = fully curved)
			Vector3 blended = Vector3.Lerp(straight, curved, 0);

			lineRenderer.SetPosition(i, blended);
		}
	}
}
