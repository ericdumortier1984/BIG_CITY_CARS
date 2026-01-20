using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaxiCabTrigger : MonoBehaviour
{
	public MissionManager missionManager;
	
	[Header("Settings")]
	[SerializeField] private TMP_FontAsset font;
	[SerializeField] private CinemachineVirtualCamera missionVCam;
	[SerializeField] private int missionIndex;
	[SerializeField] private RouteDrawer routeDrawer;

	private void Start()
	{
		SaveData saveData = SaveSystem.LoadGame();
		if (saveData.missionCompleted[missionIndex])
		{
			gameObject.SetActive(false);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Taxi"))
		{
			if (!missionManager.CanStartMission()) { return; }
			missionVCam.Priority = 20;
			missionManager.StartMissionTaxiCab();
			gameObject.SetActive(false);
			routeDrawer.gameObject.SetActive(true);
		}
		else if (other.CompareTag("IACar"))
		{
			UIMissionManager.Instance.ShowMissionText("", 1.0f, 50, font);
		}
		else
		{
			UIMissionManager.Instance.ShowMissionText("ONLY TAXI", 1.0f, 50, font);
		}
	}
}
