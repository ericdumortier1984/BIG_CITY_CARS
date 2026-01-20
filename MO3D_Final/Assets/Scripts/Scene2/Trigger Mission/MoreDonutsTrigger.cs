using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoreDonutsTrigger : MonoBehaviour
{
	public MissionManager missionManager;

	[Header("Settings")]
	[SerializeField] private TMP_FontAsset font;
	[SerializeField] private GameObject moreDonuts;
	[SerializeField] private RouteDrawer routeDrawer;
	[SerializeField] private RadioMessagesController radioMessagesController;
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
		if (other.CompareTag("Police"))
		{
			if (!missionManager.CanStartMission()) { return; }
			missionVcam.Priority = 20;
			missionManager.StartMissionMoreDonuts();
			gameObject.SetActive(false);
			moreDonuts.SetActive(true);
			radioMessagesController.gameObject.SetActive(true);
			routeDrawer.gameObject.SetActive(true);
		}
		else if (other.CompareTag("IACar"))
		{
			UIMissionManager.Instance.ShowMissionText("", 1.0f, 50, font);
		}
		else
		{
			UIMissionManager.Instance.ShowMissionText("ONLY POLICE CAR", 1.0F, 50, font);
		}
	}
}
