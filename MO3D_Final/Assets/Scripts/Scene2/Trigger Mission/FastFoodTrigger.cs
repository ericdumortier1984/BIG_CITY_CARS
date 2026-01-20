using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FastFoodTrigger : MonoBehaviour
{
	public MissionManager missionManager;

	[Header("Settings")]
	[SerializeField] private TMP_FontAsset font;
	[SerializeField] private GameObject fastFoodMission;
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
		if (other.CompareTag("Fast Food Truck"))
		{
			if (!missionManager.CanStartMission()) { return; }
			missionVcam.Priority = 21;
			missionManager.StartFastFood();
			gameObject.SetActive(false);
			fastFoodMission.SetActive(true);
		}
		else if (other.CompareTag("IACar"))
		{
			UIMissionManager.Instance.ShowMissionText("", 1.0f, 50, font);
		}
		else
		{
			UIMissionManager.Instance.ShowMissionText("ONLY FAST FOOD TRUCK", 1.0F, 50, font);
		}
	}
}
