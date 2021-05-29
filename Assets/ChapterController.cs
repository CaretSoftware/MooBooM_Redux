using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterController : MonoBehaviour
{
    public GameObject chapter2;

    void Start()
    {
        //Check if chapter 2 is unlocked. Only have 2 chapters
        if (SaveManager.IsNextChapterUnlocked(1))
        {
            chapter2.SetActive(false);
        }
    }

}
