using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFurgonTrigger : MonoBehaviour
{
    public MissionManager packageDeliveryManager;
	
	[Header("Settings")]
	[SerializeField] private Cinemachine.CinemachineVirtualCamera missionVCam;
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
		if (other.CompareTag("Red Furgon"))
		{
			if (!packageDeliveryManager.CanStartMission()) { return; }
			missionVCam.Priority = 20;
			packageDeliveryManager.StartMissionRedFurgon();
			gameObject.SetActive(false);
			routeDrawer.gameObject.SetActive(true);
		}
		else if (other.CompareTag("IACar"))
		{
			UIMissionManager.Instance.ShowMissionText("", 1.0f);
		}
		else
		{
			UIMissionManager.Instance.ShowMissionText("ONLY RED FURGON", 1.0f);
		}
	}
}
