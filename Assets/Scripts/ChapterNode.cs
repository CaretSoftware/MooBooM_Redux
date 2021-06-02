using System.Collections.Generic;
using UnityEngine;

public class ChapterNode : MonoBehaviour {

	[SerializeField] private int chapterNumber = 1;
	private Transform cowTransform;
	private Rigidbody cowRB;
	private Cow cowScript;
	[SerializeField][Range(1f, 50f)] private float attractionForce = 30f;
	private float distance;
	private Vector3 direction;
	private float timer;
	[SerializeField][Range(0f, 1f)] private float timeUntilChapterSelect = .5f;
	private bool cameFromNode;
	//private TransitionEffect transition;
	[SerializeField] private GameObject levelSelect;
	private bool transitioned;
	private static List<ChapterNode> chapterNodes = new List<ChapterNode>();
	[SerializeField] private OverWorldTransition overWorldTransition;


	private void Awake() {
		chapterNodes.Add(this);
	}

	private void Start() {
		//transition = FindObjectOfType<TransitionEffect>();
		//overWorldTransition = FindObjectOfType<OverWorldTransition>();
		cowScript = FindObjectOfType<Cow>();
		cowTransform = cowScript.transform;
		cowRB = cowTransform.GetComponent<Rigidbody>();
		if ((transform.position - cowTransform.position).magnitude < .5f) {
			cameFromNode = true;
		}
		Invoke("ReleaseCow", .5f);
	}

	private void ReleaseCow() {

		cowScript.Release();
	}

	private void FixedUpdate() {
		cameFromNode = CameFromNode();
		if (cameFromNode) {
			return;
		}

		direction = transform.position - cowTransform.position;
		distance = direction.magnitude;
		direction = direction.normalized;

		if (distance > 1f) {
			if (cameFromNode) {
				cowRB.drag = 1f;
			}
			cameFromNode = false;
			timer = 0f;
		}
		if (distance < .5f) {
			cowRB.AddForce(direction * attractionForce);
			if (distance < .5f) {
				cowRB.drag = 3f;
				timer += Time.fixedDeltaTime;
			}
			if (timer > timeUntilChapterSelect && !transitioned) {
				cameFromNode = true;
				transitioned = true;
				cowRB.drag = 3f;
				cowRB.angularDrag = 2f;
				cowRB.useGravity = false;
				SaveManager.SetChapterNumber(chapterNumber);
				levelSelect.SetActive(true);
				overWorldTransition.Animate();

			}
		}
	}

	private bool CameFromNode() {
		if (cameFromNode) {
			if ((transform.position - cowTransform.position).magnitude > 1f) {
				//transitioned = false;
				cameFromNode = false;
			}
		}
		return cameFromNode;
	}

	public static void ResetNodes() {
		for (int i = 0; i < chapterNodes.Count; i++) {
			chapterNodes[i].ResetNode();
		}
	}

	private void ResetNode() {
		transitioned = false;
		cowRB.angularDrag = 0f;
	}
}