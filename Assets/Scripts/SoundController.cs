using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

public class SoundController : MonoBehaviour
{

    [SerializeField] private Sound[] soundClips;


    //[SerializeField] private List<Sound> soundClips;

    public static SoundController onlySoundController;
    private GameController gameController;
    private MusicController musicController;

    private bool isInitialized;
    [SerializeField] private bool isNotLevel;


    private void Awake()
    {
        if (onlySoundController == null)
        {
            onlySoundController = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        for(int i = 0; i < soundClips.Length; i++)
        {
            soundClips[i].audioSource = gameObject.AddComponent<AudioSource>();
            
            soundClips[i].audioSource.clip = soundClips[i].audioClip;
            soundClips[i].audioSource.loop = soundClips[i].loop;
            soundClips[i].audioSource.pitch = soundClips[i].pitch;
            soundClips[i].audioSource.volume = soundClips[i].volume;
            soundClips[i].audioSource.outputAudioMixerGroup = soundClips[i].audioMixerGroup;
        }
    }

    private void Start()
    {
        musicController = FindObjectOfType<MusicController>();
        gameController = FindObjectOfType<GameController>();
        if (!isNotLevel)
        {
            PlaySoundWithDelay("BombThrow", 2f);
        }
    }

    private void Update()
    {
        if(!isNotLevel && gameController.isGameReady() && !isInitialized)
        {
            if (LocalisationSystem.language == LocalisationSystem.Language.Swedish)
            {
                PlaySoundWithDelay("SwedishFarmer", 1f);
            }
            else if (LocalisationSystem.language == LocalisationSystem.Language.English)
            {
                PlaySoundWithDelay("EnglishFarmer", 1f);
            }
            isInitialized = true;
        }
    }

    private Sound SearchForSound(string nameOfSound)
    {
        foreach (Sound sound in soundClips)
        {
            if (sound.name == (nameOfSound))
            {
                return sound;
            }
        }
        return null;
    }

    public void StopSound(string nameOfSound)
    {
        SearchForSound(nameOfSound).audioSource.Stop();
    }

    public void PauseSound(string nameOfSound)
    {
        SearchForSound(nameOfSound).audioSource.Pause();
    }

    public void StopSoundLoop(string nameOfSound)
    {
        SearchForSound(nameOfSound).audioSource.loop = false;
    }

    public void PlaySound(string nameOfSound)
    {
        SearchForSound(nameOfSound).audioSource.PlayOneShot(SearchForSound(nameOfSound).audioClip);  
    }

    public void PlaySoundWithDelay(string nameOfSound, float delay)
    {
        SearchForSound(nameOfSound).audioSource.PlayDelayed(delay);
    }

    public void PlayLevelLoadSound()
    {
        if(musicController != null) { musicController.PlayMooButtonSound(); }
    }

    public Sound GetSound(string nameOfSound)
    {
        return SearchForSound(nameOfSound);
    }

    public void ChangePitchForAll(float pitchToSet)
    {
        foreach(Sound sound in soundClips)
        {
            sound.audioSource.pitch = pitchToSet;
        }
    }

    public void ResetPitchForAll()
    {
        foreach(Sound sound in soundClips)
        {
            sound.audioSource.pitch = sound.pitch;
        }
    }
}
