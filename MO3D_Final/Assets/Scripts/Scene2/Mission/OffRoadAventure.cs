using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OffRoadAventure : MonoBehaviour
{
    [Header("Flags")]
    [SerializeField] private List<GameObject> flags;

	[Header("UI")]
	[SerializeField] private float textDuration;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private GameObject instructionPanel;

	[Header("FX")]
	[SerializeField] private List<ParticleSystem> startParticle;

	[Header("References")]
	[SerializeField] private MissionManager missionManager;

	[Header("Time")]
	[SerializeField] private CountdownTimer countdownTimer;

	// BOOL
	private bool hasLost = false;

	// INT
	private int flagsIndex = 0;
	private int totalFlags = 10;

	// MEDAL
	private static bool isMedal = false;

	public void BeginOffRoadAdventure()
    {
		SetElements();
    }

	private void SetElements()
	{
		// INSTRUCTIONS
		UIMissionManager.Instance.ShowMissionText("GET THOSE FLAGS", textDuration, 40);
		instructionPanel.SetActive(true);

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
		missionManager.EndMission();
		StartCoroutine(ShowWinMessage());
		DisableElements();

		if (!isMedal)
		{
			MainMenu.Instance.AddMedal(1);
			LevelData.MedalCollectedInLevel += 1;
			MainMenu.Instance.AddCoin(15);
			LevelData.CoinsCollectedInLevel += 15;

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
		UIMissionManager.Instance.ShowMissionText("YOUR A BEAST!  \n + 15 COINS", textDuration, 50);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowLoseMessage()
	{
		UIMissionManager.Instance.ShowMissionText("TIME UP!", textDuration, 50);
		yield return new WaitForSeconds(textDuration);
	}
}
