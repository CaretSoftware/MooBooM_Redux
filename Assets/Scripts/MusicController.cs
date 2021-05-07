using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip musicClip;

    public AudioSource mooButtonSource;
    public AudioClip mooButtonClip;

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
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
        mooButtonSource.clip = mooButtonClip;
    }

}
