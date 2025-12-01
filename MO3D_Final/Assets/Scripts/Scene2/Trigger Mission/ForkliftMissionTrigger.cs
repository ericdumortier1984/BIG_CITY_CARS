using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkliftMissionTrigger : MonoBehaviour
{
	public MissionManager missionManager;

	[Header("Settings")]
	[SerializeField] private GameObject forkLiftMission;
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
		if (other.CompareTag("Forklift"))
		{
			if (!missionManager.CanStartMission()) { return; }
			missionVcam.Priority = 20;
			missionManager.StartMissionForkLift();
			gameObject.SetActive(false);
			forkLiftMission.SetActive(true);
		}
		else if (other.CompareTag("IACar"))
		{
			UIMissionManager.Instance.ShowMissionText("", 1.0f, 50);
		}
		else
		{
			UIMissionManager.Instance.ShowMissionText("ONLY FORKLIFT", 1.0F, 50);
		}
	}
}
