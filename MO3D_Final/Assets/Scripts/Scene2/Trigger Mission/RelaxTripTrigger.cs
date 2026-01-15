using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelaxTripTrigger : MonoBehaviour
{
	public MissionManager missionManager;

	[Header("Settings")]
	[SerializeField] private GameObject relaxTripMission;
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
		if (!missionManager.CanStartMission()) return;

		if (other.CompareTag("Mini Van"))
		{
			missionVcam.Priority = 20;
			missionManager.StartMissionRelaxTrip();
			gameObject.SetActive(false);
			relaxTripMission.SetActive(true);
		}
	}
}
