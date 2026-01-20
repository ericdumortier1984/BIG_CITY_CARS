using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OffRoadAventure : MonoBehaviour
{
    [Header("Flags")]
    [SerializeField] private List<GameObject> flags;

	[Header("UI")]
	[SerializeField] private TMP_FontAsset font;
	[SerializeField] private float textDuration;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private GameObject instructionPanel;

	[Header("FX")]
	[SerializeField] private List<ParticleSystem> startParticle;

	[Header("References")]
	[SerializeField] private MissionManager missionManager;

	[Header("Time")]
	[SerializeField] private CountdownTimer countdownTimer;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip fastFoodMusic;
	[SerializeField] private AudioClip collectSFX;
	[SerializeField] private AudioClip winSFX;
	[SerializeField] private AudioClip loseSFX;

	// BOOL
	private bool hasLost = false;

	// INT
	private int flagsIndex = 0;
	private int totalFlags = 10;

	// MEDAL
	private static bool isMedal = false;

	// MUSIC MISSION START
	private bool missionMusicStarted = false;

	public void BeginOffRoadAdventure()
    {
		SetElements();
    }

	private void SetElements()
	{
		// INSTRUCTIONS
		UIMissionManager.Instance.ShowMissionText("GET THOSE FLAGS", textDuration, 80, font);
		instructionPanel.SetActive(true);

		// MUSIC
		if (!missionMusicStarted)
		{
			AudioManager.Instance.PlayMissionMusic(fastFoodMusic);
			missionMusicStarted = true;
		}

		// PARTICLES
		for (int i = 0; i < startParticle.Count; i++)
		{
			startParticle[i].gameObject.SetActive(true);
			startParticle[i].Play();
		}

		// TIMER AND COUNTER
		countdownTimer.StartTimer();
		UIMissionManager.Instance.ShowTimer(true);
		UIMissionManager.Instance.SetFlagsCounter(flagsIndex, totalFlags);
	}

	private void Update()
	{
		LoseMission();
	}

	public void GetFlag(GetOffRoadFlag flag)
	{
		AudioManager.Instance.PlaySFX(collectSFX);

		flagsIndex++;
		UIMissionManager.Instance.SetFlagsCounter(flagsIndex, totalFlags);
		flag.gameObject.SetActive(false);

		if (flagsIndex >= totalFlags)
		{
			WinMission();
		}
	}

	private void WinMission()
	{
		AudioManager.Instance.PlaySFX(winSFX);
		AudioManager.Instance.PlayGameplayMusic();
		missionMusicStarted = false;

		missionManager.EndMission();
		StartCoroutine(ShowWinMessage());
		DisableElements();

		if (!isMedal)
		{
			MainMenu.Instance.AddMedal(1);
			LevelData.MedalCollectedInLevel += 1;
			MainMenu.Instance.AddCoin(9500);
			LevelData.CoinsCollectedInLevel += 9500;

			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[14] = true;
			SaveSystem.SaveGame(saveData);

			isMedal = true;
		}
	}

	private void LoseMission()
	{
		if (hasLost) { return; }
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
		foreach (GameObject flag in flags)
		{
			flag.SetActive(false);
		}
		instructionPanel.SetActive(false);
		UIMissionManager.Instance.HideCounter();
		countdownTimer.StopTimer();
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("YOUR A BEAST! + 9500 COINS", textDuration, 80, font);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowLoseMessage()
	{
		UIMissionManager.Instance.ShowMissionText("TIME UP!", textDuration, 80, font);
		yield return new WaitForSeconds(textDuration);
	}
}
