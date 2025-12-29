using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectFuelPump : MonoBehaviour
{
    [Header("FXs")]
    [SerializeField] private ParticleSystem collectParticle;

    private FuelUp fuelUp;

	private void Start()
	{
		fuelUp = FindObjectOfType<FuelUp>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Tanker Truck"))
		{
			collectParticle.transform.position = transform.position;
			collectParticle.gameObject.SetActive(true);
			collectParticle.Play();

			fuelUp.FillTankFuelTruck(this);
		}
	}
}
