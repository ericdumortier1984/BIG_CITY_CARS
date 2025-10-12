using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPassengerPoint : MonoBehaviour
{
    private TaxiCab taxiCab;

	private void Start()
	{
		taxiCab = FindObjectOfType<TaxiCab>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Taxi"))
		{
			taxiCab.PickUpPassenger(this);
		}
	}
}
