using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip musicClip;

    public static MusicController onlyMusicController;



    private void Awake()
    {
        if (onlyMusicController == null)
        {
            onlyMusicController = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
            audioSource.clip = musicClip;
            audioSource.loop = true;
            audioSource.Play();
    }


}
