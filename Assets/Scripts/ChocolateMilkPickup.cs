using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocolateMilkPickup : MonoBehaviour
{

    [SerializeField] Animator animator;
    [SerializeField] MusicController musicController;
    [SerializeField] SoundController soundController;
   
    private float timer = 0f;
    private float timeBeforeReset = 3f;

    private bool isSlowMotion;

    private string pickup = "Pickup";

    private void Start()
    {
        musicController = FindObjectOfType<MusicController>();
        soundController = FindObjectOfType<SoundController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= timeBeforeReset)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            if(musicController != null)
            {
                musicController.musicSource.pitch = 1f;
            }
            soundController.ResetPitchForAll();
            isSlowMotion = false;
        }
        if (isSlowMotion) { 
            timer += Time.deltaTime; 
        }
        
        
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { 
            if(musicController != null)
            {
                musicController.musicSource.pitch = 0.5f;
            }
            soundController.ChangePitchForAll(0.5f);
            isSlowMotion = true;
            Time.timeScale = 0.2f;
            animator.SetBool(pickup, true);
        }
    }
}
