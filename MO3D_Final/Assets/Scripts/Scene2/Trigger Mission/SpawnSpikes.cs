using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpikes : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private GameObject spikePrefab;
	[SerializeField] private ParticleSystem dissapearSpikeBoxParticle;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip spawnSFX;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Police"))
		{
			AudioManager.Instance.PlaySFX(spawnSFX);
			spikePrefab.SetActive(true);
			dissapearSpikeBoxParticle.Play();
			gameObject.SetActive(false);
		}
	}
}
