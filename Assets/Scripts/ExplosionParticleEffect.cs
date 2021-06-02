using UnityEngine;

public class ExplosionParticleEffect : MonoBehaviour {

	[SerializeField] private GameObject explosionParticleEffect;

	public void Explode(Vector3 pos) {

		Instantiate(explosionParticleEffect, pos, Quaternion.identity);
	}
}
