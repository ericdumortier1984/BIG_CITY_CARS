using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloadSupply : MonoBehaviour
{
	[Header("Particle")]
	[SerializeField] private ParticleSystem unloadParticle;

    private SupplyDelivery supplyDelivery;

	private void Start()
	{
		supplyDelivery = FindObjectOfType<SupplyDelivery>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Delivery Truck"))
		{
			unloadParticle.transform.position = transform.position;
			unloadParticle.gameObject.SetActive(true);
			unloadParticle.Play();

			supplyDelivery.Unload(this);
		}
	}
}
