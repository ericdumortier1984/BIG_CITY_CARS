using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{
    private PackageDelivery packageDelivery;

	private void Start()
	{
		packageDelivery = FindObjectOfType<PackageDelivery>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Red Furgon"))
		{
			packageDelivery.DeliveryPackage(this);
		}
	}
}
