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
	[SerializeField] private GameObject instructionPanel;

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

	[Header("SFX Clips")]
	[SerializeField] private AudioClip taxiRaceMusic;
	[SerializeField] private AudioClip collectSFX;
	[SerializeField] private AudioClip winSFX;
	[SerializeField] private AudioClip loseSFX;

	private int currentWaypointIndex = 0;
	private int currentPlayerWaypointIndex = 0;
	private static bool isMedal = false;

	// MUSIC MISSION START
	private bool missionMusicStarted = false;

	void Start()
    {
		goal.SetActive(false);
		triggermission.SetActive(false);

		SpawnTaxiRacer();
		EnableMusicMission();
		SetPlayerCheckpoint();
		SetLineRenderer();
	}

    void Update()
    {
		MoveTaxiRacer();
    }

	private void SpawnTaxiRacer()
	{
		UIMissionManager.Instance.ShowMissionText("TAXI RACE", textDuration, 50);

		taxiRacer.SetActive(true);
		instructionPanel.SetActive(true);

		spawnParticle.transform.position = taxiRacer.transform.position;
		spawnParticle.Play();
		goal.SetActive(true);
	}

	private void EnableMusicMission()
	{
		// MUSIC
		if (!missionMusicStarted)
		{
			AudioManager.Instance.PlayMissionMusic(taxiRaceMusic);
			missionMusicStarted = true;
		}
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
			AudioManager.Instance.PlaySFX(winSFX);
			AudioManager.Instance.PlayGameplayMusic();
			missionMusicStarted = false;

			UIMissionManager.Instance.ShowMissionText("WINNER!!\n + 5000 COINS", textDuration, 50);
			MainMenu.Instance.AddMedal(1);
			LevelData.MedalCollectedInLevel += 1;
			MainMenu.Instance.AddCoin(5000);
			LevelData.CoinsCollectedInLevel += 5000;
			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[2] = true;
			SaveSystem.SaveGame(saveData);
			isMedal = true;
			missionManager.EndMission();
			taxiRacer.SetActive(false);
			goal.SetActive(false);
			routeDrawer.gameObject.SetActive(false);
			instructionPanel.SetActive(false);
		}
		
	}

	public void LoseMission()
	{
		AudioManager.Instance.PlaySFX(loseSFX);
		AudioManager.Instance.PlayGameplayMusic();
		missionMusicStarted = false;

		UIMissionManager.Instance.ShowMissionText("TOO SLOWN!!", textDuration, 50);
		missionManager.EndMission();
		taxiRacer.SetActive(false);
		goal.SetActive(false);
		playerWaypoints[currentPlayerWaypointIndex].SetActive(false);
		routeDrawer.gameObject.SetActive(false);
		instructionPanel.SetActive(false);
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
			AudioManager.Instance.PlaySFX(collectSFX);

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
			}
		}
	}

	public bool IsCheckpointCompleted()
	{
		return currentPlayerWaypointIndex >= playerWaypoints.Count;
	}
}
