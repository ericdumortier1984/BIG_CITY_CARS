using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SupplyDelivery : MonoBehaviour
{
	[Header("UI")]
	[SerializeField] private TMP_FontAsset font;
	[SerializeField] private float textDuration;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private GameObject instructionPanel;
	[SerializeField] private RouteDrawer routeDrawer;

	[Header("References")]
	[SerializeField] private MissionManager missionManager;
	[SerializeField] private List<GameObject> supplys;
	[SerializeField] private List<Transform> loadSupplyPosition;
	[SerializeField] private List<Transform> unloadSupplyPosition;
	[SerializeField] private CinemachineVirtualCamera mainVcam;

	[Header("Time")]
	[SerializeField] private CountdownTimer countdownTimer;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip supplyDeliveryMusic;
	[SerializeField] private AudioClip collectSFX;
	[SerializeField] private AudioClip winSFX;
	[SerializeField] private AudioClip loseSFX;

	// BOOL 
	private bool hasLost = false;

	// INT
	private int supplyIndex = 0;
	private int totalSupply = 5;

	// MEDAL
	private static bool isMedal = false;

	// MUSIC MISSION START
	private bool missionMusicStarted = false;

	public void BeginSupplyDelivery()
	{
		SetElements();
	}

	private void SetElements()
	{
		// INSTRUCTIONS
		UIMissionManager.Instance.ShowMissionText("SUPPLY DELIVERY", textDuration, 80, font);
		instructionPanel.SetActive(true);

		// MUSIC
		if (!missionMusicStarted)
		{
			AudioManager.Instance.PlayMissionMusic(supplyDeliveryMusic);
			missionMusicStarted = true;
		}

		// LOAD AND UNLOAD POSITION
		foreach (Transform loadPosition in loadSupplyPosition)
		{
			loadPosition.gameObject.SetActive(false);
		}

		foreach (Transform unloadPosition in unloadSupplyPosition)
		{
			unloadPosition.gameObject.SetActive(false);
		}

		loadSupplyPosition[supplyIndex].gameObject.SetActive(true);

		// TIMER AND COUNTER
		UIMissionManager.Instance.SetSuppliesCounter(supplyIndex, totalSupply);
		countdownTimer.StartTimer();
		UIMissionManager.Instance.ShowTimer(true);

		// ROUTE DRAWER
		routeDrawer.gameObject.SetActive(true);
		routeDrawer.SetTarget(loadSupplyPosition[supplyIndex]);

		// CAMERA
		mainVcam.Priority = 20;
	}

	private void Update()
	{
		LoseMission();
	}

	public void Load(LoadSupply loadSupply)
	{
		AudioManager.Instance.PlaySFX(collectSFX);
		loadSupplyPosition[supplyIndex].gameObject.SetActive(false);
		unloadSupplyPosition[supplyIndex].gameObject.SetActive(true);
		routeDrawer.SetTarget(unloadSupplyPosition[supplyIndex]);
		ShowUnloadPositionMessage();
	}

	public void Unload(UnloadSupply unloadSupply)
	{
		AudioManager.Instance.PlaySFX(collectSFX);
		unloadSupplyPosition[supplyIndex].gameObject.SetActive(false);
		NextSupply();
	}

	private void NextSupply()
	{
		supplyIndex++;
		UIMissionManager.Instance.SetSuppliesCounter(supplyIndex, totalSupply);
		UIMissionManager.Instance.ShowMissionText("GO TO NEXT SUPPLY", textDuration, 50, font);

		if (supplyIndex >= totalSupply)
		{
			WinMission();
			return;
		}

		loadSupplyPosition[supplyIndex].gameObject.SetActive(true);
		routeDrawer.SetTarget(loadSupplyPosition[supplyIndex]);
	}

	private void ShowUnloadPositionMessage()
	{
		switch (supplyIndex)
		{
			case 0:
				UIMissionManager.Instance.ShowMissionText("UNLOAD THAT CHASIS IN SERVICE CAR", textDuration, 50, font);
				break;
			case 1:
				UIMissionManager.Instance.ShowMissionText("UNLOAD THOSE POWER CABS IN POWER PLANT", textDuration, 50, font);
				break;
			case 2:
				UIMissionManager.Instance.ShowMissionText("UNLOAD THOSE ROOF VENTS IN BUILDING COMPLEX", textDuration, 50, font);
				break;
			case 3:
				UIMissionManager.Instance.ShowMissionText("UNLOAD THOSE CABBAGES IN SUPERMARKET", textDuration, 50, font);
				break;
			case 4:
				UIMissionManager.Instance.ShowMissionText("UNLOAD THOSE CONTROL BOX IN STADIUM", textDuration, 50, font);
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
			MainMenu.Instance.AddCoin(3500);
			LevelData.CoinsCollectedInLevel += 3500;

			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[18] = true;
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
		countdownTimer.StopTimer();
		routeDrawer.gameObject.SetActive(false);
		mainVcam.Priority = 16;
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("ALL SUPPLIES DELIVERED! + 3500 COINS", textDuration, 80, font);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowLoseMessage()
	{
		UIMissionManager.Instance.ShowMissionText("TOO SLOW!", textDuration, 80, font);
		yield return new WaitForSeconds(textDuration);
	}
}
