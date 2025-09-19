using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
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
			taxiRace.WinMission();
		}
		//else if (other.gameObject == taxiRacer)
		else if (other.gameObject == taxiRace.GetTaxiRacer())
		{
			taxiRace.LoseMission();
		}
	}
}
