using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassDisplacement : MonoBehaviour {
	[SerializeField] private GameObject cow;
	[SerializeField] private Material grassMaterial;
	private Texture displacementTex;
	private bool cowGrounded;
	private Vector2 offset;
	private Vector2 grassPos;
	private Vector2 relativePos;
	private string windTex = "_WindTexture";

	private void Start() {
		grassPos = transform.position;
		displacementTex = grassMaterial.GetTexture("_WindTexture");
	}

	private void Update() {
		Trample();
		Move();
	}

	private void Move() {
		Vector3 move;
		move.x = Mathf.Sin(Time.time);
		move.y = 0;
		move.z = Mathf.Sin(Time.time * 2) / 2;
		cow.transform.position = move * 2f;
	}

	private void Trample() {
		if (true) {//cowGrounded) {
			relativePos.x = Mathf.InverseLerp(5f, -5f, cow.transform.position.x);
			relativePos.y = Mathf.InverseLerp(5f, -5f, cow.transform.position.z);

			offset.x = Mathf.Lerp(0f, 1f, relativePos.x);
			offset.y = Mathf.Lerp(0f, 1f, relativePos.y);
			grassMaterial.SetTextureOffset(windTex, offset);
		}
	}

	private void OnCollisionEnter(Collision collision) {
		cowGrounded = true;
	}

	private void OnCollisionExit(Collision collision) {
		cowGrounded = false;
	}
}
