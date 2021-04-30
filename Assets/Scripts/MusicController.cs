using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip musicClip;

    private bool musicIsPlaying;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!musicIsPlaying)
        {
            audioSource.clip = musicClip;
            audioSource.loop = true;
            audioSource.Play();
            musicIsPlaying = true;
        }
    }


}
