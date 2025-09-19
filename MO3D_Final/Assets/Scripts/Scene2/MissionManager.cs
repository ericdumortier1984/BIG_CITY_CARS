using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
	[Header("Cabbage Mission")]
	[SerializeField] private GameObject collectableItem;
	[Header("Harvest Mission")]
	[SerializeField] private List<GameObject> collectableItem2;
	[Header("Taxi mission")]
	[SerializeField] private GameObject taxiRacer;

	private bool isMissionActive = false;
	public bool IsMissionActive => isMissionActive;

	public bool CanStartMission()
	{
		return !isMissionActive;
	}

	public void StartMissionCabagge(MissionData missionData)
	{
		if (isMissionActive) { return; }
		collectableItem.SetActive(true);
		isMissionActive = true;
	}

	public void StartMissionHarvest(MissionData missionData)
	{
		if (isMissionActive) { return;}

		foreach (GameObject item in collectableItem2)
		{
			item.SetActive(true);
		}

		isMissionActive = true;
	}

	public void StartMisionTaxiRace(MissionData missionData)
	{
		if (isMissionActive) { return; }
		taxiRacer.SetActive(true);
		isMissionActive = true;
	}

	public void EndMission()
	{
		isMissionActive = false;
	}
}