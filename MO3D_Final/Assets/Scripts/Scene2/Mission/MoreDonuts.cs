using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoreDonuts : MonoBehaviour
{
	[Header("Donuts")]
	[SerializeField] private List<GameObject> donuts;

	[Header("Time")]
	[SerializeField] private CountdownTimer countdownTimer;

	[Header("UI")]
    [SerializeField] float textDuration;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private GameObject InstructionPanel;
	[SerializeField] private RouteDrawer routeDrawer;

	[Header("References")]
	[SerializeField] private Transform policeCar;
	[SerializeField] private MissionManager missionManager;
	[SerializeField] private GameObject deliveryPoint;
	[SerializeField] private RadioMessagesController radioController;

	//BOOL 
	private bool isAllDonutsCollected = false;
	private bool hasLost = false;

	// INT
	private int donutsIndex = 0;
	private int totalDonuts = 10;

	//MEDAL
	private static bool isMedal = false;

	public void BeginMoreDonuts()
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
		UIMissionManager.Instance.ShowMissionText("COLLECT ALL DONUTS", 10, 70);
		InstructionPanel.SetActive(true);

		// TIMER AND COUNTER
		countdownTimer.StartTimer();
		UIMissionManager.Instance.ShowTimer(true);
		UIMissionManager.Instance.SetDonutsCounter(donutsIndex, totalDonuts);

		// RADIO MESSAGES
		radioController.gameObject.SetActive(true);
		radioController.StartRadio();

		if (donuts.Count > 0)
		{
			routeDrawer.SetTarget(donuts[0].transform);
		}
	}

	public void DonutCollected(CollectDonutsTrigger donut)
	{
		donutsIndex++;
		UIMissionManager.Instance.SetDonutsCounter(donutsIndex, totalDonuts);

		donut.gameObject.SetActive(false);

		GameObject nextDonut = GetNextDonut(donut.transform.position);

		if (nextDonut != null)
		{
			routeDrawer.SetTarget(nextDonut.transform);
		}
		else
		{
			// SI NO QUEDAN DONAS ACTIVAS
			isAllDonutsCollected = true;
			deliveryPoint.SetActive(true);
			routeDrawer.SetTarget(deliveryPoint.transform);
			UIMissionManager.Instance.ShowMissionText("DELIVERY POINT IS THE POLICE STATION", 10, 40);
		}
	}

	private GameObject GetNextDonut(Vector3 currentPosition)
	{
		GameObject nearest = null;
		float nearestDistance = Mathf.Infinity;

		foreach (GameObject donut in donuts)
		{
			if (donut != null && donut.activeSelf)
			{
				float distance = Vector3.Distance(currentPosition, donut.transform.position);
				if (distance < nearestDistance)
				{
					nearestDistance = distance;
					nearest = donut;
				}
			}
		}

		return nearest;
	}

	public void WinMission()
	{
		missionManager.EndMission();
		StartCoroutine(ShowWinMessage());
		countdownTimer.StopTimer();
		DisableElements();

		if (!isMedal)
		{
			MainMenu.Instance.AddMedal(1);
			LevelData.MedalCollectedInLevel += 1;
			MainMenu.Instance.AddCoin(15);
			LevelData.CoinsCollectedInLevel += 15;

			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[7] = true;
			SaveSystem.SaveGame(saveData);

			isMedal = true;
		}
	}

	private void LoseMission()
	{
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
		foreach (GameObject donut in donuts)
		{
			donut.SetActive(false);
		}
		InstructionPanel.SetActive(false);
		countdownTimer.StopTimer();
		routeDrawer.gameObject.SetActive(false);
		deliveryPoint.SetActive(false);
		radioController.StopRadio();
		radioController.gameObject.SetActive(false);
		UIMissionManager.Instance.HideCounter();
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("YOU WILL BE DECORATED! \n + 15 COINS", textDuration, 50);
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
