using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiCheckpointTrigger : MonoBehaviour
{
	private TaxiRace taxiRace;

	private void Start()
	{
		taxiRace = FindAnyObjectByType<TaxiRace>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Taxi"))
		{
			taxiRace.UpdatePlayerCheckpoints(gameObject);
		}
	}
}
