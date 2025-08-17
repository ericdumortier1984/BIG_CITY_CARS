using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
	[SerializeField] private GameObject collectableItem;
	//[SerializeField] private GameObject collectableItem2;
	[SerializeField] private List<GameObject> collectableItem2;

	private bool IsMissionActive = false;

	public bool CanStartMission()
	{
		return !IsMissionActive;
	}

	public void StartMissionCabagge(MissionData missionData)
	{
		if (IsMissionActive)
		{
			return;
		}
		collectableItem.SetActive(true);
		IsMissionActive = true;
	}

	public void StartMissionHarvest(MissionData missionData)
	{
		if (IsMissionActive)
		{
			return;
		}
		//collectableItem2.SetActive(true);

		foreach (GameObject item in collectableItem2)
		{
			item.SetActive(true);
		}

		IsMissionActive = true;
	}

	public void EndMission()
	{
		IsMissionActive = false;
	}
}