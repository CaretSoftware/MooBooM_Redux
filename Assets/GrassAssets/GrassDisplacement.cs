using UnityEngine;

public class GrassDisplacement : MonoBehaviour {
	[SerializeField] private Transform cow;
	[SerializeField] private Material grassMaterial;
	private Texture displacementTex;
	private bool cowGrounded;
	private Vector2 offset;
	private Vector2 relativePos;
	private string windTex = "_WindTexture";
	private float WIDTH;
	private float HEIGHT;


	private void Start() {
		WIDTH = 5f * transform.localScale.x;
		HEIGHT = 5f * transform.localScale.z;

		cow = FindObjectOfType<Cow>().transform;
		displacementTex = grassMaterial.GetTexture("_WindTexture");
	}

	private void Update() {
		Trample();
		//Move();
	}

	private void Move() {
		Vector3 move;
		move.x = Mathf.Sin(Time.time);
		move.y = 0;
		move.z = Mathf.Sin(Time.time * 2) / 2;
		cow.position = move * 2f;
	}

	private void Trample() {
		relativePos.x = Mathf.InverseLerp(WIDTH, -WIDTH, cow.position.x);
		relativePos.y = Mathf.InverseLerp(HEIGHT, -HEIGHT, cow.position.z);

		offset.x = Mathf.Lerp(0f, 1f, relativePos.x);
		offset.y = Mathf.Lerp(0f, 1f, relativePos.y);
		grassMaterial.SetTextureOffset(windTex, offset);
	}
}
