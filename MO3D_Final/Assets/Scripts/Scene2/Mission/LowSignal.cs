using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LowSignal : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private float textDuration;
    [SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private RouteDrawer routeDrawer;
    [SerializeField] private GameObject instructionPanel;
	[SerializeField] private Image lowSignalPanelDistorsion;
	[SerializeField] private Image lowSignalPanelColor;
	[SerializeField] private List<Slider> signalChannels;
	
	[Header("References")]
    [SerializeField] private MissionManager missionManager;
	[SerializeField] private List<GameObject> tvAntennas;

	[Header("Time")]
	[SerializeField] private CountdownTimer countdownTimer;

	// BOOL 
	private bool hasLost = false;

	// INT
	private int totalTvAntennas = 5;
	private int indexTvAntennas = 0;

	// MEDAL
	private static bool isMedal = false;

	public void BeginLowSignal()
    {
        SetElements();
    }

    private void SetElements()
    {
		// INSTRUCTIONS
		Invoke(nameof(StartDistorsionView), 5f);
		instructionPanel.SetActive(true);
		UIMissionManager.Instance.ShowMissionText("FIX THOSE TV ANTENNAS", textDuration, 40);

		// SLIDERS
		foreach (Slider channel in signalChannels)
		{
			channel.gameObject.SetActive(true);
		}

		// TIMER
		countdownTimer.StartTimer();
		UIMissionManager.Instance.ShowTimer(true);

		// ROUTE DRAWER
		if (tvAntennas.Count > 0)
		{
			routeDrawer.gameObject.SetActive(true);
			routeDrawer.SetTarget(tvAntennas[0].transform);
		}
	}

	private void Update()
	{
		LoseMission();
	}

	public void FixTvSignal(CheckTvSignal tvAntenna)
	{
		indexTvAntennas++;
		FillSliderChannelDistorsion();

		// MEJORAR DISTORSION Y COLOR
		Color colorDistorsion = lowSignalPanelDistorsion.color;
		float colorDistorsionUpgrade = 30f / 255f;
		colorDistorsion.a = Mathf.Clamp01(colorDistorsion.a - colorDistorsionUpgrade);
		lowSignalPanelDistorsion.color = colorDistorsion;

		Color colorChannel = lowSignalPanelColor.color;
		colorChannel.a = Mathf.Clamp01(colorChannel.a - colorDistorsionUpgrade);
		lowSignalPanelColor.color = colorChannel;

		if (indexTvAntennas < tvAntennas.Count)
		{
			routeDrawer.SetTarget(tvAntennas[indexTvAntennas].transform);
		}
		else 
		{
			WinMission();
		}
	}

	private void FillSliderChannelDistorsion()
	{
		if (indexTvAntennas - 1 < signalChannels.Count)
		{
			signalChannels[indexTvAntennas - 1].value = 1f;
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
			saveData.missionCompleted[17] = true;
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
		routeDrawer.gameObject.SetActive(false);
		instructionPanel.SetActive(false);
		lowSignalPanelDistorsion.gameObject.SetActive(false);
		lowSignalPanelColor.gameObject.SetActive(false);
		countdownTimer.StopTimer();

		foreach (Slider channel in signalChannels)
		{
			channel.gameObject.SetActive(false);
		}
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("GREAT, GET POPCORN! \n + 15 COINS", textDuration, 50);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowLoseMessage()
	{
		UIMissionManager.Instance.ShowMissionText("FORGET IT, THE NOVEL IS OVER!", textDuration, 50);
		yield return new WaitForSeconds(textDuration);
	}

	private void StartDistorsionView()
	{
		lowSignalPanelDistorsion.gameObject.SetActive(true);
		lowSignalPanelColor.gameObject.SetActive(true);
	}
}
