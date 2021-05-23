using UnityEngine;
using UnityEngine.UI;

public class RotateGlobeTexture : MonoBehaviour {

    [SerializeField] private float scrollSpeed = -.25f;
    [SerializeField] private RawImage img;
    private float offset;

	private void Update() {
        offset = Time.time * scrollSpeed;
        img.uvRect = new Rect(offset, 0f, 1f, 1f);
    }
}