using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCheckpointMuscleCar : MonoBehaviour
{
	private MaxSpeedPlease maxSpeedPlease;

	private void Start()
	{
		maxSpeedPlease = FindObjectOfType<MaxSpeedPlease>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Muscle Car"))
		{
			if (maxSpeedPlease != null)
			{
				maxSpeedPlease.OnCheckpoint(this);
			}
			gameObject.SetActive(false);
		}
	}
}
