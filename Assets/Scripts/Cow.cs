using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : MonoBehaviour {

	[SerializeField] private Rigidbody rb;
	private SoundController soundController;
	Sound rollSound;

	float nominalSpeed = 1f;
	float speed;

	private void Start() {
		Freeze();
		soundController = FindObjectOfType<SoundController>();
		rollSound = soundController.GetSound("CowRoll");
	}

    private void FixedUpdate()
    {
		speed = rb.velocity.magnitude;
		/*if (speed >= 0.1f && !rollSound.audioSource.isPlaying)
        {
			PlayRollSound();
        }
		else if(speed < 0.1f && rollSound.audioSource.isPlaying)
        {
			rollSound.audioSource.Pause();
        }*/
	}

    public void Release() {
		rb.isKinematic = false;
	}

	public void Freeze() {
		rb.isKinematic = true;
	}

	public void Explosion(Vector3 otherPos) {
		Debug.Log("Boom");
		rb.AddExplosionForce(500f, otherPos, 20f, 1f);
	}

	public void UseGravity(bool gravity) {
		rb.useGravity = gravity;
	}


    private void OnCollisionStay(Collision collision)
    {

		if(Time.timeScale == 0.2f)
        {
			rollSound.audioSource.pitch = (speed / nominalSpeed) / 5;

        }
        else if(speed >= 3f)
        {
			rollSound.audioSource.pitch = speed / nominalSpeed;
        }else if(speed < 3f)
        {
			rollSound.audioSource.pitch = 2.7f;

		}
		if (rollSound.audioSource.isPlaying == false && speed >= 0.7f && collision.gameObject.tag == "Ground")
		{
			soundController.PlaySound("CowRoll");
		}
		else if (rollSound.audioSource.isPlaying == true && speed < 0.7f && collision.gameObject.tag == "Ground")
		{
			soundController.StopSound("CowRoll");
		}
	}

    private void OnCollisionExit(Collision collision)
    {
		if (rollSound.audioSource.isPlaying == true && collision.gameObject.tag == "Ground")
		{
			soundController.PauseSound("CowRoll");
		}
	}

    /*private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Fence")
        {
			soundController.PlaySound("HittingFence");
        }
    }
	*/

}
