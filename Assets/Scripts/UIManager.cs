using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject optionsPanel;    //temp private because don't have an options panel yet
    private WinScreen winScreen;
    private GameObject levelSelectCanvas;
    private GameController gameController;
    [SerializeField] private Button playNextButton;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        winScreen = FindObjectOfType<WinScreen>();
        //winScreen.gameObject.SetActive(false);
		//disableEndOfLevelCanvas(true);
		//levelSelectCanvas = GameObject.Find("LevelSelectCanvas");
		//levelSelectCanvas.SetActive(false);
	}

    public void OptionsPanel()
    {
        Time.timeScale = 0;
        optionsPanel.SetActive(true);
    }

    public void ReturnToGame()
    {
        Time.timeScale = 1;
        optionsPanel.SetActive(false);
    }

    public void EndOfLevel()
    {
        //winScreen.gameObject.SetActive(true);
        winScreen.Display();
        //DisplayRightAmountOfStars();
        

        if (!gameController.isLevelWon())
        {
            DisablePlayNextButton();
            //DisplayLoosingCross();
        } 

    }
    
    public void openLevelSelect() {
        levelSelectCanvas.SetActive(true);
    }

    public void closeLevelSelect() { 
        levelSelectCanvas.SetActive(false);
    }

    public void DisableEndOfLevelCanvas() {
        if(winScreen != null)    //In overworld there is not endOfLevelCanvas, so need this check here
            winScreen.gameObject.SetActive(false);

    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void DisplayRightAmountOfStars() {
        int starsEarned = gameController.GetStarsCount();

        if (gameController.isLevelWon())
        {
            for (int i = 1; i <= starsEarned; i++)
            {
                //winScreen.transform.GetChild(0).Find("Star" + i).gameObject.SetActive(true);
            }
        }
    }

    private void DisplayLoosingCross()
    {
		winScreen.transform.GetChild(0).Find("LoosingCross").gameObject.SetActive(true);
	}

    private void DisablePlayNextButton() {
            playNextButton.interactable = false;
    }
}
