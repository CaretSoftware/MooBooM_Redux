using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DazedStarAnimation : MonoBehaviour {
	[SerializeField] private Image[] stars = null;
	[SerializeField] private float speed = 1f;
	[SerializeField] private float radius = 200f;
	[SerializeField] private float tilt = 200f;
	[SerializeField] private Vector2 anchor;

	private void Start() {
		for (int i = 0; i < stars.Length; i++) {
			StartCoroutine(Rotate(i));
		}
	}

	private IEnumerator Rotate(int i) {
		float t;
		while (true) {
			t = Time.time;
			float x = Mathf.Sin( ( t * speed ) + ( 2f * i ) )  * radius ;
			float y = Mathf.Sin( ( t * speed ) + ( 2f * i ) - .5f ) * tilt ; 
			stars[i].rectTransform.anchoredPosition = anchor + new Vector2(x, y);
			if (x < -radius + 5f ) {
				stars[i].transform.SetAsLastSibling();
			} else if (x > radius - 5f ) {
				stars[i].transform.SetAsFirstSibling();
			}
			yield return null;
		}
	}
}
