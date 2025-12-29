using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMaterialTrigger : MonoBehaviour
{
	[Header("FXs")]
	[SerializeField] ParticleSystem spawnParticle;

	private HeavyLoad heavyLoad;

	private void Start()
	{
		heavyLoad = FindObjectOfType<HeavyLoad>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Red Truck"))
		{
			spawnParticle.transform.position = transform.position;
			spawnParticle.gameObject.SetActive(true);
			spawnParticle.Play();

			heavyLoad.SpawnMaterial();
		}
	}
}
