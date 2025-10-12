using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffPassengerPoint : MonoBehaviour
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
			taxiCab.DropOffPassenger(this);
		}
	}
}
