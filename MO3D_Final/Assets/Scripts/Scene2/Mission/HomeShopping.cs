using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HomeShopping : MonoBehaviour
{
    [Header("Provisions")]
    [SerializeField] private List<GameObject> provisions;
	[SerializeField] private List<GameObject> provisionsPurchased;

	[Header("Time")]
	[SerializeField] private CountdownTimer countdownTimer;

	[Header("UI")]
	[SerializeField] private TMP_FontAsset font;
	[SerializeField] float textDuration;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private GameObject InstructionPanel;
	[SerializeField] private List<TextMeshProUGUI> provisionTexts;
	[SerializeField] private RouteDrawer routeDrawer;

	[Header("References")]
	[SerializeField] private Transform simpleCar;
	[SerializeField] private MissionManager missionManager;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip HomeShoppingMusic;
	[SerializeField] private AudioClip purchasedSFX;
	[SerializeField] private AudioClip winSFX;
	[SerializeField] private AudioClip loseSFX;

	// BOOL 
	private bool isAllProvisions = false;
	private bool hasLost = false;

	// INT
	private int purchasedIndex = 0;
	private int provisionIndex = 0;
	private int totalProvision = 10;

	// MEDAL
	private bool isMedal = false;

	// MUSIC MISSION START
	private static bool missionMusicStarted = false;

	public void BeginHomeShopping()
    {
		SetElements();
    }

    private void SetElements()
    {
		// COUNT
		totalProvision = provisions.Count;

		// INSTRUCTIONS
		UIMissionManager.Instance.ShowMissionText("GET ALL THE HOME STUFFS", 10, 80, font);
		InstructionPanel.SetActive(true);

		// MUSIC
		if (!missionMusicStarted)
		{
			AudioManager.Instance.PlayMissionMusic(HomeShoppingMusic);
			missionMusicStarted = true;
		}

		// TIMER AND COUNTER
		countdownTimer.StartTimer();
		UIMissionManager.Instance.ShowTimer(true);
		UIMissionManager.Instance.SetProvisionCounter(provisionIndex, totalProvision);

		// ROUTE DRAWER
		routeDrawer.gameObject.SetActive(true);
		routeDrawer.SetTarget(provisions[provisionIndex].transform);
	}

	private void Update()
	{
		LoseMission();
	}

	public void OnProvisionPurchased(CollectProvisionsTrigger provisionCollected)
	{
		AudioManager.Instance.PlaySFX(purchasedSFX);

		provisions[provisionIndex].SetActive(false);

		if (purchasedIndex < provisionsPurchased.Count) // COMPRADO
			provisionsPurchased[purchasedIndex].SetActive(true);

		if (provisionIndex < provisionTexts.Count) // TACHADO DE LA LISTA
		{
			provisionTexts[provisionIndex].fontStyle = FontStyles.Strikethrough;
			provisionTexts[provisionIndex].color = Color.gray;
		}

		provisionIndex++;
		purchasedIndex++;

		UIMissionManager.Instance.SetProvisionCounter(provisionIndex, totalProvision);

		if (provisionIndex < totalProvision) // SIGUIENTE EN LA RUTA
		{
			routeDrawer.SetTarget(provisions[provisionIndex].transform);
		}
		else
		{
			WinMission();
		}
	}


	public void WinMission()
	{
		AudioManager.Instance.PlaySFX(winSFX);
		AudioManager.Instance.PlayGameplayMusic();
		missionMusicStarted = false;

		isAllProvisions = true;
		missionManager.EndMission();
		StartCoroutine(ShowWinMessage());
		countdownTimer.StopTimer();
		DisableElements();

		if (!isMedal)
		{
			MainMenu.Instance.AddMedal(1);
			LevelData.MedalCollectedInLevel += 1;
			MainMenu.Instance.AddCoin(1500);
			LevelData.CoinsCollectedInLevel += 1500;

			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[10] = true;
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
		foreach (GameObject prvs in provisions)
		{
			prvs.SetActive(false);
		}

		foreach (GameObject purchPrvs in provisionsPurchased)
		{
			purchPrvs.SetActive(false);
		}

		InstructionPanel.SetActive(false);
		countdownTimer.StopTimer();
		routeDrawer.gameObject.SetActive(false);
		UIMissionManager.Instance.HideCounter();
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("ALL HOME STUFF PURCHASED! + 1500 COINS", textDuration, 80, font);
		missionText.gameObject.SetActive(false);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowLoseMessage()
	{
		UIMissionManager.Instance.ShowMissionText("TIME UP!", textDuration, 80, font);
		missionText.gameObject.SetActive(false);
		yield return new WaitForSeconds(textDuration);
	}
}
