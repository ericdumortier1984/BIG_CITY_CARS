using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCigarette : MonoBehaviour
{
	private RelaxTrip relaxTrip;

	private void Start()
	{
		relaxTrip = FindObjectOfType<RelaxTrip>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Mini Van"))
		{
			if (relaxTrip != null)
			{
				relaxTrip.CollectCigarettes(this);
			}
			gameObject.SetActive(false);
		}
	}
}
