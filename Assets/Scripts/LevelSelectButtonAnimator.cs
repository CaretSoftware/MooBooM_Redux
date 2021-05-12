using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButtonAnimator : MonoBehaviour {

	[SerializeField] private RectTransform levelSelect;
	[SerializeField] private RectTransform buttons;
	[SerializeField] private Image image;
	[SerializeField] private RectTransform buttonText;
	[SerializeField] private CanvasGroup buttonTextGroup;

	private readonly Vector2 levelSelectOpenPos = new Vector2(222.43f, 275f);
	private readonly Vector2 levelSelectClosedPos = new Vector2(0f, 4f);
	private readonly Vector2 levelSelectSizeDeltaOpen = new Vector2(740f, 740f);
	private readonly Vector2 levelSelectSizeDeltaClosed = new Vector2(280f, 280f);
	private readonly float pixelsPerUnitOpen = 6;
	private readonly float pixelsPerUnitClosed = 1;
	private readonly Vector2 buttonsSizeDeltaOpen = new Vector2(480f, 480f);
	private readonly Vector2 buttonsSizeDeltaClosed = new Vector2(20f, 20f);
	private readonly Vector2 buttonTextScaleOpen = new Vector2(.5f, .5f);
	private readonly Vector2 buttonTextScaleClosed = new Vector2(.12f, .12f);
	private readonly float alphaOutTarget = -8f;
	private bool isOpen;

	public void Animate() {
		StopAllCoroutines();
		StartCoroutine(OpenLevelSelect(!isOpen));
	}

    private IEnumerator OpenLevelSelect(bool open) {
		isOpen = open;
		float t = open ? 0f : 1f;
		float end = open ? 1f : 0f;
		float e;
		while (open ? t < end : t > end) {
			e = open ? Ease.EaseOutElastic(t) : Ease.EaseInBounce(t);
			levelSelect.anchoredPosition =
					Vector2.LerpUnclamped(levelSelectClosedPos, levelSelectOpenPos, e);
			levelSelect.sizeDelta =
					Vector2.LerpUnclamped(levelSelectSizeDeltaClosed, levelSelectSizeDeltaOpen, e);
			image.pixelsPerUnitMultiplier =
					Mathf.LerpUnclamped(pixelsPerUnitClosed, pixelsPerUnitOpen, e);
			buttons.sizeDelta =
					Vector2.LerpUnclamped(buttonsSizeDeltaClosed, buttonsSizeDeltaOpen, e);
			buttonText.localScale =
					Vector2.LerpUnclamped(buttonTextScaleClosed, buttonTextScaleOpen, e);
			float fade = Ease.EaseInExpo(t * 2f);
			buttonTextGroup.alpha =
					open ? Mathf.Lerp(0f, 1f, fade) : Mathf.Lerp(alphaOutTarget, 1f, fade);
			t = open ? t += Time.deltaTime : t -= Time.deltaTime * 2f;
			yield return null;
		}

		levelSelect.anchoredPosition = open ? levelSelectOpenPos : levelSelectClosedPos;
		levelSelect.sizeDelta = open ? levelSelectSizeDeltaOpen : levelSelectSizeDeltaClosed;
		image.pixelsPerUnitMultiplier = open ? pixelsPerUnitOpen : pixelsPerUnitClosed;
		buttons.sizeDelta = open ? buttonsSizeDeltaOpen : buttonsSizeDeltaClosed;
		buttonText.localScale = open ? buttonTextScaleOpen : buttonTextScaleClosed;
		buttonTextGroup.interactable = open;
		buttonTextGroup.blocksRaycasts = open;
	}
}
