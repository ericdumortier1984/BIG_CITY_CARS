using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSupply : MonoBehaviour
{
	[Header("Particle")]
	[SerializeField] private ParticleSystem loadParticle;

	private SupplyDelivery supplyDelivery;

	private void Start()
	{
		supplyDelivery = FindObjectOfType<SupplyDelivery>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Delivery Truck"))
		{
			loadParticle.transform.position = transform.position;
			loadParticle.gameObject.SetActive(true);
			loadParticle.Play();

			supplyDelivery.Load(this);
		}
	}
}
