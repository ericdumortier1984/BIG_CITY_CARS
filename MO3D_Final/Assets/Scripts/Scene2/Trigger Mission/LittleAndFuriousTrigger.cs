using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleAndFuriousTrigger : MonoBehaviour
{
	public MissionManager missionManager;

	[Header("Settings")]
	[SerializeField] private GameObject littleAndFurious;
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
		if (other.CompareTag("Mini Car"))
		{
			if (!missionManager.CanStartMission()) { return; }
			missionVcam.Priority = 20;
			missionManager.StartMissionLittleAndFurious();
			gameObject.SetActive(false);
			littleAndFurious.SetActive(true);
		}
		else if (other.CompareTag("IACar"))
		{
			UIMissionManager.Instance.ShowMissionText("", 1.0f, 50);
		}
		else
		{
			UIMissionManager.Instance.ShowMissionText("ONLY MINI CAR", 1.0F, 50);
		}
	}
}
