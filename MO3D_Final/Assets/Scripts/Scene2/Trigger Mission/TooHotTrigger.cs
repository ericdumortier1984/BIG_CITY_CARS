using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooHotTrigger : MonoBehaviour
{
    public MissionManager missionManager;

	[Header("Settings")]
	[SerializeField] private GameObject tooHot;
	[SerializeField] private RouteDrawer routeDrawer;
	[SerializeField] private Cinemachine.CinemachineVirtualCamera missionVcam;
	[SerializeField] private int missionIndex;

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
		if (other.CompareTag("Fire Truck"))
		{
			if (!missionManager.CanStartMission()) { return; }
			missionVcam.Priority = 20;
			missionManager.StartMissionTooHot();
			gameObject.SetActive(false);
			tooHot.SetActive(true);
			routeDrawer.gameObject.SetActive(true);
		}
		else if (other.CompareTag("IACar"))
		{
			UIMissionManager.Instance.ShowMissionText("", 1.0f, 50);
		}
		else
		{
			UIMissionManager.Instance.ShowMissionText("ONLY FIRE TRUCK", 1.0F, 50);
		}
	}
}
