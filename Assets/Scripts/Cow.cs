using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : MonoBehaviour
{

	[SerializeField] private Rigidbody rb;
	private SoundController soundController;
	Sound rollSound;
	Sound hittingFence;
	Sound hittingFenceMoo;

	bool mooHit = false;

	float nominalSpeed = 1f;
	float speed;

	private float minCollisionVolume = 0.3f;
	private float impactToVolumeRatio = 0.2f;

	private void Start()
	{
		Freeze();
		soundController = FindObjectOfType<SoundController>();
		rollSound = soundController.GetSound("CowRoll");
		hittingFence = soundController.GetSound("HittingFence");
		hittingFenceMoo = soundController.GetSound("HittingFenceMoo");
	}

	private void FixedUpdate()
	{
		speed = rb.velocity.magnitude;
	}

	public void Release()
	{
		rb.isKinematic = false;
	}

	public void Freeze()
	{
		rb.isKinematic = true;
	}

	public void Explosion(Vector3 otherPos)
	{
		rb.AddExplosionForce(500f, otherPos, 20f, 1f);
	}

	public void UseGravity(bool gravity)
	{
		rb.useGravity = gravity;
	}


	private void OnCollisionStay(Collision collision)
	{

		if (Time.timeScale == 0.2f)
		{
			rollSound.audioSource.pitch = (speed / nominalSpeed) / 5;

		}
		else if (speed >= 3f)
		{
			rollSound.audioSource.pitch = speed / nominalSpeed;
		}
		else if (speed < 3f)
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

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Fence")
		{
			if (collision.relativeVelocity.magnitude > 4f)
			{
				float volume = collision.relativeVelocity.magnitude * impactToVolumeRatio;
				volume = Mathf.Clamp(volume, minCollisionVolume, 3f);
				if (mooHit && !soundController.GetSound("HurtCow").audioSource.isPlaying)
				{
					hittingFenceMoo.audioSource.PlayOneShot(hittingFenceMoo.audioClip, volume);
					mooHit = false;
				}
				else
				{
					hittingFence.audioSource.PlayOneShot(hittingFence.audioClip, volume);
					mooHit = true;
				}
			}
		}


	}
}
