using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour {
	private const int numOfBombCountInRow = 8;
	private const int maxPossibleStars = 3;
	[SerializeField] private CanvasGroup exitButton;
	[SerializeField] private CanvasGroup display;
	[SerializeField] private Transform canvas;
	[SerializeField] private GameObject[] stars;
	[SerializeField] private GameObject starHolder;
	[SerializeField] private GameObject gameOverDisplay;
	[SerializeField] private Image gameOverImage;
	[SerializeField] private Sprite bombSprite;
	[SerializeField] private LevelSelect levelSelect;
	[SerializeField] private Image[] bombCount;
	[SerializeField] private GameObject[] bombCountHolders;
	[SerializeField] private GameObject[] bombCountRows;
	[SerializeField] private Image[] puffClouds;
	private bool opened;
	private GameController gameController;
	private UIManager uiManager;


	private void Start() {
		gameController = FindObjectOfType<GameController>();
		uiManager = FindObjectOfType<UIManager>();
	}

	public void OpenLevelSelect() {
		PlayClickSound();

		levelSelect.DisableLockedLevels();

		if (SaveManager.CheckIfFileExists()) {
			levelSelect.DisplayEarnedStarsOnButtons(SaveManager.getChapterNumber());
		}
	}

	public void ReplayLevel() {
		PlayClickSound();
		levelSelect.ReplayLevel();
	}

	public void PlayNextLevel() {
		PlayClickSound();
		levelSelect.PlayNextLevel();
	}

	public void GoToMainMenu() {
		PlayClickSound();
		uiManager.GoMainMenu();
	}

	public void GoToLevel(int level) {
		Debug.Log("go to level " + level);
		PlayClickSound();
		levelSelect.LoadLevel(level);
	}

	public void Display() {
		if (!opened) {
			opened = true;
			StartCoroutine(FadeInDisplay());
		}
	}

	private IEnumerator FadeInDisplay() {
		bool survived = false;

		display.interactable = true;
		display.blocksRaycasts = true;
		exitButton.interactable = true;
		exitButton.blocksRaycasts = true;
		ShowBombCounterPits();

		if (gameController.GetMineExploded()) {
			yield return new WaitForSecondsRealtime(1f);
			DisplayGameOverImage();
		} else if (gameController.isCowAlreadyHurt()) {
			yield return new WaitForSecondsRealtime(1f);
			DisplayGameOverImage(true, true);
		} else {
			yield return new WaitForSecondsRealtime(.5f);
			DisplayStars();
			survived = true; 
		}

		float t = 0;
		while(t < 1f) {
			t += Time.deltaTime;
			display.alpha = t;
			exitButton.alpha = t;
			yield return null;
		}

		display.alpha = 1f;
		exitButton.alpha = 1f;

		yield return new WaitForSecondsRealtime(.5f);

		if (survived) {
			StartCoroutine(ShowPickedUpBombs());
		}
	}

	private void DisplayStars() {
		DisplayGameOverImage(false);
		DisplayStarHolder(true);
		for (int i = 0; i < stars.Length; i++) {
			stars[i].SetActive(true);
		}
	}

	public void DisplayGameOverImage(bool show = true, bool bomb = false) {
		DisplayStarHolder(false);
		if (bomb) {
			gameOverImage.sprite = bombSprite;
		}
		gameOverDisplay.SetActive(show);
	}

	private void DisplayStarHolder(bool show) {
		starHolder.SetActive(show);
	}

	private void ShowBombCounterPits() {
		int numTotalBombs = gameController.getStartingBombCount();

		for (int i = 1; i < bombCountRows.Length; i++) {
			bombCountRows[i].SetActive(numTotalBombs > numOfBombCountInRow * i);
		}

		for (int i = 0; i < numTotalBombs; i++) {
			bombCountHolders[i].SetActive(true);
		}
	}

	private IEnumerator ShowPickedUpBombs() {
		int numTotalBombs = gameController.getStartingBombCount();
		int numPickedUpBombs = gameController.getPickedUpBombsCount();
		int numStarsEarned = gameController.GetStarsCount();

		float t = 0f;
		float e;
		int star = 3;
		int starPulse = 0;

		int numCycles = Mathf.Max(numTotalBombs, maxPossibleStars);
		int startStarPulseAt = Mathf.Clamp(numTotalBombs - maxPossibleStars, 0, int.MaxValue);
		int startExplosionAt = numCycles - Mathf.Max(
						maxPossibleStars - numStarsEarned,
						numTotalBombs - numPickedUpBombs);

		yield return new WaitForSecondsRealtime(.5f);

		for (int i = 0; i < numCycles; i++) {
			t += 1f / numCycles;
			e = Mathf.Lerp(Ease.EaseInQuint(.95f - t), Ease.EaseInQuint(t), t);
			bombCount[i].enabled = i < numPickedUpBombs;

			if (gameController.GetStarsCount() == maxPossibleStars
					&& i >= startStarPulseAt
					&& starPulse < stars.Length) {
				StartCoroutine(AnimateStarPulse(starPulse++));
			}
			if (i >= startExplosionAt) {
				if (i < bombCount.Length) {
					StartCoroutine(AnimateBombCountExplosion(bombCount[i].rectTransform, i));
				}
				if (--star >= 0) {
					StartCoroutine(AimateStarExplosion(stars[star]));
					PlayStarExplosion();
				}
				PlayExplosionSound();
			} else {
				PlayBombPickupCounter();
			}
			yield return new WaitForSecondsRealtime(e);
		}
	}

	private IEnumerator AnimateStarPulse(int starNum) {
		//Debug.Log("star Num " + starNum);
		float t = 0f;
		Vector2 startScale = stars[starNum].transform.localScale;
		float endScaleFactor = 1.2f;
		float e = 0f;

		while (t < 1f) {
			t += Time.deltaTime;
			e = Mathf.Lerp(Ease.EaseInQuint(1f - t), 0f, t);
			stars[starNum].transform.localScale =
					Vector2.Lerp(
							startScale,
							startScale * endScaleFactor,
							e);
			yield return null;
		}
	}

	private IEnumerator AnimateBombCountExplosion(RectTransform rect, int bombNum) {
		Image[] puffArray = new Image[5];
		Vector2[] randomUnitCirclePos = new Vector2[puffArray.Length];
		for (int i = 0; i < puffArray.Length; i++) {

			puffArray[i] = Instantiate(puffClouds[Random.Range(0, puffClouds.Length)]);
			puffArray[i].transform.SetParent(rect.parent, false);
			puffArray[i].transform.SetAsFirstSibling();
			randomUnitCirclePos[i] = rect.anchoredPosition + Random.insideUnitCircle * 80f;
		}
		float t = 0f;
		float e;
		float c;
		float x;
		Color startColor = new Color(0f,0f,0f,1f);
		Color endColor = new Color(1f, 1f, 1f, 0f);

		while (t < 1f) {
			e = Ease.EaseOutExpo(t);
			c = Ease.EaseInExpo(t);
			x = Ease.EaseOutBack(t);
			bombCount[bombNum].enabled = true;
			for (int i = 0; i < puffArray.Length; i++) {
				puffArray[i].rectTransform.anchoredPosition =
						Vector2.LerpUnclamped(rect.anchoredPosition, randomUnitCirclePos[i], e);
				puffArray[i].color = Color.Lerp(startColor, endColor, e);
				bombCount[bombNum].color = Color.Lerp(Color.white, endColor, c);
				bombCount[bombNum].rectTransform.localScale =
						Vector2.LerpUnclamped(Vector2.zero, Vector2.one * 2f, x);
			}
			t += Time.deltaTime;
			yield return null;
		}
		for (int i = 0; i < puffArray.Length; i++) {
			puffArray[i].color = Color.clear;
		}
		bombCount[bombNum].color = Color.clear;
	}

	private IEnumerator AimateStarExplosion(GameObject star) {
		float t = 0f;
		float e;
		RectTransform starRect = star.GetComponent<RectTransform>();
		Vector2 starStartPos = starRect.anchoredPosition + Vector2.up
				* starRect.parent.GetComponent<RectTransform>().anchoredPosition.y;
		star.transform.SetParent(canvas, false);
		star.transform.SetAsLastSibling();
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
				starRect,
				Camera.main.ViewportToScreenPoint(Vector2.zero),
				null,
				out Vector2 outVec);
		Quaternion startRotation = starRect.rotation;
		Quaternion endRotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-40f, 40f)));
		Vector2 starEndPos = new Vector2(starStartPos.x, outVec.y - starRect.sizeDelta.y);
		while (t < 1f) {
			e = Ease.EaseInBack(t);
			starRect.anchoredPosition = Vector2.LerpUnclamped(starStartPos, starEndPos, e);
			starRect.rotation = Quaternion.Lerp(startRotation, endRotation, t);
			t += Time.deltaTime;
			yield return null;
		}
		starRect.anchoredPosition = starEndPos;
	}

	public void PlayClickSound() {
		SoundController.onlySoundController.PlaySound("ButtonClick");
	}

	public void PlayExplosionSound() {
		SoundController.onlySoundController.PlaySound("MiniExplosion");
	}

	private void PlayStarExplosion() {
		SoundController.onlySoundController.PlaySound("StarExplosion");
	}

	public void PlayPickupSound() {
		SoundController.onlySoundController.PlaySound("PickupBomb");
	}

	private void PlayBombPickupCounter() {
		SoundController.onlySoundController.PlaySound("BombPickupCounter");
	}

	private void OnDestroy() {
		StopAllCoroutines();
	}
}