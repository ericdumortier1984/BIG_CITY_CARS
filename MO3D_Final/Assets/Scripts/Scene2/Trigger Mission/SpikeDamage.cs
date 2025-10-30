using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float damageAmount;
	[SerializeField] private GameObject spike;
	[SerializeField] private ParticleSystem damageParticle;

	private void OnTriggerEnter(Collider other)
	{
		DamageManager damageManager = other.GetComponent<DamageManager>();
		if (damageManager != null)
		{
			damageManager.TakeDamage(damageAmount);
			spike.gameObject.GetComponent<Collider>().enabled = false;
			damageParticle.Play();
		}
	}
}
