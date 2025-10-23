using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AlmostFerrari : MonoBehaviour
{
	[Header("Spawn Settings")]
	[SerializeField] private List<GameObject> racers;
	[SerializeField] private WheelController wheelController;
	[SerializeField] private List<SpeedBoost> speedBoost;
	[SerializeField] private List<OilSlippery> oilSlippery;
	[SerializeField] private List<SlowTime> slowTime;
	[SerializeField] private Rigidbody playerRb;

	[Header("UI")]
	[SerializeField] private GameObject leaderboardPanel;
	[SerializeField] private float textDuration;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private TextMeshProUGUI lapsText;
	[SerializeField] private TextMeshProUGUI positionText;
	[SerializeField] private TextMeshProUGUI leaderboardText;

	[Header("References")]
	[SerializeField] private MissionManager missionManager;
	[SerializeField] private TrafficLightsController raceLightController;

	[Header("Race Settings")]
	[SerializeField] private GameObject player;
	[SerializeField] private List<float> racerSpeed;
	[SerializeField] private List<Transform> racersWaypoints;
	[SerializeField] private List<GameObject> playerWaypoints;
	[SerializeField] private float waypointThreshold = 0f;
	[SerializeField] private GameObject goal;
	[SerializeField] private GameObject triggermission;

	[Header("FX")]
	[SerializeField] private List<ParticleSystem> spawnParticle;

	// BOOL
	private bool raceStarted = false;
	private bool raceFinished = false;

	// WAYPOINT
	private List<int> racerWaypointIndex = new List<int>();
	private int currentWaypointIndex = 0;
	private int currentPlayerWaypointIndex = 0;
	private int waypointsInLap = 17;

	// LAP
	private int currentLap = 1;
	private int totalLaps = 3;

	// POSITION
	private int position = 4;

	// MEDAL
	private static bool isMedal = false;

	private void Start()
	{
		BeginAlmostFerrari();
	}

	public void BeginAlmostFerrari()
	{
		StartCoroutine(RaceCountdown());
		SetElements();
		SetPlayerCheckpoint();
		SetLapCounter();
	}

	private void Update()
	{
		MoveRacer();
		UpdateRacePosition();
	}

	private void SetElements()
	{
		for (int i = 0; i < spawnParticle.Count; i++)
		{
			spawnParticle[i].gameObject.SetActive(true);
		}


		racerWaypointIndex.Clear();

		for (int i = 0; i < racers.Count; i++)
		{
			racerWaypointIndex.Add(0);
		}

		foreach (SpeedBoost sBoost in speedBoost)
		{
			sBoost.gameObject.SetActive(true);
		}
		foreach (OilSlippery oSlippery in oilSlippery)
		{
			oSlippery.gameObject.SetActive(true);
		}
		foreach (SlowTime sTime in slowTime)
		{
			sTime.gameObject.SetActive(true);
		}

		raceLightController.gameObject.SetActive(true);
		raceLightController.SwitchToRed();
	}

	private void MoveRacer()
	{
		if (!raceStarted || raceFinished) return;
		if (racersWaypoints == null || racersWaypoints.Count == 0) { return; }
		if (racers.Count == 0) { return; }

		for (int i = 0; i < racers.Count; i++)
		{
			if (i >= racerWaypointIndex.Count) { continue; }

			int index = racerWaypointIndex[i];

			if (index >= racersWaypoints.Count)
			{
				LoseMission();
				return;
			}

			Transform target = racersWaypoints[index];
			Vector3 direction = (target.position - racers[i].transform.position).normalized;

			racers[i].transform.position += direction * racerSpeed[i] * Time.deltaTime;
			racers[i].transform.forward = Vector3.Lerp(racers[i].transform.forward, direction, 10f * Time.deltaTime);

			float distance = Vector3.Distance(racers[i].transform.position, target.position);

			if (distance <= waypointThreshold)
			{
				racerWaypointIndex[i]++;
			}
		}

	}

	public void WinMission()
	{
		missionManager.EndMission();
		DisableElements();
		StartCoroutine(ShowWinMessage());

		for (int i = 0; i < racers.Count; i++)
		{
			racers[i].SetActive(false);
		}

		if (!isMedal)
		{
			MainMenu.Instance.AddMedal(1);
			LevelData.MedalCollectedInLevel += 1;
			MainMenu.Instance.AddCoin(15);
			LevelData.CoinsCollectedInLevel += 15;

			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[5] = true;
			SaveSystem.SaveGame(saveData);

			isMedal = true;
		}
	}

	public void LoseMission()
	{
		raceFinished = true;
		DisableElements();
		StartCoroutine(ShowLoseMessage());

		for (int i = 0; i < playerWaypoints.Count; i++)
		{
			playerWaypoints[i].SetActive(false);
		}
		for (int j = 0; j < racers.Count; j++)
		{
			racers[j].SetActive(false);
		}

		missionManager.EndMission();
	}

	public GameObject GetRacers()
	{
		for (int i = 0; i < racers.Count; i++)
		{
			return racers[i];
		}
		return null;
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
			}

			if (currentPlayerWaypointIndex % waypointsInLap == 0)
			{
				UpdateLapCounter();
			}
		}
	}

	public bool IsCheckpointCompleted()
	{
		return currentPlayerWaypointIndex >= playerWaypoints.Count;
	}

	private void SetLapCounter()
	{
		lapsText.text = "LAPS " + currentLap + " / " + totalLaps;
	}

	private void UpdateLapCounter()
	{
		if (currentLap < totalLaps)
		{
			currentLap++;
			SetLapCounter();
			UIMissionManager.Instance.ShowMissionText("LAP " + currentLap + " / " + totalLaps, 1.0f, 50);
		}
	}

	private void UpdateRacePosition()
	{
		if (!raceStarted || raceFinished) return;

		List<(string name, float progress, bool isPlayer)> positions = new List<(string, float, bool)>();

		string[] carNames = { "CAR 8", "CAR 45", "CAR 73" };

		// PLAYER
		int playerIndex = Mathf.Min(currentPlayerWaypointIndex, racersWaypoints.Count - 1);
		Transform nextPlayerWP = racersWaypoints[playerIndex];
		float playerDist = Vector3.Distance(player.transform.position, nextPlayerWP.position);
		float playerProgress = playerIndex + (1 - Mathf.Clamp01(playerDist / 100f));
		positions.Add(("TUNNING CAR", playerProgress, true));

		// NPC
		for (int i = 0; i < racers.Count; i++)
		{
			int index = Mathf.Min(racerWaypointIndex[i], racersWaypoints.Count - 1);
			Transform nextWP = racersWaypoints[index];
			float dist = Vector3.Distance(racers[i].transform.position, nextWP.position);
			float progress = index + (1 - Mathf.Clamp01(dist / 100f));

			string carName = i < carNames.Length ? carNames[i] : $"CAR {i + 1}";
			positions.Add((carName, progress, false));
		}

		// ORDENAR
		positions = positions.OrderByDescending(p => p.progress).ToList();

		// MOSTRAR 
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		sb.AppendLine("POSITIONS");
		for (int i = 0; i < positions.Count; i++)
		{
			sb.AppendLine($"{i + 1}. {positions[i].name}");
		}
		leaderboardText.text = sb.ToString();

		// ACTUALIZO PLAYER
		int playerPos = positions.FindIndex(p => p.isPlayer) + 1;
		position = playerPos;
	}

	private IEnumerator RaceCountdown()
	{
		wheelController.enabled = false;
		playerRb.constraints = RigidbodyConstraints.FreezeAll;

		UIMissionManager.Instance.ShowMissionText("READY", 1.0f, 150);
		yield return new WaitForSeconds(1.0f);

		UIMissionManager.Instance.ShowMissionText("SET", 1.0f, 150);
		yield return new WaitForSeconds(1.0f);

		raceLightController.SwitchToGreen();

		UIMissionManager.Instance.ShowMissionText("GO!!", 1.0f, 150);
		yield return new WaitForSeconds(1.0f);

		wheelController.enabled = true;
		playerRb.constraints = RigidbodyConstraints.None;

		leaderboardPanel.SetActive(true);

		raceStarted = true;
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("NUMBER ONE! \n + 15 COINS", textDuration, 50);
		missionText.gameObject.SetActive(false);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowLoseMessage()
	{
		UIMissionManager.Instance.ShowMissionText("TOO SLOW!", textDuration, 50);
		missionText.gameObject.SetActive(false);
		yield return new WaitForSeconds(textDuration);
	}

	private void DisableElements()
	{
		raceLightController.gameObject.SetActive(false);
		leaderboardPanel.SetActive(false);
		lapsText.gameObject.SetActive(false);
		foreach (SpeedBoost sBoost in speedBoost)
		{
			sBoost.gameObject.SetActive(false);
		}
		foreach (OilSlippery oSlippery in oilSlippery)
		{
			oSlippery.gameObject.SetActive(false);
		}
		foreach (SlowTime sTime in slowTime)
		{
			sTime.gameObject.SetActive(false);
		}
	}
}