using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterController : MonoBehaviour
{
    public GameObject chapter2;

    void Start()
    {
        //Check if chapter 2 is unlocked
        if (SaveManager.IsNextChapterUnlocked(SaveManager.getChapterNumber()))
        {
            chapter2.SetActive(false);
        }
    }

}
