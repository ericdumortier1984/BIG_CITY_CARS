using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolSucksTrigger : MonoBehaviour
{
	public MissionManager missionManager;

	[Header("Settings")]
	[SerializeField] private GameObject schoolSucksMission;
	[SerializeField] private CinemachineVirtualCamera missionVcam;
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
		if (other.CompareTag("School Bus"))
		{
			if (!missionManager.CanStartMission()) { return; }
			missionVcam.Priority = 20;
			missionManager.StartSchoolSucks();
			gameObject.SetActive(false);
		}
		else if (other.CompareTag("IACar"))
		{
			UIMissionManager.Instance.ShowMissionText("", 1.0f, 50);
		}
		else
		{
			UIMissionManager.Instance.ShowMissionText("ONLY SCHOOL BUS", 1.0f, 50);
		}
	}
}
