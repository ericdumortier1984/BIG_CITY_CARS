using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopRobberBankTrigger : MonoBehaviour
{
    public MissionManager missionManager;

    [Header("Settings")]
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
		if (other.CompareTag("Police"))
		{
			if (!missionManager.CanStartMission()) { return; }
			missionVcam.Priority = 20;
			missionManager.StartMissionRobberBank();
			gameObject.SetActive(false);
		}
		else if (other.CompareTag("IA Car"))
		{
			UIMissionManager.Instance.ShowMissionText("", 1.0f, 50);
		}
		else
		{
			UIMissionManager.Instance.ShowMissionText("ONLY POLICE CAR", 1.0f, 50);
		}
	}
}
