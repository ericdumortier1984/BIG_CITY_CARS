using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooHot : MonoBehaviour
{
    [Header("Fires")]
    [SerializeField] private List<GameObject> fires;

	[Header("Time")]
	[SerializeField] private CountdownTimer countdownTimer;

	[Header("UI")]
	[SerializeField] private TMP_FontAsset font;
	[SerializeField] float textDuration;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private GameObject InstructionPanel;
	[SerializeField] private RouteDrawer routeDrawer;
	[SerializeField] private RectTransform crosshair;

	[Header("References")]
	[SerializeField] private Transform fireTruck;
	[SerializeField] private MissionManager missionManager;
	[SerializeField] private ParticleSystem fireParticlePrefab;
	[SerializeField] private GameObject waterStreamPrefab;
	[SerializeField] private List<FireZoneTrigger> fireZoneTrigger;

	[Header("ShootMode")]
	[SerializeField] private CinemachineVirtualCamera shootCamera;
	[SerializeField] private Transform normalView;
	[SerializeField] private Transform shootView;
	[SerializeField] private FireTruckTurret fireTruckTurret;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip tooHotMusic;
	[SerializeField] private AudioClip collectSFX;
	[SerializeField] private AudioClip winSFX;
	[SerializeField] private AudioClip loseSFX;

	// BOOL 
	private bool isShootMode = false;
	private bool isAllFireExtingued = false;
	private bool hasLost = false;

	// INT
	private int firesIndex = 0;
	private int totalFires = 8;

	// MEDAL
	private static bool isMedal = false;

	// MUSIC MISSION START
	private bool missionMusicStarted = false;

	public void BeginTooHot()
    {
		SetElements();
    }

	private void Update()
	{
		LoseMission();
	}

	private void SetElements()
	{
		// INSTRUCTIONS
		UIMissionManager.Instance.ShowMissionText("EXTINGUE FIRES", 10, 80, font);
		InstructionPanel.SetActive(true);

		// MUSIC
		if (!missionMusicStarted)
		{
			AudioManager.Instance.PlayMissionMusic(tooHotMusic);
			missionMusicStarted = true;
		}

		// FIRE ZONE TRIGGERS
		foreach (var fireZone in fireZoneTrigger)
		{
			fireZone.gameObject.SetActive(true);
		}
		
		// TIMER AND COUNTER
		countdownTimer.StartTimer();
		UIMissionManager.Instance.ShowTimer(true);
		UIMissionManager.Instance.SetFiresCounter(firesIndex, totalFires);

		// ROUTE DRAWER
		GameObject firstFire = GetNextFireZone();

		if (firstFire != null)
		{
			routeDrawer.SetTarget(firstFire.transform);
		}
	}

	private GameObject GetNextFireZone()
	{
		GameObject nearest = null;
		float nearestDistance = Mathf.Infinity;

		foreach (var fire in fires)
		{
			if (fire != null && fire.activeSelf)
			{
				float distance = Vector3.Distance(fireTruck.position, fire.transform.position);
				if (distance < nearestDistance)
				{
					nearestDistance = distance;
					nearest = fire;
				}
			}
		}

		return nearest;
	}

	public void EnterWaterMode(FireZoneTrigger zone)
	{
		if (isShootMode) { return; }
		isShootMode = true;
		shootCamera.Priority = 20;
		fireTruckTurret.EnableTurret(true);
	}

	public void ExitWaterMode()
	{
		isShootMode = false;
		shootCamera.Priority = 19;
		fireTruckTurret.EnableTurret(false);
		crosshair.gameObject.SetActive(false);
	}

	public void OnFireExtinguished()
	{
		AudioManager.Instance.PlaySFX(collectSFX);

		firesIndex++;
		UIMissionManager.Instance.SetFiresCounter(firesIndex, totalFires);

		GameObject nextFire = GetNextFireZone();

		if (nextFire != null)
		{
			routeDrawer.SetTarget(nextFire.transform);
		}
		else
		{
			isAllFireExtingued = true;
			WinMission();
		}
	}

	public void WinMission()
	{
		AudioManager.Instance.PlaySFX(winSFX);
		AudioManager.Instance.PlayGameplayMusic();
		missionMusicStarted = false;

		isAllFireExtingued = true;
		missionManager.EndMission();
		StartCoroutine(ShowWinMessage());
		countdownTimer.StopTimer();
		DisableElements();

		if (!isMedal)
		{
			MainMenu.Instance.AddMedal(1);
			LevelData.MedalCollectedInLevel += 1;
			MainMenu.Instance.AddCoin(5000);
			LevelData.CoinsCollectedInLevel += 5000;

			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[8] = true;
			SaveSystem.SaveGame(saveData);

			isMedal = true;
		}
	}

	private void LoseMission()
	{
		if (countdownTimer.IsTimeUp && !hasLost)
		{
			AudioManager.Instance.PlaySFX(loseSFX);
			AudioManager.Instance.PlayGameplayMusic();
			missionMusicStarted = false;

			hasLost = true;
			StartCoroutine(ShowLoseMessage());
			DisableElements();
			missionManager.EndMission();
		}
	}

	private void DisableElements()
	{
		foreach (GameObject fire in fires)
		{
			fire.SetActive(false);
		}

		foreach (FireZoneTrigger fireZone in fireZoneTrigger)
		{
			fireZone.gameObject.SetActive(false);
		}

		waterStreamPrefab.SetActive(false);
		crosshair.gameObject.SetActive(false);
		InstructionPanel.SetActive(false);
		countdownTimer.StopTimer();
		routeDrawer.gameObject.SetActive(false);
		UIMissionManager.Instance.HideCounter();
		ExitWaterMode();
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("ALL FIRE EXTINGUISHED! + 5000 COINS", textDuration, 80, font);
		missionText.gameObject.SetActive(false);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowLoseMessage()
	{
		UIMissionManager.Instance.ShowMissionText("TIME UP!", textDuration, 80, font);
		missionText.gameObject.SetActive(false);
		yield return new WaitForSeconds(textDuration);
	}
}
