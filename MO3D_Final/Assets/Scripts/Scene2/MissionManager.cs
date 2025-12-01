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
	[Header("Tunning Car Mission")]
	[SerializeField] private AlmostFerrari almostFerrari;
	[SerializeField] private List<GameObject> otherRacers;
	[Header("Stop Robber Bank Mission")]
	[SerializeField] private StopRobberBank stopRobberBank;
	[SerializeField] private VehicleIntro missionIntro;
	[SerializeField] private GameObject theftFurgon;
	[Header("More Donuts Mission")]
	[SerializeField] private MoreDonuts moreDonuts;
	[SerializeField] private VehicleIntro missionIntroMoreDonuts;
	[SerializeField] private List<GameObject> donuts;
	[Header("Too Hot! Mission")]
	[SerializeField] private TooHot tooHot;
	[SerializeField] private List<GameObject> fires;
	[Header("Little and Furious Mission")]
	[SerializeField] private VehicleIntro missionIntroLittleAndFurious;
	[SerializeField] private LittleAndFurious littleAndFurious;
	[SerializeField] private List<GameObject> growthPill;
	[Header("Home Shopping Mission")]
	[SerializeField] private HomeShopping homeShopping;
	[SerializeField] private List<GameObject> provisions;
	[Header("ForkLift Mission")]
	[SerializeField] private ForkLiftMission forkLift;
	[SerializeField] private VehicleIntro forkliftIntro;
	[SerializeField] private List<GameObject> pallets;
	[SerializeField] private List<GameObject> boxes;
	[SerializeField] private List<GameObject> trucks;
	[SerializeField] private List<GameObject> loadPoints;
	[Header("Max Speed Please Mission")]
	[SerializeField] private MaxSpeedPlease maxSpeedPlease;
	[SerializeField] private List<GameObject> checkpoints;
	[SerializeField] private List<GameObject> muscleCars;
	[Header("Relax Trip Mission")]
	[SerializeField] private RelaxTrip relaxTrip;
	[SerializeField] private VehicleIntro missionIntroRelaxTrip;
	[SerializeField] private List<GameObject> cigarettes;
	[Header("Off Road Adventure Mission")]
	[SerializeField] private OffRoadAventure offRoadAdventure;
	[SerializeField] private VehicleIntro missionIntroOffRoadAdventure;
	[SerializeField] private List<GameObject> flags;
	[Header("Get The Autoparts Mission")]
	[SerializeField] private GetTheAutoparts getAutoparts;
	[SerializeField] private VehicleIntro missionIntroGetAutoparts;
	[SerializeField] private List<GameObject> autoparts;

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

	public void StartMissionTunningCar()
	{
		if (isMissionActive) { return; }
		foreach (GameObject others in otherRacers)
		{
			others.SetActive(true);
		}
		almostFerrari.BeginAlmostFerrari();
		SetWorldState(false);
		isMissionActive = true;
	}

	public void StartMissionRobberBank()
	{
		if (isMissionActive) { return; }
		theftFurgon.SetActive(true);
		missionIntro.PlayIntro();
		stopRobberBank.BeginStopRobberBank();
		SetWorldState(false);
		isMissionActive = true;
	}

	public void StartMissionMoreDonuts()
	{
		if (isMissionActive) { return; }
		foreach (GameObject donut in donuts)
		{
			donut.SetActive(true);
		}
		missionIntroMoreDonuts.PlayIntro();
		moreDonuts.BeginMoreDonuts();
		SetWorldState(false);
		isMissionActive = true;
	}

	public void StartMissionTooHot()
	{
		if (isMissionActive) { return; }
		foreach (GameObject fire in fires)
		{
			fire.SetActive(true);
		}
		tooHot.BeginTooHot();
		SetWorldState(false);
		isMissionActive = true;
	}

	public void StartMissionLittleAndFurious()
	{
		if (isMissionActive) { return; }
		foreach (GameObject pill in growthPill)
		{
			pill.SetActive(true);
		}
		missionIntroLittleAndFurious.PlayIntro();
		littleAndFurious.BeginLittlendFurious();
		SetWorldState(false);
		isMissionActive = true;
	}

	public void StartMissionHomeShopping()
	{
		if (isMissionActive) { return; }
		foreach (GameObject prvs in provisions)
		{
			prvs.SetActive(true);
		}
		homeShopping.BeginHomeShopping();
		SetWorldState(false);
		isMissionActive = true;
	}

	public void StartMissionForkLift()
	{
		if (isMissionActive) { return; }
		foreach (GameObject pallet in pallets)
		{
			pallet.SetActive(true);
		}
		foreach (GameObject box in boxes)
		{
			box.SetActive(true);
		}
		foreach (GameObject truck in trucks)
		{
			truck.SetActive(true);
		}
		foreach (GameObject loadPoint in loadPoints)
		{
			loadPoint.SetActive(true);
		}
		forkliftIntro.PlayIntro();
		forkLift.BeginForkLiftMission();
		SetWorldState(false);
		isMissionActive = true;
	}

	public void StartMissionMaxSpeedPlease()
	{
		if (IsMissionActive) { return; }
		foreach (GameObject checkpoint in checkpoints)
		{
			checkpoint.SetActive(true);
		}
		foreach (GameObject muscleCar in muscleCars)
		{
			muscleCar.SetActive(true);
		}
		maxSpeedPlease.BeginMission();
		SetWorldState(false);
		isMissionActive = true;
	}

	public void StartMissionRelaxTrip()
	{
		if (isMissionActive) { return; }
		foreach (GameObject cigarette in cigarettes)
		{
			cigarette.SetActive(true);
		}
		missionIntroRelaxTrip.PlayIntro();
		relaxTrip.BeginRelaxTrip();
		SetWorldState(false);
		isMissionActive = true;
	}

	public void StartOffRoadAdventure()
	{
		if (isMissionActive) { return; }
		foreach (GameObject flag in flags)
		{
			flag.SetActive(true);
		}
		missionIntroOffRoadAdventure.PlayIntro();
		offRoadAdventure.BeginOffRoadAdventure();
		SetWorldState(false);
		isMissionActive = true;
	}

	public void StartGetTheAutoparts()
	{
		if (isMissionActive) { return; }
		foreach (GameObject autopart in autoparts)
		{
			autopart.SetActive(true);
		}
		missionIntroGetAutoparts.PlayIntro();
		getAutoparts.BeginGetTheAutoparts();
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