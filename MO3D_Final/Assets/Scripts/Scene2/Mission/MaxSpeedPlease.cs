using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class MaxSpeedPlease : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Rigidbody carPlayerRb;
	[SerializeField] private List<GameObject> checkpoints;
	[SerializeField] private List<GameObject> muscleCars;

	[Header("Time")]
    [SerializeField] private CountdownTimer countdownTimer;

    [Header("UI")]
	[SerializeField] private TMP_FontAsset font;
	[SerializeField] private float textDuration;
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private GameObject instructionPanel;
	[SerializeField] private RouteDrawer routeDrawer;

	[Header("FX")]
    [SerializeField] private ParticleSystem speedParticle;

    [Header("References")]
    [SerializeField] private MissionManager missionManager;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip maxSpeedPleaseMusic;
	[SerializeField] private AudioClip collectSFX;
	[SerializeField] private AudioClip winSFX;
	[SerializeField] private AudioClip loseSFX;

	//BOOL
	private bool hasLost = false;
    private bool isSpeeding = false;

    //INT
    private int checkpointIndex = 0;
    private int totalCheckpoint = 10;

    //FLOAT
    private float maxSpeed = 35f;

	// MEDAL
	private static bool isMedal = false;

	// MUSIC MISSION START
	private bool missionMusicStarted = false;

	public void BeginMission()
    {
		SetElements();
    }

	private void SetElements()
	{
		// COUNT
		totalCheckpoint = checkpoints.Count;

		// INSTRUCTIONS
		UIMissionManager.Instance.ShowMissionText("SPEED UP MAN!", 15, 50, font);
		instructionPanel.SetActive(true);

		// MUSIC
		if (!missionMusicStarted)
		{
			AudioManager.Instance.PlayMissionMusic(maxSpeedPleaseMusic);
			missionMusicStarted = true;
		}

		// TIMER AND COUNTER
		countdownTimer.StartTimer();
		UIMissionManager.Instance.ShowTimer(true);
		UIMissionManager.Instance.SetCheckpointCounter(checkpointIndex, totalCheckpoint);

		//ROUTE DRAWER
		if (checkpoints.Count > 0)
		{
			routeDrawer.gameObject.SetActive(true);
			routeDrawer.SetTarget(checkpoints[0].transform);
		}
	}

	private void Update()
	{
		SetSpeedParticle();
		LoseMission();
	}

	private void SetSpeedParticle()
	{
		float speed = carPlayerRb.velocity.magnitude * 3.6f;

		if (speed >= maxSpeed && !isSpeeding)
		{
			isSpeeding = true;
			speedParticle.Play();
		}
		else if (speed < maxSpeed && isSpeeding)
		{
			isSpeeding = false;
			speedParticle.Stop();
		}
	}

	public void OnCheckpoint(GetCheckpointMuscleCar checkpoint)
	{
		AudioManager.Instance.PlaySFX(collectSFX);
		checkpointIndex++;
		UIMissionManager.Instance.SetCheckpointCounter(checkpointIndex, totalCheckpoint);

		if (checkpointIndex < checkpoints.Count)
		{
			routeDrawer.SetTarget(checkpoints[checkpointIndex].transform);
		}
		else
		{
			WinMission();
		}
	}

	public void WinMission()
	{
		AudioManager.Instance.PlaySFX(loseSFX);
		AudioManager.Instance.PlayGameplayMusic();
		missionMusicStarted = false;

		missionManager.EndMission();
		StartCoroutine(ShowWinMessage());
		countdownTimer.StopTimer();
		DisableElements();

		if (!isMedal)
		{
			MainMenu.Instance.AddMedal(1);
			LevelData.MedalCollectedInLevel += 1;
			MainMenu.Instance.AddCoin(12000);
			LevelData.CoinsCollectedInLevel += 12000;

			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[12] = true;
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
		routeDrawer.gameObject.SetActive(false);
		foreach (GameObject checkpoint in checkpoints)
		{
			checkpoint.SetActive(false);
		}
		instructionPanel.SetActive(false);
		countdownTimer.StopTimer();
		UIMissionManager.Instance.HideCounter();
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("THATS A REAL MUSCLE CAR! + 12000 COINS", textDuration, 80, font);
		missionText.gameObject.SetActive(false);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowLoseMessage()
	{
		UIMissionManager.Instance.ShowMissionText("TOO SLOW!", textDuration, 80, font);
		missionText.gameObject.SetActive(false);
		yield return new WaitForSeconds(textDuration);
	}
}
