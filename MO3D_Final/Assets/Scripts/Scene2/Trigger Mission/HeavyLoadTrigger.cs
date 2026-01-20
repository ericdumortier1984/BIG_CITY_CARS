using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeavyLoadTrigger : MonoBehaviour
{
    public MissionManager missionManager;

    [Header("Settings")]
	[SerializeField] private TMP_FontAsset font;
	[SerializeField] private GameObject heavyLoadMission;
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
		if (other.CompareTag("Red Truck"))
		{
			if (!missionManager.CanStartMission()) { return; }
			missionVcam.Priority = 20;
			missionManager.StartHeavyLoad();
			gameObject.SetActive(false);
		}
		else if (other.CompareTag("IACar"))
		{
			UIMissionManager.Instance.ShowMissionText("", 1.0f, 50, font);
		}
		else
		{
			UIMissionManager.Instance.ShowMissionText("ONLY RED TRUCK", 1.0f, 50, font);
		}
	}
}
