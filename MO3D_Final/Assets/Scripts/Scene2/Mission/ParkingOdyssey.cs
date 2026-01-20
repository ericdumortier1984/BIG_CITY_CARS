using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class ParkingOdyssey : MonoBehaviour
{
    [Header("UI")]
	[SerializeField] private TMP_FontAsset font;
	[SerializeField] private float textDuration;
    [SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private GameObject instructionPanel;
	[SerializeField] private RouteDrawer routeDrawer;

    [Header("References")]
    [SerializeField] private MissionManager missionManager;
	[SerializeField] private CinemachineVirtualCamera parkingView;
	[SerializeField] private List<Transform> parkingPoints;

	[Header("Time")]
	[SerializeField] private CountdownTimer countdownTimer;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip parkingOdisseyMusic;
	[SerializeField] private AudioClip parkedSFX;
	[SerializeField] private AudioClip winSFX;
	[SerializeField] private AudioClip loseSFX;

	// BOOL
	private bool hasLost = false;

	// INT
	private int parkingIndex = 0;
	private int totalParking = 5;

    // MEDAL
    private static bool isMedal = false;

	// MUSIC MISSION START
	private bool missionMusicStarted = false;

	public void BeginParkingOdyssey()
    {
        SetElements();
    }

    private void SetElements()
    {
		// INSTRUCTIONS
		UIMissionManager.Instance.ShowMissionText("PUT THE TRUCK IN REVERSE TO UNLOAD", textDuration, 80, font);
		instructionPanel.SetActive(true);

		// MUSIC
		if (!missionMusicStarted)
		{
			AudioManager.Instance.PlayMissionMusic(parkingOdisseyMusic);
			missionMusicStarted = true;
		}

		// PARKING POINTS
		foreach (Transform parkingPoint in parkingPoints)
		{
			parkingPoint.gameObject.SetActive(false);
		}

		parkingPoints[parkingIndex].gameObject.SetActive(true);

		// COUNTER AND TIMER
		UIMissionManager.Instance.SetParkingCounter(parkingIndex, totalParking);
		countdownTimer.StartTimer();
		UIMissionManager.Instance.ShowTimer(true);

		// ROUTE DRAWER
		routeDrawer.gameObject.SetActive(true);
		routeDrawer.SetTarget(parkingPoints[parkingIndex]);

		// CAMERA
		parkingView.Priority = 21;
	}

	private void Update()
	{
		LoseMission();
	}

	public void TruckParked(ParkingZoneController parkingZone)
	{
		AudioManager.Instance.PlaySFX(parkedSFX);
		parkingPoints[parkingIndex].gameObject.SetActive(false);
		NextParkingZone();
	}

	private void NextParkingZone()
	{
		parkingIndex++;
		UIMissionManager.Instance.SetParkingCounter(parkingIndex, totalParking);
		ShowUnloadPositionMessage();

		if (parkingIndex >= totalParking)
		{
			WinMission();
			return;
		}

		parkingPoints[parkingIndex].gameObject.SetActive(true);
		routeDrawer.SetTarget(parkingPoints[parkingIndex]);
	}

	private void ShowUnloadPositionMessage()
	{
		switch (parkingIndex)
		{
			case 0:
				UIMissionManager.Instance.ShowMissionText("UNLOAD IN SUPERMARKET", textDuration, 50, font);
				break;
			case 1:
				UIMissionManager.Instance.ShowMissionText("NEXT UNLOAD IN STADIUM", textDuration, 50, font);
				break;
			case 2:
				UIMissionManager.Instance.ShowMissionText("NEXT UNLOAD IN FARM", textDuration, 50, font);
				break;
			case 3:
				UIMissionManager.Instance.ShowMissionText("NEXT UNLOAD IN CAMP", textDuration, 50, font);
				break;
			case 4:
				UIMissionManager.Instance.ShowMissionText("NEXT UNLOAD IN WORKER", textDuration, 50, font);
				break;
			default:
				break;
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
			MainMenu.Instance.AddCoin(4000);
			LevelData.CoinsCollectedInLevel += 4000;

			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[19] = true;
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
		UIMissionManager.Instance.HideCounter();
		instructionPanel.SetActive(false);
		routeDrawer.gameObject.SetActive(false);
		countdownTimer.StopTimer();
		parkingView.Priority = 16;
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("YOUR THE BEST! + 4000 COINS", textDuration, 80, font);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowLoseMessage()
	{
		UIMissionManager.Instance.ShowMissionText("KEEP PRACTICE!", textDuration, 80, font);
		yield return new WaitForSeconds(textDuration);
	}
}
