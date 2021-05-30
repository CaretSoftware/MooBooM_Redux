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

    public void Exploded()
    {
        gameController.MineExploded();
    }

    public void PickMeUp()
    {
        //Inherited from IExplosive
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
