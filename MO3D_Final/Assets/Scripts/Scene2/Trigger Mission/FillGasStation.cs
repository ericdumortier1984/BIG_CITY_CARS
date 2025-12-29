using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FillGasStation : MonoBehaviour
{
	private FuelUp fuelUp;

	private void Start()
	{
		fuelUp = FindObjectOfType<FuelUp>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Tanker Truck"))
		{
			fuelUp.GasStationFuelUp();
		}
	}
}
