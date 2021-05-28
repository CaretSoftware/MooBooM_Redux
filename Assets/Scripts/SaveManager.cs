using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager{

    //No need to create and initialize this 3 times so it's a class variable
    private static string path = Application.persistentDataPath + "/playerProgress.save";
    private static BinaryFormatter formatter = new BinaryFormatter();


    private static int[][] levelStarsofChapters;     //[chapter][levels]
    private static int[] levelStarsChap1 = new int[9];
    private static int[] levelStarsChap2 = new int[9];
    private static int[] levelStarsChap3 = new int[9];
    private static int[] levelStarsChap4 = new int[9];
    private static int[] levelStarsChap5 = new int[9];
    private static int[] levelStarsChap6 = new int[9];

    private static int chapter = 1;
    public static void Initialize() {
        if (!File.Exists(path))
        {
            createANewSaveProgress();
            CreateNewSave();
        }
    }

    private static void CreateNewSave() {
        FileStream fileStream = new FileStream(path, FileMode.Create);
        for (int chapter = 0; chapter < levelStarsofChapters.Length; chapter++)
        {
            for (int level = 0; level < levelStarsofChapters[chapter].Length; level++)
            {
                levelStarsofChapters[chapter][level] = 0;
            }
        }
        formatter.Serialize(fileStream, levelStarsofChapters);
        fileStream.Close();
    }
    public static void SaveLevelStars(GameController gameController, LevelSelect levelSelect) {

        FileStream fileStream = new FileStream(path, FileMode.Create); //A stream of data contained in a file

        if(fileStream.Length > 0)
        {
            levelStarsofChapters = formatter.Deserialize(fileStream) as int[][];
        }
        
        int levelNumber = levelSelect.getLevelNameAsInt();
        int starstoAdd = gameController.GetStarsCount();
        //chapter = getChapterNumber();

        //If the level has been played before and the existing starCount is lower than the new one  -> replace
        //This adds them in order
        if (levelStarsofChapters[chapter - 1][levelNumber - 1] < starstoAdd)
        {
            levelStarsofChapters[chapter - 1][levelNumber - 1] = starstoAdd;
        }

        
        string output = "";
        foreach (int x in levelStarsofChapters[chapter - 1])
        {
            output = output + (" " + x);
            
        }
        Debug.Log("Earened stars on chapter " + chapter + ": " + output);

        formatter.Serialize(fileStream, levelStarsofChapters);  //Write data to the file, binary
        fileStream.Close();  
    }

    private static void createANewSaveProgress() {
        levelStarsofChapters = new int[6][];
        levelStarsofChapters[0] = levelStarsChap1;
        levelStarsofChapters[1] = levelStarsChap2;
        levelStarsofChapters[2] = levelStarsChap3;
        levelStarsofChapters[3] = levelStarsChap4;
        levelStarsofChapters[4] = levelStarsChap5;
        levelStarsofChapters[5] = levelStarsChap6;
    }

    public static void SetChapterNumber(int chapterNumber) {
        chapter = chapterNumber;
    }

    public static int getChapterNumber() {
        return chapter;
    }

    public static bool isLevelUnlocked(int chapter, int currentLevel)
    {
        //Loads the players progress if it hasn't already been loaded
        if (levelStarsofChapters == null)
        {
            levelStarsofChapters = LoadSaveProgress();
        }

        //If the specified level have 1 or more stars, you can play the next level
        if (levelStarsofChapters != null)
        {
            if (levelStarsofChapters[chapter - 1][currentLevel - 1] > 0)
            {
                return true;
            }
        }
        
        return false;
    }

    public static int[][] LoadSaveProgress() {
        FileStream fileStream = new FileStream(path, FileMode.Open);  //Open the existing data
        if (File.Exists(path) && fileStream.Length > 0)
        {
            levelStarsofChapters = formatter.Deserialize(fileStream) as int[][]; //Reads the file ( -> from binary to original) casts it to int[][]
            fileStream.Close();
            return levelStarsofChapters;
        }
        else
        {
            Debug.LogError("Could not find saved data from " + path);   //error message

            return null;
        }
    }

    public static bool CheckIfFileExists() {
        if (File.Exists(path))
        {
            return true;
        }
        return false;
    }

    public static bool IsNextChapterUnlocked(int previousChapter) {

        int nextChapter = previousChapter + 1;
        int starsToUnlockNewChapter = nextChapter * 5;
        //Loads the players progress if it hasn't already been loaded
        if (levelStarsofChapters == null)
        {
            levelStarsofChapters = LoadSaveProgress();
        }

        int earnedStars = 0;
        for (int i = 0; i < levelStarsofChapters[previousChapter - 1].Length; i++)
        {
            earnedStars = earnedStars + levelStarsofChapters[previousChapter - 1][i];
        }

        if (earnedStars >= starsToUnlockNewChapter)
            return true;
        else
            return false;
    }
}