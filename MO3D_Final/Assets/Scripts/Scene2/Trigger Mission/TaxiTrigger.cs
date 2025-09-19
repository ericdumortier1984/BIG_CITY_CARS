using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiTrigger : MonoBehaviour
{
	public MissionData missionData;
	public MissionManager missionManager;

	[Header("Settings")]
	[SerializeField] private Cinemachine.CinemachineVirtualCamera missionVCam;
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
		if (other.CompareTag("Taxi"))
		{
			if (!missionManager.CanStartMission()) { return; }
			missionVCam.Priority = 20;
			missionManager.StartMisionTaxiRace(missionData);
			gameObject.SetActive(false);
		}
		else if (other.CompareTag("IACar"))
		{
			UIMissionManager.Instance.ShowMissionText("", 1.0f);
		}
		else
		{
			UIMissionManager.Instance.ShowMissionText("ONLY TAXI VEHICLES", 1.0f);
		}
	}
}
