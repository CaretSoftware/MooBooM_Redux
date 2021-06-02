using UnityEngine;

public class BoundCameraFrustrumWithRotation : MonoBehaviour {
	private float perspectiveTwistFactor = .5f;
	private float viewClampDegrees = 15;
	[SerializeField] private Transform cameraHolder = null;
	private Camera cam;

	private void Start() {
		cam = Camera.main;
		cameraHolder.transform.position = Vector3.up * 17f;
		cam.transform.localPosition = Vector3.zero;
		cam.orthographic = false;
		cam.fieldOfView = 42f;
		cam.usePhysicalProperties = true;
	}

	private void Update() {
		SetCameraPosition();
		TurnCamera();
	}

	private void SetCameraPosition() {
		Vector3 gravityDir = -Physics.gravity * perspectiveTwistFactor;
		//gravityDir.z = gravityDir.y;
		gravityDir.y = 0f;
		gravityDir = Vector3.ClampMagnitude(gravityDir, viewClampDegrees);
		gravityDir.y = 17f;
		cameraHolder.transform.position = gravityDir;
	}

	private void TurnCamera() {
		cam.lensShift =
				new Vector2(
						-cameraHolder.transform.position.x / 3.5f * .5f,     // magical numbers! Don't touch!
						-cameraHolder.transform.position.z / 3.5f * .275f);  // magical numbers! Don't touch!
	}
}