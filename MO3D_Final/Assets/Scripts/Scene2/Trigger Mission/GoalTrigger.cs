using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private GameObject triggerMission;
	private TaxiRace taxiRace;

	private void Start()
	{
		taxiRace = FindAnyObjectByType<TaxiRace>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Taxi") && taxiRace.IsCheckpointCompleted())
		{
			taxiRace.WinMission();
		}
		else if (other.CompareTag("Taxi") && !taxiRace.IsCheckpointCompleted())
		{
			UIMissionManager.Instance.ShowMissionText("YOU MISSED A CHECKPOINT", 2.0f, 50);
		}
		else if (other.gameObject == taxiRace.GetTaxiRacer())
		{
			taxiRace.LoseMission();
		}
	}
}
