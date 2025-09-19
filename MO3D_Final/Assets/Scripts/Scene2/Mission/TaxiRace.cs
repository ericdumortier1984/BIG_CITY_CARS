using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaxiRace : MonoBehaviour
{
	[Header("Spawn Settings")]
	[SerializeField] private GameObject taxiRacer;

	[Header("UI")]
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private float textDuration;

	[Header("Mission Manager")]
	[SerializeField] private MissionManager missionManager;

	[Header("Mission Data")]
	[SerializeField] private MissionData missionData;

	[Header("Race Settings")]
	[SerializeField] private GameObject player;
	[SerializeField] private List<Transform> waypoints;
	[SerializeField] private float taxiSpeed = 0f;
	[SerializeField] private float waypointThreshold = 0f;
	[SerializeField] private GameObject goal;
	[SerializeField] private GameObject triggermission;

	[Header("FX")]
	[SerializeField] private ParticleSystem spawnParticle;

	private int currentWaypointIndex = 0;
	private static bool isMedal = false;

	void Start()
    {
		goal.SetActive(false);
		triggermission.SetActive(false);
		UIMissionManager.Instance.ShowMissionText("TAXI RACE", textDuration);
		SpawnTaxiRacer();
	}

    void Update()
    {
		MoveTaxiRacer();
    }

	private void SpawnTaxiRacer()
	{
		spawnParticle.transform.position = taxiRacer.transform.position;
		spawnParticle.Play();
		goal.SetActive(true);
	}

	private void MoveTaxiRacer()
	{
		if (currentWaypointIndex >= waypoints.Count)
		{
			return;
		}

		Transform target = waypoints[currentWaypointIndex];

		Vector3 direction = (target.position - taxiRacer.transform.position).normalized;
		taxiRacer.transform.position += direction * taxiSpeed * Time.deltaTime;
		taxiRacer.transform.forward = Vector3.Lerp(taxiRacer.transform.forward, direction, 10f * Time.deltaTime);

		float distance = Vector3.Distance(taxiRacer.transform.position, target.position);
		if (distance <= waypointThreshold)
		{
			currentWaypointIndex++;
		}
	}

	public void WinMission()
	{
		if(!isMedal)
		{
			UIMissionManager.Instance.ShowMissionText("WINNER!!\n + 15 COINS", textDuration);
			MainMenu.Instance.AddMedal(1);
			LevelData.MedalCollectedInLevel += 1;
			MainMenu.Instance.AddCoin(15);
			LevelData.CoinsCollectedInLevel += 15;
			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[2] = true;
			SaveSystem.SaveGame(saveData);
			isMedal = true;
			missionManager.EndMission();
			taxiRacer.SetActive(false);
			goal.SetActive(false);
		}
		
	}

	public void LoseMission()
	{
		UIMissionManager.Instance.ShowMissionText("TOO SLOWN!!", textDuration);
		missionManager.EndMission();
		taxiRacer.SetActive(false);
		goal.SetActive(false);
	}

	public GameObject GetTaxiRacer()
	{
		return taxiRacer;
	}
}
