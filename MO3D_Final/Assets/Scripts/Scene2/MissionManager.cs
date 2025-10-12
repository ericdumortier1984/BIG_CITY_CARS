using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
	[Header("Global References")]
	[SerializeField] private List<GameObject> coins;
	[Header("Cabbage Mission")]
	[SerializeField] private GameObject collectableItem;
	[Header("Harvest Mission")]
	[SerializeField] private List<GameObject> collectableItem2;
	[Header("Taxi mission")]
	[SerializeField] private GameObject taxiRacer;
	[Header("Taxi Cab Mission")]
	[SerializeField] private TaxiCab taxiCab;
	[SerializeField] private List<GameObject> passenger;
	[Header("Red Furgon Mission")]
	[SerializeField] private List<GameObject> redFurgonItems;

	private bool isMissionActive = false;
	public bool IsMissionActive => isMissionActive;

	public bool CanStartMission()
	{
		return !isMissionActive;
	}
	
	public void StartMissionCabagge()
	{
		if (isMissionActive) { return; }
		collectableItem.SetActive(true);
		SetWorldState(false);
		isMissionActive = true;
	}

	public void StartMissionHarvest()
	{
		if (isMissionActive) { return;}

		foreach (GameObject item in collectableItem2)
		{
			item.SetActive(true);
		}
		SetWorldState(false);
		isMissionActive = true;
	}

	public void StartMissionTaxiRace()
	{
		if (isMissionActive) { return; }
		taxiRacer.SetActive(true);
		SetWorldState(false);
		isMissionActive = true;
	}

	public void StartMissionTaxiCab()
	{
		if (isMissionActive) { return; }
		taxiCab.gameObject.SetActive(true);
		taxiCab.BeginTaxiCabMission();
		SetWorldState(false);
		isMissionActive = true;
	}

	public void StartMissionRedFurgon()
	{
		if (isMissionActive) { return; }
		foreach (GameObject item in redFurgonItems)
		{
			item.SetActive(true);
		}
		SetWorldState(false);
		isMissionActive = true;
	}

	public void EndMission()
	{
		isMissionActive = false;
		SetWorldState(true); 
	}

	private void SetWorldState(bool active)
	{
		foreach (GameObject coin in coins)
		{
			if (coin != null)
				coin.SetActive(active);
		}
	}
}