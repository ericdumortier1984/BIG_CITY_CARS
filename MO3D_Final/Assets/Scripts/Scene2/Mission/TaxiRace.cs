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
	[SerializeField] private RouteDrawer routeDrawer;

	[Header("Mission Manager")]
	[SerializeField] private MissionManager missionManager;

	[Header("Race Settings")]
	[SerializeField] private GameObject player;
	[SerializeField] private List<Transform> waypoints;
	[SerializeField] private List<GameObject> playerWaypoints;
	[SerializeField] private float taxiSpeed = 0f;
	[SerializeField] private float waypointThreshold = 0f;
	[SerializeField] private GameObject goal;
	[SerializeField] private GameObject triggermission;

	[Header("FX")]
	[SerializeField] private ParticleSystem spawnParticle;

	private int currentWaypointIndex = 0;
	private int currentPlayerWaypointIndex = 0;
	private static bool isMedal = false;

	void Start()
    {
		goal.SetActive(false);
		triggermission.SetActive(false);

		SpawnTaxiRacer();
		SetPlayerCheckpoint();
		SetLineRenderer();
	}

    void Update()
    {
		MoveTaxiRacer();
    }

	private void SpawnTaxiRacer()
	{
		UIMissionManager.Instance.ShowMissionText("TAXI RACE", textDuration);

		taxiRacer.SetActive(true);

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

	private void SetLineRenderer()
	{
		if (playerWaypoints.Count > 0)
		{
			routeDrawer.SetTarget(playerWaypoints[0].transform);
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
			routeDrawer.gameObject.SetActive(false);
		}
		
	}

	public void LoseMission()
	{
		UIMissionManager.Instance.ShowMissionText("TOO SLOWN!!", textDuration);
		missionManager.EndMission();
		taxiRacer.SetActive(false);
		goal.SetActive(false);
		playerWaypoints[currentPlayerWaypointIndex].SetActive(false);
		routeDrawer.gameObject.SetActive(false);
	}

	public GameObject GetTaxiRacer()
	{
		return taxiRacer;
	}

	private void SetPlayerCheckpoint()
	{
		for (int i = 0; i < playerWaypoints.Count; i++)
		{
			playerWaypoints[i].SetActive(i == 0);
		}
	}

	public void UpdatePlayerCheckpoints(GameObject checkpoint)
	{
		if (playerWaypoints[currentPlayerWaypointIndex] == checkpoint)
		{
			playerWaypoints[currentPlayerWaypointIndex].SetActive(false);
			currentPlayerWaypointIndex++;

			if (currentPlayerWaypointIndex < playerWaypoints.Count)
			{
				playerWaypoints[currentPlayerWaypointIndex].SetActive(true);
				routeDrawer.SetTarget(playerWaypoints[currentPlayerWaypointIndex].transform);
			}
			else 
			{
				routeDrawer.ClearRoute();
				//Debug.Log("All Checkpoint passed");
			}
		}
	}

	public bool IsCheckpointCompleted()
	{
		return currentPlayerWaypointIndex >= playerWaypoints.Count;
	}
}
