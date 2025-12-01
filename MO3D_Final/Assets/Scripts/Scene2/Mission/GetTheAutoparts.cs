using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetTheAutoparts : MonoBehaviour
{
	[Header("Autoparts")]
	[SerializeField] private List<GameObject> autoparts;

	[Header("Autoparts Construction Panel")]
	[SerializeField] private List<Image> autopartImages;
	[SerializeField] private List<Color> colorAutopartsImages;

	[Header("UI")]
	[SerializeField] private float textDuration;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private GameObject instructionPanel;
	[SerializeField] private GameObject constructionPanel;
	[SerializeField] private RouteDrawer routeDrawer;

	[Header("References")]
	[SerializeField] private MissionManager missionManager;
	[SerializeField] private GameObject car;

	[Header("Time")]
	[SerializeField] private CountdownTimer countdownTimer;

	// BOOL 
	private bool hasLost = false;

    // INT
    private int autopartsIndex = 0;
    private int totalAutoparts = 7;

	// MEDAL
	private static bool isMedal = false;

	public void BeginGetTheAutoparts()
    {
        SetElements();
    }

    private void SetElements()
    {
        // INSTRUCTIONS
        UIMissionManager.Instance.ShowMissionText("GET THE AUTOPARTS", textDuration, 40);
		instructionPanel.SetActive(true);
		constructionPanel.SetActive(true);

		// TIMER AND COUNTER
		countdownTimer.StartTimer();
		UIMissionManager.Instance.ShowTimer(true);
		UIMissionManager.Instance.SetAutopartsCounter(autopartsIndex, totalAutoparts);

		// ROUTE DRAWER
		if (autoparts.Count > 0)
		{
			routeDrawer.gameObject.SetActive(true);
			routeDrawer.SetTarget(autoparts[0].transform);
		}

		// CAR
		car.SetActive(false);
    }

	private void Update()
	{
		LoseMission();
	}

	private void WinMission()
	{
		car.SetActive(true);
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
			saveData.missionCompleted[15] = true;
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

	public void CollectAutopart(GetAutopart getAutopart)
	{
		autopartsIndex++;
		UIMissionManager.Instance.SetAutopartsCounter(autopartsIndex, totalAutoparts);

		PaintCollectedPart(getAutopart.autopartID);

		if (autopartsIndex < autoparts.Count)
		{
			routeDrawer.SetTarget(autoparts[autopartsIndex].transform);
		}
		else 
		{
			WinMission();
		}
	}

	private void PaintCollectedPart(int autopartID)
	{
		if (autopartID < 0 || autopartID >= autopartImages.Count) {return;}
		if (autopartID >= colorAutopartsImages.Count) { return; }

		Image autopartImage = autopartImages[autopartID];
		Color autopartColor = colorAutopartsImages[autopartID];
		autopartColor.a = 0.5f;
		autopartImage.color = autopartColor;
	}


	private void DisableElements()
	{
		foreach (GameObject autopart in autoparts)
		{
			autopart.SetActive(false);
		}
		instructionPanel.SetActive(false);
		constructionPanel.SetActive(false);
		UIMissionManager.Instance.HideCounter();
		countdownTimer.StopTimer();
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("DONE, GO CHECKOUT THE CAR IN THE SERVICE SHOP \n + 15 COINS", textDuration, 50);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowLoseMessage()
	{
		UIMissionManager.Instance.ShowMissionText("TIME UP!", textDuration, 50);
		yield return new WaitForSeconds(textDuration);
	}
}
