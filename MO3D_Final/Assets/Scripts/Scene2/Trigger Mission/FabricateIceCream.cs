using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class FabricateIceCream : MonoBehaviour
{
    private IceIceCreamBaby iceIceCreamBaby;

	private bool IceCreamTruckInside = false;

	private void Start()
	{
		iceIceCreamBaby = FindObjectOfType<IceIceCreamBaby>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ice Cream Truck"))
		{
			IceCreamTruckInside = true;
			iceIceCreamBaby.BeginFabricateIceCream();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Ice Cream Truck"))
		{
			IceCreamTruckInside = false;
			iceIceCreamBaby.StopFabricateIceCream();
		}
	}
}
