using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTheAutopartsTrigger : MonoBehaviour
{
	public MissionManager missionManager;

	[Header("Settings")]
	[SerializeField] private GameObject autopartsMission;
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
		if (other.CompareTag("4x4"))
		{
			if (!missionManager.CanStartMission()) { return; }
			missionVcam.Priority = 21;
			missionManager.StartGetTheAutoparts();
			gameObject.SetActive(false);
			autopartsMission.SetActive(true);
		}
		else if (other.CompareTag("IACar"))
		{
			UIMissionManager.Instance.ShowMissionText("", 1.0f, 50);
		}
		else
		{
			UIMissionManager.Instance.ShowMissionText("ONLY 4X4 TRUCK", 1.0F, 50);
		}
	}
}
