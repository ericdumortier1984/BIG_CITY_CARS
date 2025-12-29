using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSalePointTrigger : MonoBehaviour
{
    private bool isFoodTruckInside = false;

	private void OnTriggerEnter(Collider other)
	{
		FastFood fastFoodMission = FindObjectOfType<FastFood>();

		if (other.CompareTag("Fast Food Truck") && !isFoodTruckInside)
		{
			isFoodTruckInside = true;

			if (fastFoodMission != null)
			{
				fastFoodMission.EnterCookingMode(this);
				gameObject.SetActive(false);
			}
		}
	}
}
