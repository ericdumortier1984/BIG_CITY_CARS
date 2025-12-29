using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffRoadAdventureTrigger : MonoBehaviour
{
	public MissionManager missionManager;

	[Header("Settings")]
	[SerializeField] private GameObject offRoadAventureMission;
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
		if (other.CompareTag("Off Road"))
		{
			if (!missionManager.CanStartMission()) { return; }
			missionVcam.Priority = 21;
			missionManager.StartOffRoadAdventure();
			gameObject.SetActive(false);
			offRoadAventureMission.SetActive(true);
		}
		else if (other.CompareTag("IACar"))
		{
			UIMissionManager.Instance.ShowMissionText("", 1.0f, 50);
		}
		else
		{
			UIMissionManager.Instance.ShowMissionText("ONLY YELLOW JEEP", 1.0F, 50);
		}
	}
}
