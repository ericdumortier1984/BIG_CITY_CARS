using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaxiCab : MonoBehaviour
{
	[Header("Spawn Settings")]
	[SerializeField] private List<GameObject> passengerPrefab;
	[SerializeField] Vector3 passengerOffset;
	[SerializeField] private ParticleSystem passengerParticle;
	[SerializeField] private CountdownTimer countdownTimer;

	[Header("References")]
	[SerializeField] private Transform taxiPos;
	[SerializeField] private Transform passengerSitPoint;
	[SerializeField] private List<Transform> passengerLocation;
	[SerializeField] private List<Transform> dropOffLocation;

	[Header("UI")]
	[SerializeField] private float textDuration;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private TextMeshProUGUI passengerCounterText;
	[SerializeField] private TextMeshProUGUI timeCounterText;
	[SerializeField] private RouteDrawer routeDrawer;
	[SerializeField] private StressManager stressBar;

	[Header("Mission Manager")]
	[SerializeField] private MissionManager missionManager;

	private int totalPassenger = 5;
	private int safePassenger = 0;
	private int passengerIndex = 0;
	private static bool isMedal = false;

	public bool IsPassengerOnTaxi { get; private set; } = false;

	public void BeginTaxiCabMission()
	{
		EnableUIElements();
		SpawnPassenger();
	}

	private void Update()
	{
		UpdatePassengerCounter();

		if (countdownTimer.IsTimeUp && missionManager.IsMissionActive)
		{
			LoseMission();
		}
	}

	private void SpawnPassenger()
	{
		if (passengerIndex < 0 || passengerIndex >= passengerPrefab.Count) { return; }

		for (int i = 0; i < passengerPrefab.Count; i++)
		{
			passengerPrefab[i].SetActive(false);
			passengerLocation[i].gameObject.SetActive(false);
			dropOffLocation[i].gameObject.SetActive(false);
		}

		SetElements();
	}

	private void SetElements()
	{
		passengerPrefab[passengerIndex].gameObject.SetActive(true);
		passengerPrefab[passengerIndex].transform.position = passengerLocation[passengerIndex].transform.position + passengerOffset;
		passengerPrefab[passengerIndex].transform.SetParent(null);

		passengerLocation[passengerIndex].gameObject.SetActive(true);
		routeDrawer.SetTarget(passengerLocation[passengerIndex]);

		passengerParticle.gameObject.SetActive(true);
	}

	private void EnableUIElements()
	{
		countdownTimer.StartTimer();
		UIMissionManager.Instance.ShowMissionText("PASSENGER IS WAITING", textDuration, 50);
		UIMissionManager.Instance.ShowTimer(true);
		UIMissionManager.Instance.SetPassengerCounter(safePassenger, totalPassenger);
	}

	private void WinMission()
	{
		countdownTimer.StopTimer();
		routeDrawer.gameObject.SetActive(false);

		StartCoroutine(ShowWinMessage());

		missionManager.EndMission();

		if (!isMedal)
		{
			MainMenu.Instance.AddMedal(1);
			LevelData.MedalCollectedInLevel += 1;
			MainMenu.Instance.AddCoin(15);
			LevelData.CoinsCollectedInLevel += 15;

			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[4] = true;
			SaveSystem.SaveGame(saveData);

			isMedal = true;
		}
	}

	private void LoseMission(bool stressedOut = false)
	{
		if (!stressedOut && !countdownTimer.IsTimeUp)
			return;

		foreach (GameObject passenger in passengerPrefab)
			passenger.SetActive(false);

		foreach (Transform pick in passengerLocation)
			pick.gameObject.SetActive(false);

		foreach (Transform drop in dropOffLocation)
			drop.gameObject.SetActive(false);

		countdownTimer.StopTimer();
		routeDrawer.ClearRoute();
		if (stressedOut)
		{
			StartCoroutine(ShowStressOutMessage());
		}
		else
		{
			StartCoroutine(ShowTimeUpMessage());
		}
			

		missionManager.EndMission();
	}

	public void StressedOut()
	{
		LoseMission(true);
	}

	private void UpdatePassengerCounter()
	{
		if (missionManager.IsMissionActive && safePassenger < totalPassenger)
		{
			UIMissionManager.Instance.SetPassengerCounter(safePassenger, totalPassenger);
		}
	}

	public void PickUpPassenger(PickUpPassengerPoint pickUpPoint)
	{
		if (IsPassengerOnTaxi) return;

		int current = passengerIndex;

		passengerParticle.transform.position = passengerPrefab[current].transform.position;
		passengerParticle.Play();

		passengerPrefab[current].transform.position = passengerSitPoint.position + passengerOffset;
		passengerPrefab[current].transform.rotation = taxiPos.rotation;
		passengerPrefab[current].transform.SetParent(taxiPos);

		IsPassengerOnTaxi = true;

		passengerLocation[current].gameObject.SetActive(false);

		stressBar.BeginStressActivity();

		if (dropOffLocation.Count > current)
		{
			dropOffLocation[current].gameObject.SetActive(true);
			routeDrawer.SetTarget(dropOffLocation[current]);
		}

		UIMissionManager.Instance.ShowMissionText("TAKE PASSENGER SAFELY", textDuration, 50);
	}

	public void DropOffPassenger(DropOffPassengerPoint dropOffPoint)
	{
		if (!IsPassengerOnTaxi) return;

		int current = passengerIndex;

		passengerPrefab[current].SetActive(false);
		dropOffPoint.gameObject.SetActive(false);
		stressBar.StopStressActivity();

		passengerParticle.transform.position = dropOffPoint.transform.position;
		passengerParticle.Play();

		IsPassengerOnTaxi = false;
		safePassenger++;
		UpdatePassengerCounter();
		passengerIndex++;

		if (passengerIndex < passengerPrefab.Count)
		{
			SetElements();
			UIMissionManager.Instance.ShowMissionText("NEXT PASSENGER IS WAITING", textDuration, 50);
		}
		else
		{
			routeDrawer.ClearRoute();
			WinMission();
		}
	}

	private void HideCounter()
	{
		if (safePassenger == totalPassenger)
		{
			UIMissionManager.Instance.HideCounter();
		}
	}

	private IEnumerator ShowTimeUpMessage()
	{
		UIMissionManager.Instance.ShowMissionText("TIME UP", textDuration, 50);
		UIMissionManager.Instance.HideCounter(); 
		UIMissionManager.Instance.ShowTimer(false);

		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("ALL PASSENGER DROPPED OFF\n + 15 COINS", textDuration, 50);
		UIMissionManager.Instance.HideCounter();
		UIMissionManager.Instance.ShowTimer(false);

		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowStressOutMessage()
	{
		UIMissionManager.Instance.ShowMissionText("PASSENGER STRESSED OUT", textDuration, 50);
		UIMissionManager.Instance.HideCounter();
		UIMissionManager.Instance.ShowTimer(false);

		yield return new WaitForSeconds(textDuration);
	}
}


