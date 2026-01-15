using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PackageDelivery : MonoBehaviour
{
	[Header("Spawn Settings")]
	[SerializeField] private List<GameObject> packagePrefab;
	[SerializeField] Vector3 packageOffset;
	[SerializeField] private ParticleSystem packageParticle;
	[SerializeField] private ParticleSystem deliveryParticle;
	[SerializeField] private CountdownTimer countdownTimer;

	[Header("References")]
	[SerializeField] private Transform redFurgonPos;
	[SerializeField] private List<Transform> deliveryLocation;

	[Header("UI")]
	[SerializeField] private float textDuration;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private TextMeshProUGUI packageCounterText;
	[SerializeField] private TextMeshProUGUI timeCounterText;
	[SerializeField] private RouteDrawer routeDrawer;
	[SerializeField] private GameObject InstructionPanel;

	[Header("Mission Manager")]
	[SerializeField] private MissionManager missionManager;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip deliveryMissionMusic;
	[SerializeField] private AudioClip deliverySFX;
	[SerializeField] private AudioClip winSFX;
	[SerializeField] private AudioClip loseSFX;

	private int totalPackage = 6;
	private int deliveredPackage = 0;
	private int deliveryIndex = 0;
	private static bool isMedal = false;
	private bool missionMusicStarted = false;

	private void Start()
	{
		if (!missionMusicStarted)
		{
			AudioManager.Instance.PlayMissionMusic(deliveryMissionMusic);
			missionMusicStarted = true;
		}

		SpawnPackage();
		SpawnLocation();
		EnableUIElements();

		if (deliveryLocation.Count > 0)
		{
			routeDrawer.SetTarget(deliveryLocation[0]);
		}
	}

	private void Update()
	{
		UpdatePackageCounter();
		LoseMission();
	}

	private void SpawnPackage()
	{
		for (int i = 0; i < packagePrefab.Count; i++)
		{
			packagePrefab[i].SetActive(true);
			packagePrefab[i].transform.position = redFurgonPos.position + packageOffset;
			packagePrefab[i].transform.SetParent(redFurgonPos);
		}

		if (packagePrefab.Count > 0)
		{
			packageParticle.transform.position = packagePrefab[0].transform.position;
			packageParticle.Play();
		}
	}

	private void SpawnLocation()
	{
		for (int i = 0; i < deliveryLocation.Count; i++)
		{
			deliveryLocation[i].gameObject.SetActive(false);
		}

		if (deliveryLocation.Count > 0)
		{
			deliveryLocation[0].gameObject.SetActive(true);
			deliveryIndex = 0;
		}
	}

	private void EnableUIElements()
	{
		countdownTimer.StartTimer();
		UIMissionManager.Instance.ShowMissionText("DELIVER ALL PACKAGE", textDuration, 50);
		UIMissionManager.Instance.ShowTimer(true);
		UIMissionManager.Instance.SetPackageCounter(deliveredPackage, totalPackage);
		InstructionPanel.SetActive(true);
	}

	private void WinMission()
	{
		foreach (GameObject package in packagePrefab)
			package.SetActive(false);

		AudioManager.Instance.PlaySFX(winSFX);
		AudioManager.Instance.PlayGameplayMusic();
		missionMusicStarted = false;

		countdownTimer.StopTimer();
		routeDrawer.gameObject.SetActive(false);
		InstructionPanel.SetActive(false);

		UIMissionManager.Instance.ShowMissionText("ALL PACKAGE DELIVERED\n + 2000 COINS", textDuration, 50);
		UIMissionManager.Instance.HideCounter();
		UIMissionManager.Instance.ShowTimer(false);

		missionManager.EndMission();

		if (!isMedal)
		{
			MainMenu.Instance.AddMedal(1);
			LevelData.MedalCollectedInLevel += 1;
			MainMenu.Instance.AddCoin(2000);
			LevelData.CoinsCollectedInLevel += 2000;
			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[3] = true;
			SaveSystem.SaveGame(saveData);
			isMedal = true;
		}
	}

	private void LoseMission()
	{
		if (countdownTimer.IsTimeUp)
		{
			foreach (GameObject package in packagePrefab)
				package.SetActive(false);

			countdownTimer.StopTimer();
			routeDrawer.gameObject.SetActive(false);
			InstructionPanel.SetActive(false);

			AudioManager.Instance.PlaySFX(loseSFX);
			AudioManager.Instance.PlayGameplayMusic();
			missionMusicStarted = false;

			UIMissionManager.Instance.ShowMissionText("TIME UP", textDuration, 50);
			UIMissionManager.Instance.HideCounter();
			UIMissionManager.Instance.ShowTimer(false);

			missionManager.EndMission();

			foreach (GameObject package in packagePrefab)
			{
				package.SetActive(false);
			}

			foreach (Transform point in deliveryLocation)
			{
				point.gameObject.SetActive(false);
			}
		}
	}

	private void UpdatePackageCounter()
	{
		UIMissionManager.Instance.SetPackageCounter(deliveredPackage, totalPackage);
	}

	public void DeliveryPackage(DeliveryPoint point)
	{
		if (deliveredPackage >= totalPackage) return;

		deliveryParticle.transform.position = transform.position;
		deliveryParticle.Play();

		point.gameObject.SetActive(false);

		AudioManager.Instance.PlaySFX(deliverySFX);

		deliveredPackage++;
		UpdatePackageCounter();

		deliveryIndex++;

		if (deliveryIndex < deliveryLocation.Count)
		{
			deliveryLocation[deliveryIndex].gameObject.SetActive(true);
			routeDrawer.SetTarget(deliveryLocation[deliveryIndex]);
		}
		else
		{
			routeDrawer.ClearRoute();
			WinMission();
		}
	}

}

