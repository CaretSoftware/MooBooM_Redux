using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip musicClip;

    public AudioSource mooButtonSource;
    public AudioClip mooButtonClip;

    public AudioSource winSoundSource;
    public AudioClip winSoundClip;

    public AudioSource looseSoundSource;
    public AudioClip looseSoundClip;

    public AudioSource looseSoundYehaSource;
    public AudioClip looseSoundYehaClip;

    public static MusicController onlyMusicController;

    private bool hasFarmerShouted = false;


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
        winSoundSource.clip = winSoundClip;
        looseSoundSource.clip = looseSoundClip;
    }

    public void PlayMooButtonSound()
    {
        mooButtonSource.PlayOneShot(mooButtonClip);
    }

    public void PlayWinSound()
    {
        winSoundSource.PlayOneShot(winSoundClip);
    }

    public void PlayLooseSound()
    {
        if (hasFarmerShouted)
        {
            looseSoundYehaSource.PlayOneShot(looseSoundYehaClip);
        }
        else
        {
            looseSoundSource.PlayOneShot(looseSoundClip);
        }
        hasFarmerShouted = false;
    }

    public void FarmerHasShouted(bool readyToShout)
    {
        hasFarmerShouted = readyToShout;
    }
}
