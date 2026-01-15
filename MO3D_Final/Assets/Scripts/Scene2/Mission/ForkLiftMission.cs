using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ForkLiftMission : MonoBehaviour
{
	[Header("Objects")]
	[SerializeField] private List<GameObject> pallets;
	[SerializeField] private List<GameObject> boxes;
	[SerializeField] private List<GameObject> trucks;
	[SerializeField] private List<GameObject> loadPoints;

	[Header("Time")]
	[SerializeField] private CountdownTimer countdownTimer;

	[Header("UI")]
	[SerializeField] float textDuration;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private GameObject instructionPanel;

	[Header("FX")]
	[SerializeField] private List<ParticleSystem> beginParticle;

	[Header("References")]
	[SerializeField] private MissionManager missionManager;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip forkliftMusic;
	[SerializeField] private AudioClip collectSFX;
	[SerializeField] private AudioClip winSFX;
	[SerializeField] private AudioClip loseSFX;

	//BOOL
	private bool hasLost = false;

	// INT
	private int palletIndex = 0;
	private int totalPallets = 4;

	// MEDAL
	private static bool isMedal = false;

	// MUSIC MISSION START
	private bool missionMusicStarted = false;

	public void BeginForkLiftMission()
    {
		SetElements();
    }

	private void SetElements()
	{
		// MUSIC
		if (!missionMusicStarted)
		{
			AudioManager.Instance.PlayMissionMusic(forkliftMusic);
			missionMusicStarted = true;
		}

		// COUNT
		totalPallets = pallets.Count;

		//PARTICLES
		for (int i = 0; i < beginParticle.Count; i++)
		{
			beginParticle[i].gameObject.SetActive(true);
		}

		// INSTRUCTIONS
		UIMissionManager.Instance.ShowMissionText("LOAD THOSE TRUCKS \n AND LOOK THE KEYS AT INTRUCTIONS", 15, 45);
		instructionPanel.SetActive(true);

		// TIMER AND COUNTER
		countdownTimer.StartTimer();
		UIMissionManager.Instance.ShowTimer(true);
		UIMissionManager.Instance.SetPalletsCounter(palletIndex, totalPallets);
	}

	private void Update()
	{
		LoseMission();
	}

	public void OnSetPallet()
	{
		AudioManager.Instance.PlaySFX(collectSFX);
		palletIndex++;
		UIMissionManager.Instance.SetPalletsCounter(palletIndex, totalPallets);

		if (palletIndex >= totalPallets)
		{
			WinMission();
		}
	}

	public void WinMission()
	{
		AudioManager.Instance.PlaySFX(winSFX);
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
			MainMenu.Instance.AddCoin(6000);
			LevelData.CoinsCollectedInLevel += 6000;

			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[11] = true;
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
		foreach (GameObject pallet in pallets)
		{
			pallet.SetActive(false);
		}
		foreach (GameObject box in boxes)
		{
			box.SetActive(false);
		}
		foreach (GameObject loadPoint in loadPoints)
		{
			loadPoint.SetActive(false);
		}
		instructionPanel.SetActive(false);
		countdownTimer.StopTimer();
		UIMissionManager.Instance.HideCounter();
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("GREAT JOB! \n + 6000 COINS", textDuration, 50);
		missionText.gameObject.SetActive(false);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowLoseMessage()
	{
		UIMissionManager.Instance.ShowMissionText("TIME UP!", textDuration, 50);
		missionText.gameObject.SetActive(false);
		yield return new WaitForSeconds(textDuration);
	}
}
