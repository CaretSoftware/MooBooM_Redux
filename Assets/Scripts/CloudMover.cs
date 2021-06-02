using UnityEngine;

public class CloudMover : MonoBehaviour {

	[SerializeField] private float speed = 1f;
	private float cameraWidth;
	private float margin = 2f;

	private void Start() {
		Camera cam = FindObjectOfType<Camera>();
		Vector3 leftCorner = cam.ViewportToWorldPoint(new Vector3(0f, 0f, cam.transform.position.y));
		Vector3 rightCorner = cam.ViewportToWorldPoint(new Vector3(1f, 0f, cam.transform.position.y));

		cameraWidth = rightCorner.x - leftCorner.x + margin;
	}

	private void Update() {
		transform.Translate(Vector3.left * Time.deltaTime * speed);
    }

	private void OnBecameInvisible() {
		transform.position += Vector3.right * cameraWidth;
	}
}
