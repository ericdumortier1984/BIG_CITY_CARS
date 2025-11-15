using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireZoneTrigger : MonoBehaviour
{
	private bool isFireTruckInside = false;


	private void OnTriggerEnter(Collider other)
	{
		TooHot mission = FindObjectOfType<TooHot>();

		if (other.CompareTag("Fire Truck") && !isFireTruckInside)
		{
			isFireTruckInside = true;
			
			if (mission != null)
			{
				mission.EnterWaterMode(this);
			}
		}
	}
}
