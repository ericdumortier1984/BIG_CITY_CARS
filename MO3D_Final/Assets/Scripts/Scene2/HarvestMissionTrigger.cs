using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestMissionTrigger : MonoBehaviour
{
	public MissionData missionData;
	public MissionManager missionManager;

	[Header("Settings")]
	[SerializeField] private Cinemachine.CinemachineVirtualCamera missionVCam;
	[SerializeField] private GameObject harvestMapPos;
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
		if (other.CompareTag("FarmCar"))
		{
			if (!missionManager.CanStartMission())
			{
				return;
			}
			missionVCam.Priority = 20;
			missionManager.StartMissionHarvest(missionData);
			gameObject.SetActive(false);
			harvestMapPos.SetActive(true);
		}
		else
		{
			UIMissionManager.Instance.ShowMissionText("ONLY FARM VEHICLES", 1.0f);
		}
	}
}

