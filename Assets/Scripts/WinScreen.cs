using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour {

	[SerializeField] GameObject[] stars;
	[SerializeField] GameObject starHolder;
	[SerializeField] GameObject gameOverImage;
	[SerializeField] private LevelSelect levelSelect;

	//private void Start() {
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
	}

	public void GoToLevel(int level) {
		PlayClickSound();
		levelSelect.loadLevel(level);
	}

	public void DisplayStars(int score) {
		DisplayGameOverImage(false);
		DisplayStarHolder(true);
		for (int i = 0; i < stars.Length; i++) {
			stars[i].SetActive(i < score);
		}
	}

	private void DisplayStarHolder(bool show) {
		starHolder.SetActive(show);
	}

	public void DisplayGameOverImage(bool show = true) {
		DisplayStarHolder(true);
		gameOverImage.SetActive(show);
	}

	public void ShowPickedUpBombs(int numBombs) {

	}

	private void PlayClickSound() {
		SoundController.onlySoundController?.PlaySound("ButtonClick");
	}
}