using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParkingOdysseyTrigger : MonoBehaviour
{
    public MissionManager missionManager;

    [Header("Settings")]
	[SerializeField] private TMP_FontAsset font;
	[SerializeField] private GameObject ParkingOdysseyMission;
    [SerializeField] CinemachineVirtualCamera missionVcam;
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
		if (other.CompareTag("Big Truck"))
		{
			if (!missionManager.CanStartMission()) { return; }
			missionVcam.Priority = 21;
			missionManager.StartParkingOdyssey();
			gameObject.SetActive(false);
		}
		else if (other.CompareTag("IACar"))
		{
			UIMissionManager.Instance.ShowMissionText("", 1.0f, 50, font);
		}
		else
		{
			UIMissionManager.Instance.ShowMissionText("ONLY BIG TRUCK", 1.0f, 50, font);
		}
	}
}
