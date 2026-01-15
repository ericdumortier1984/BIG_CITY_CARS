using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FuelUp : MonoBehaviour
{
	[Header("UI")]
	[SerializeField] private float textDuration;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private GameObject instructionPanel;
	[SerializeField] private Slider gasStationSlider;
	[SerializeField] private Slider fuelTankerTruckSlider;
	[SerializeField] private RouteDrawer routeDrawer;

	[Header("References")]
	[SerializeField] private MissionManager missionManager;
	[SerializeField] private List<Transform> fuelPumpPoints;
	[SerializeField] private Transform gasStationFuelPoint;
	[SerializeField] private Rigidbody playerRigidbody;

	[Header("Intro Camera")]
	[SerializeField] private Camera mainCamera;
	[SerializeField] private Camera introCamera;
	[SerializeField] private float cameraShowTime;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip fuelUpMusic;
	[SerializeField] private AudioClip collectSFX;
	[SerializeField] private AudioClip winSFX;
	[SerializeField] private AudioClip loseSFX;

	// BOOL
	private bool hasLost = false;
	private bool sliderOn = false;
	private bool objectiveCameraPlayed = false;

	// INT 
	private int fuelPumpIndex = 0;
	private int totalFuelPump = 4;

	// FLOAT
	private float currentTankerFuel = 0f;
	private float currentGasStationFuel = 0.3f;
	private float fuelLoaded = 0.25f;
	private float fuelSpend = 0.0005f;

	// MEDAL
	private static bool isMedal = false;

	// MUSIC MISSION START
	private bool missionMusicStarted = false;

	public void BeginFuelUp()
	{
		SetElements();
	}

	private void SetElements()
	{
		// INSTRUCTIONS
		objectiveCameraPlayed = true;
		StartCoroutine(ShowIntroCamera());
		UIMissionManager.Instance.ShowMissionText("GAS STATION NEEDS TO REFUEL", textDuration, 40);
		instructionPanel.SetActive(true);

		// MUSIC
		if (!missionMusicStarted)
		{
			AudioManager.Instance.PlayMissionMusic(fuelUpMusic);
			missionMusicStarted = true;
		}

		// FUEL PUMPS POINTS
		foreach (Transform fuelPumpPoint in fuelPumpPoints)
		{
			fuelPumpPoint.gameObject.SetActive(true);
		}

		// GAS STATION POINT
		gasStationFuelPoint.gameObject.SetActive(false);

		// COUNTER
		UIMissionManager.Instance.SetFuelPumpCounter(fuelPumpIndex, totalFuelPump);

		// SLIDERS
		fuelTankerTruckSlider.gameObject.SetActive(true);
		gasStationSlider.gameObject.SetActive(true);

		fuelTankerTruckSlider.value = currentTankerFuel;
		gasStationSlider.value = currentGasStationFuel;

		sliderOn = true;

		// ROUTE DRAWER
		routeDrawer.gameObject.SetActive(true);
		routeDrawer.SetTarget(fuelPumpPoints[fuelPumpIndex]);
	}

	private void Update()
	{
		SpendGasStationFuel();
	}

	public void FillTankFuelTruck(CollectFuelPump fuelPump)
	{
		AudioManager.Instance.PlaySFX(collectSFX);
		fuelPumpPoints[fuelPumpIndex].gameObject.SetActive(false);
		fuelPumpIndex++;
		UIMissionManager.Instance.SetFuelPumpCounter(fuelPumpIndex, totalFuelPump);
		UpdateSliderTankerTruck();

		if (fuelPumpIndex < fuelPumpPoints.Count)
		{
			routeDrawer.SetTarget(fuelPumpPoints[fuelPumpIndex]);
		}
		else
		{
			UIMissionManager.Instance.ShowMissionText("GO TO THE DOWNLOAD POINT", textDuration, 50);
			gasStationFuelPoint.gameObject.SetActive(true);
			routeDrawer.SetTarget(gasStationFuelPoint);
		}
	}

	private void UpdateSliderTankerTruck()
	{
		currentTankerFuel += fuelLoaded;
		currentTankerFuel = Mathf.Clamp(currentTankerFuel, 0, 1);

		fuelTankerTruckSlider.value = currentTankerFuel;
	}

	private void UpdateSliderGasStation()
	{
		gasStationSlider.value = currentGasStationFuel;
	}

	private void SpendGasStationFuel()
	{
		if (!sliderOn || hasLost) { return; }

		currentGasStationFuel -= fuelSpend * Time.deltaTime;
		currentGasStationFuel = Mathf.Clamp(currentGasStationFuel ,0, 1);

		UpdateSliderGasStation();

		if (currentGasStationFuel <= 0f)
		{
			LoseMission();
		}
	}

	public void GasStationFuelUp()
	{
		currentGasStationFuel += currentTankerFuel;
		currentGasStationFuel = Mathf.Clamp(currentGasStationFuel, 0, 1);

		currentTankerFuel = 0f;

		UpdateSliderGasStation();
		fuelTankerTruckSlider.value = currentTankerFuel;

		WinMission();
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
			MainMenu.Instance.AddCoin(7500);
			LevelData.CoinsCollectedInLevel += 7500;

			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[21] = true;
			SaveSystem.SaveGame(saveData);

			isMedal = true;
		}
	}

	private void LoseMission()
	{
		if (hasLost) { return; }

		AudioManager.Instance.PlaySFX(loseSFX);
		AudioManager.Instance.PlayGameplayMusic();
		missionMusicStarted = false;

		hasLost = true;
		StartCoroutine(ShowLoseMessage());
		DisableElements();
		missionManager.EndMission();
	}

	private void DisableElements()
	{
		sliderOn = false;
		routeDrawer.gameObject.SetActive(false);
		instructionPanel.SetActive(false);
		UIMissionManager.Instance.HideCounter();
		gasStationSlider.gameObject.SetActive(false);
		fuelTankerTruckSlider.gameObject.SetActive(false);
		gasStationFuelPoint.gameObject.SetActive(false);

		foreach (Transform fuelPumpPoint in fuelPumpPoints)
		{
			fuelPumpPoint.gameObject.SetActive(false);
		}
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("GAS STATION IS REFUEL! \n + 7500 COINS", textDuration, 50);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowLoseMessage()
	{
		UIMissionManager.Instance.ShowMissionText("TOO SLOWN!", textDuration, 50);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowIntroCamera()
	{
		playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
		mainCamera.enabled = false;
		introCamera.enabled = true;

		yield return new WaitForSeconds(cameraShowTime);

		playerRigidbody.constraints = RigidbodyConstraints.None;
		introCamera.enabled = false;
		mainCamera.enabled = true;
	}
}
