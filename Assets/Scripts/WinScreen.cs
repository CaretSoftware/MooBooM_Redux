using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour {
	private const int numOfBombCountInRow = 8;
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

	private GameController gameController;
	private UIManager uiManager;

	private void Awake() {
		gameController = FindObjectOfType<GameController>();
		uiManager = FindObjectOfType<UIManager>();
	}

	//private void OnEnable() {
	//	Display();
	//}

	public void OpenLevelSelect() {
		PlayClickSound();

		levelSelect.DisableLockedLevels();

		if (SaveManager.CheckIfFileExists()) {
			levelSelect.DisplayEarnedStarsOnButtons(SaveManager.getChapterNumber());
		}
	}

	public void ReplayLevel() {
		PlayClickSound();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		//levelSelect.ReplayLevel();
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
		PlayClickSound();
		levelSelect.loadLevel(level);
	}

	public void Display() {
		if (gameController.GetMineExploded()) {
			DisplayGameOverImage();
			StartCoroutine(ShowPickedUpBombs(false));
		} else if (gameController.isCowAlreadyHurt()) {
			Debug.Log("X MINE");
			DisplayGameOverImage(true, true);
			StartCoroutine(ShowPickedUpBombs(false));
		} else {
			DisplayStars();
			StartCoroutine(ShowPickedUpBombs());
		}
	}

	private void DisplayStars() {
		//int starsEarned = 2;//gameController.getStarsCount(); FIXME?

		DisplayGameOverImage(false);
		DisplayStarHolder(true);
		for (int i = 0; i < stars.Length; i++) {
			stars[i].SetActive(true);//i < starsEarned); FIXME?
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

	private IEnumerator ShowPickedUpBombs(bool won = true) {
		int numTotalBombs = gameController.getStartngBombCount();
		int numPickedUpBombs = gameController.getPickedUpBombsCount();

		for (int i = 1; i < bombCountRows.Length; i++) {
			bombCountRows[i].SetActive(numTotalBombs > numOfBombCountInRow * i);
		}

		for (int i = 0; i < numTotalBombs; i++) {
			bombCountHolders[i].SetActive(true);
		}

		yield return new WaitForSecondsRealtime(.5f);
		float t = 0f;
		float e;
		int star = 3;

		if (won) {
			for (int i = 0; i < numTotalBombs; i++) {
				bombCount[i].enabled = i < numPickedUpBombs;
				t += 1f / numTotalBombs;
				e = Mathf.Lerp(Ease.EaseInQuint(1f - t), Ease.EaseInQuint(t), t) * .25f;

				if (i >= numPickedUpBombs) {
					//RectTransform explosionPos = bombCount[i].rectTransform.anchoredPosition;
					StartCoroutine(AnimateBombCountExplosion(bombCount[i].rectTransform, i));
					float wait;
					wait = 1.5f / (numTotalBombs - numPickedUpBombs);
					if (--star >= 0) {
						StartCoroutine(AimateStarExplosion(stars[star]));
					}
					yield return new WaitForSecondsRealtime(wait);
				} else {
					yield return new WaitForSecondsRealtime(e);
				}
			}
		}
	}

	private IEnumerator AnimateBombCountExplosion(RectTransform rect, int bombNum) {
		Image[] puffArray = new Image[5];
		Vector2[] randomUnitCirclePos = new Vector2[puffArray.Length];
		for (int i = 0; i < puffArray.Length; i++) {

			puffArray[i] = Instantiate(puffClouds[Random.Range(0, puffClouds.Length)]);
			puffArray[i].transform.SetParent(rect.parent, false);
			puffArray[i].transform.SetAsFirstSibling();
			randomUnitCirclePos[i] = rect.anchoredPosition + Random.insideUnitCircle * 60f;
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
				puffArray[i].rectTransform.anchoredPosition = Vector2.LerpUnclamped(rect.anchoredPosition, randomUnitCirclePos[i], e);
				puffArray[i].color = Color.Lerp(startColor, endColor, e);
				bombCount[bombNum].color = Color.Lerp(Color.white, endColor, c);
				bombCount[bombNum].rectTransform.localScale = Vector2.LerpUnclamped(Vector2.zero, Vector2.one * 2f, x);
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

	private void PlayClickSound() {
		SoundController.onlySoundController?.PlaySound("ButtonClick");
	}
}