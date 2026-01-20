using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MaxSpeedPleaseTrigger : MonoBehaviour
{
	public MissionManager missionManager;

	[Header("Settings")]
	[SerializeField] private TMP_FontAsset font;
	[SerializeField] private GameObject maxSpeedPleaseMission;
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
		if (other.CompareTag("Muscle Car"))
		{
			if (!missionManager.CanStartMission()) { return; }
			missionVcam.Priority = 21;
			missionManager.StartMissionMaxSpeedPlease();
			gameObject.SetActive(false);
			maxSpeedPleaseMission.SetActive(true);
		}
		else if (other.CompareTag("IACar"))
		{
			UIMissionManager.Instance.ShowMissionText("", 1.0f, 50, font);
		}
		else
		{
			UIMissionManager.Instance.ShowMissionText("ONLY MUSCLE CAR", 1.0F, 50, font);
		}
	}
}
