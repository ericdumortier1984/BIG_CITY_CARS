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
		if (other.CompareTag("Mini Van"))
		{
			if (!missionManager.CanStartMission()) { return; }
			missionVcam.Priority = 21;
			missionManager.StartMissionRelaxTrip();
			gameObject.SetActive(false);
			relaxTripMission.SetActive(true);
		}
		else if (other.CompareTag("IACar"))
		{
			UIMissionManager.Instance.ShowMissionText("", 1.0f, 50);
		}
		else
		{
			UIMissionManager.Instance.ShowMissionText("ONLY MINIVAN", 1.0F, 50);
		}
	}
}
