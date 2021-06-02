using UnityEngine;

public class FuseSegment : MonoBehaviour {

	[SerializeField] private GameObject fuseSegmentPrefab;
	private GameObject instance;

	public void SpawnFuseSegment(int numSegmentsLeft) {
		instance = this.gameObject;
		//Debug.Log(numSegmentsLeft);
		if (numSegmentsLeft >= 0) {
			FuseSegment next =
					Instantiate(
							instance,
							this.gameObject.transform).GetComponent<FuseSegment>();
			next.GetComponent<HingeJoint>().connectedBody = GetComponent<Rigidbody>();
			next.GetComponent<HingeJoint>().anchor = Vector3.up * 2f;
		}
	}

	public void SetYScale(float y) {
		Vector3 size = transform.localScale;
		size.y = y;
		transform.localScale = size;
	}

}
