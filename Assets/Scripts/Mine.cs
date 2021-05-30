using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour, IExplosive
{

    private GameController gameController;
    private Animator animator;
    private SoundController soundController;
    private Cow cow;
    private CameraShake cameraShake;

    private string explosion = "explosion";

    private bool isExploding;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        animator = GetComponentInChildren<Animator>();
        soundController = FindObjectOfType<SoundController>();
        cow = FindObjectOfType<Cow>();
        cameraShake = FindObjectOfType<CameraShake>();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isExploding)
        {
            isExploding = true;
            cameraShake.StartShake();
            cow.Explosion(transform.position);
            soundController.PlaySound("ExplosionFx");
            AnimateExplosion();
            if (!soundController.GetSound("HurtCow").audioSource.isPlaying)
            {
                soundController.PlaySound("HurtCow");
            }
            else
            {
                soundController.StopSound("HurtCow");
                soundController.PlaySound("HurtCow");
            }
        }
    }

    //private void HurtCow()
    //{
    //    Debug.Log("OUCHIEEE -> Mine");

    //    cow.Explosion(transform.position);
    //    soundController.PlaySound("HurtCow");
    //    gameController.CowTakesDamage();
    //    gameController.MineExploded();
    //}

    public void Exploded()
    {
        gameController.MineExploded();
    }

    public void PickMeUp()
    {
        //Kan implementeras senare som ny funktion
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void AnimateExplosion()
    {
        animator.SetBool(explosion, true);
    }
}
