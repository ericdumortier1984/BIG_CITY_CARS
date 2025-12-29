using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SupplyDeliveryTrigger : MonoBehaviour
{
	public MissionManager missionManager;

	[Header("Settings")]
	[SerializeField] private GameObject supplyDeliveryMission;
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
		if (other.CompareTag("Delivery Truck"))
		{
			if (!missionManager.CanStartMission()) { return; }

			missionVcam.Priority = 21;
			missionManager.StartSupplyDelivery();
			gameObject.SetActive(false);
			supplyDeliveryMission.SetActive(true);
		}
		else if (other.CompareTag("IACar"))
		{
			UIMissionManager.Instance.ShowMissionText("", 1.0f, 50);
		}
		else 
		{
			UIMissionManager.Instance.ShowMissionText("ONLY DELIVERY TRUCK", 1.0f, 50);
		}
	}
}
