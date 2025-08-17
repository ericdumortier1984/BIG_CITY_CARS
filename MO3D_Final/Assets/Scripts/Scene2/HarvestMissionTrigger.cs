using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestMissionTrigger : MonoBehaviour
{
	public MissionData missionData;
	public MissionManager missionManager;

	[SerializeField] private GameObject harvestMapPos;

	private void Start()
	{
		harvestMapPos.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("FarmCar"))
		{
			if (!missionManager.CanStartMission())
			{
				return;
			}
			missionManager.StartMissionHarvest(missionData);
			gameObject.SetActive(false);
			harvestMapPos.SetActive(true);
		}
	}

	public void HideMapIcon()
	{
		harvestMapPos.SetActive(false);
	}
}

