using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    private GameController gameController;
    public List<Button> buttonList;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void OnEnable()
    {
        DisableLockedLevels();

        if(SaveManager.CheckIfFileExists())
            DisplayEarnedStarsOnButtons(SaveManager.getChapterNumber());
    }


    public void DisableLockedLevels() {
        for (int i = 1; i < buttonList.Count; i++)
        {
            bool isLevelOpen = SaveManager.isLevelUnlocked(SaveManager.getChapterNumber(), i);     //True or false if level is unlocked
            buttonList[i].interactable = isLevelOpen;   //the next button sets to true or false if it's unlocked or not
        }
 
    }

    public void DisplayEarnedStarsOnButtons(int chapter)
    {
        int[][] playerProgress = SaveManager.LoadSaveProgress();


        for (int i = 0; i < playerProgress[chapter - 1].Length; i++)
        {
            for (int starPlacement = 0; starPlacement < playerProgress[chapter - 1][i]; starPlacement++)
            {
                buttonList[i].transform.GetChild(starPlacement).gameObject.SetActive(true);
            }
        }
    }

    public void loadLevel(int levelToLoad){
        SceneManager.LoadScene(levelToLoad);
    }

    public void ReplayLevel()
    {
        SceneManager.LoadScene(getLevelNameAsInt());
    }

    public void PlayNextLevel()
    {
        //If the player is on level 9 or 18 etc.. next level button will take the player to 
        //the overwolrd (to select a new chapter)
        if (getLevelNameAsInt() % 9 == 0)
            SceneManager.LoadScene("Overworld Level");
        else
            SceneManager.LoadScene(getLevelNameAsInt() + 1);
    }

    public int getLevelNameAsInt(){
        string level = SceneManager.GetActiveScene().name;  //The level name is only numbers
        int levelNumber = System.Convert.ToInt32(level);
        return levelNumber;
    }

    
}
