using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunningCarCheckpoint : MonoBehaviour
{
	[SerializeField] private AlmostFerrari almostFerrari;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Tunning Car"))
		{
			almostFerrari.UpdatePlayerCheckpoints(gameObject);

			if (almostFerrari.IsCheckpointCompleted())
			{
				almostFerrari.WinMission();
			}
		}
	}
}

