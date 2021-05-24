using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

public class SoundController : MonoBehaviour
{

    [SerializeField] private Sound[] soundClips;
    [SerializeField] private Sound[] engFarmerSoundClips;
    [SerializeField] private Sound[] sweFarmerSoundClips;
    private Sound[] languageClips;


    public static SoundController onlySoundController;
    private GameController gameController;
    private MusicController musicController;

    private Sound farmerSound;

    private bool isInitialized;
    private bool hasCalledMusicController = false;
    [SerializeField] private bool isNotLevel;
    private float time = 3f;


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

        if(LocalisationSystem.language == LocalisationSystem.Language.English)
        {
            languageClips = engFarmerSoundClips;
        }
        else
        {
            languageClips = sweFarmerSoundClips;
        }

        for(int i = 0; i < languageClips.Length; i++)
        {
            languageClips[i].audioSource = gameObject.AddComponent<AudioSource>();

            languageClips[i].audioSource.clip = languageClips[i].audioClip;
            languageClips[i].audioSource.pitch = languageClips[i].pitch;
            languageClips[i].audioSource.volume = languageClips[i].volume;
            languageClips[i].audioSource.outputAudioMixerGroup = languageClips[i].audioMixerGroup;
            languageClips[i].audioSource.panStereo = languageClips[i].stereoPan;
        }
    }

    private void Start()
    {
        musicController = FindObjectOfType<MusicController>();
        gameController = FindObjectOfType<GameController>();
        if (!isNotLevel)
        {
            PlaySoundWithDelay("BombThrow", 1.5f);
        }
        if(musicController != null)
        {
            musicController.FarmerHasShouted(false);
        }
    }

    private void Update()
    {
        if(!isNotLevel && !isInitialized && gameController.isGameReady())
        {
            farmerSound = languageClips[UnityEngine.Random.Range(0, languageClips.Length)];
            farmerSound.audioSource.PlayDelayed(1f);
            isInitialized = true;
        }
        else if (isInitialized && musicController != null)
        {
            if(time > 0)
            {
                //print(time);
                time -= Time.deltaTime;
            }
            else if (!hasCalledMusicController)
            {
                musicController.FarmerHasShouted(true);
                hasCalledMusicController = true;
            }
      
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
        foreach(Sound sound in languageClips)
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
        foreach(Sound sound in languageClips)
        {
            sound.audioSource.pitch = sound.pitch;
        }
    }

}
