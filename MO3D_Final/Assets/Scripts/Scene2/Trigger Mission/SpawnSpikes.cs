using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpikes : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private GameObject spikePrefab;
	[SerializeField] private ParticleSystem dissapearSpikeBoxParticle;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Police"))
		{
			spikePrefab.SetActive(true);
			dissapearSpikeBoxParticle.Play();
			gameObject.SetActive(false);
		}
	}
}
