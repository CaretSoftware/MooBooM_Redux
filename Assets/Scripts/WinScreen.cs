using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour {
	private const int numOfBombCountInRow = 8;
	[SerializeField] private GameObject[] stars;
	[SerializeField] private GameObject starHolder;
	[SerializeField] private GameObject gameOverImage;
	[SerializeField] private LevelSelect levelSelect;
	[SerializeField] private Image[] bombCount;
	[SerializeField] private GameObject[] bombCountHolders;
	[SerializeField] private GameObject[] bombCountRows;

	private GameController gameController;
	private UIManager uiManager;

	private void Awake() {
		gameController = FindObjectOfType<GameController>();
		uiManager = FindObjectOfType<UIManager>();
	}

	private void Start() {
		OnEnable();	// FIXME
	}

	private void OnEnable() {
		Display();
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

	private void Display() {
		if (true) {//gameController.isLevelWon()) { FIXME
			DisplayStars();
			ShowPickedUpBombs();
		} else {
			DisplayGameOverImage();
			ShowPickedUpBombs(false);
		}
	}

	private void DisplayStars() {
		int starsEarned = 2;//gameController.getStarsCount(); FIXME

		DisplayGameOverImage(false);
		DisplayStarHolder(true);
		for (int i = 0; i < stars.Length; i++) {
			stars[i].SetActive(i < starsEarned);
		}
	}

	public void DisplayGameOverImage(bool show = true) {
		DisplayStarHolder(false);
		gameOverImage.SetActive(show);
	}

	private void DisplayStarHolder(bool show) {
		starHolder.SetActive(show);
	}

	private void ShowPickedUpBombs(bool won = true) {
		int numTotalBombs = 9;// gameController.getStartngBombCount();
		int numPickedUpBombs = 8;// gameController.getPickedUpBombsCount();

		for (int i = 1; i < bombCountRows.Length; i++) {
			bombCountRows[i].SetActive(numTotalBombs > numOfBombCountInRow * i);
		}

		for (int i = 0; i < numTotalBombs; i++) {
			bombCountHolders[i].SetActive(true);
		}

		for (int i = 0; i < numPickedUpBombs; i++) {
			bombCount[i].enabled = won;
		}
	}

	private void PlayClickSound() {
		SoundController.onlySoundController?.PlaySound("ButtonClick");
	}
}