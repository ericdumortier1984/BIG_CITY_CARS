using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HeavyLoad : MonoBehaviour
{
    [Header("UI")]
	[SerializeField] private TMP_FontAsset font;
	[SerializeField] private float textDuration;
    [SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private GameObject materialPanel;
	[SerializeField] private GameObject housePanel;
	[SerializeField] private GameObject timePanel;
 	[SerializeField] private List<Toggle> materialCheckboxes;
	[SerializeField] private List<Toggle> houseCheckboxes;

    [Header("References")]
    [SerializeField] private MissionManager missionManager;
	[SerializeField] private SpeedTruckController speedTruckController;
	[SerializeField] private Transform spawnMaterialPoint;
	[SerializeField] private Transform deliveryMaterialPoint;
	[SerializeField] private GameObject introSequence;
	[SerializeField] private Rigidbody playerRigidbody;

	[Header("House and Materials")]
	[SerializeField] private List<GameObject> constructionMaterials;
	[SerializeField] private List<GameObject> houseStructure;

	[Header("Time")]
	[SerializeField] CountdownTimer countdownTimer;

	[Header("Farm Camera")]
	[SerializeField] private Camera mainCamera;
	[SerializeField] private Camera deliveryPointCamera;
	[SerializeField] private float cameraShowTime;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip heavyLoadMusic;
	[SerializeField] private AudioClip collectSFX;
	[SerializeField] private AudioClip winSFX;
	[SerializeField] private AudioClip loseSFX;

	// BOOL
	private bool hasLost = false;
	private bool hasMaterial = false;
	private bool objectiveCameraPlayed = false;

	// INT
	private int materialIndex = 0;
	private int totalMaterials = 4;

	// MEDAL
	private static bool isMedal = false;

	// MUSIC MISSION START
	private bool missionMusicStarted = false;

	public void BeginHeavyLoad()
    {
        SetElements();
    }

    private void SetElements()
    {
        // INSTRUCTIONS
		introSequence.SetActive(true);
		UIMissionManager.Instance.ShowMissionText("LOAD IN THIS POINT", textDuration, 50, font);

		// MUSIC
		if (!missionMusicStarted)
		{
			AudioManager.Instance.PlayMissionMusic(heavyLoadMusic);
			missionMusicStarted = true;
		}

		// PANEL AND TOGGLES
		materialPanel.SetActive(true);
		housePanel.SetActive(true);

		// SPAWN AND DELIVERY POINTS
		spawnMaterialPoint.gameObject.SetActive(true);
		deliveryMaterialPoint.gameObject.SetActive(true);

		// TIME
		timePanel.SetActive(true);
		countdownTimer.StartTimer();
	}

	private void Update()
	{
		LoseMission();
	}

	public void SpawnMaterial()
	{
		if (hasMaterial){ return; }

		hasMaterial = true;
		constructionMaterials[materialIndex].SetActive(true);

		if (materialIndex == 0)
		{
			objectiveCameraPlayed = true;
			StartCoroutine(ShowDeliveryPointCamera());

			UIMissionManager.Instance.ShowMissionText("THE UNLOAD POINT IS IN THE FARM", textDuration, 50, font);
		}

		if (materialIndex < materialCheckboxes.Count)
		{
			materialCheckboxes[materialIndex].isOn = true;
			materialCheckboxes[materialIndex].image.color = Color.green;
		}

		speedTruckController.SetLoaded(true);
	}

	public void DeliveryMaterial()
	{
		if (!hasMaterial) { return; }

		AudioManager.Instance.PlaySFX(collectSFX);

		constructionMaterials[materialIndex].SetActive(false);
		houseStructure[materialIndex].SetActive(true);

		if (materialIndex < materialCheckboxes.Count)
		{
			houseCheckboxes[materialIndex].isOn = true;
			houseCheckboxes[materialIndex].image.color = Color.green;
		}

		speedTruckController.SetLoaded(false);

		materialIndex++;
		hasMaterial = false;

		if (materialIndex >= constructionMaterials.Count)
		{
			WinMission();
		}
		else 
		{
			UIMissionManager.Instance.ShowMissionText("RETURN FOR NEXT LOAD", textDuration, 50, font);
		}
	}

    private void WinMission()
    {
		AudioManager.Instance.PlaySFX(loseSFX);
		AudioManager.Instance.PlayGameplayMusic();
		missionMusicStarted = false;

		missionManager.EndMission();
		StartCoroutine(ShowWinMessage());
		DisableElements();

		if (!isMedal)
		{
			MainMenu.Instance.AddMedal(1);
			LevelData.MedalCollectedInLevel += 1;
			MainMenu.Instance.AddCoin(8000);
			LevelData.CoinsCollectedInLevel += 8000;

			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[20] = true;
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
		materialPanel.SetActive(false);
		housePanel.SetActive(false);
		timePanel.SetActive(false);
		spawnMaterialPoint.gameObject.SetActive(false);
		deliveryMaterialPoint.gameObject.SetActive(false);
		countdownTimer.StopTimer();
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("CONSTRUCTION COMPLETE! + 8000 COINS", textDuration, 80, font);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowLoseMessage()
	{
		UIMissionManager.Instance.ShowMissionText("TOO SLOW!", textDuration, 80, font);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowDeliveryPointCamera()
	{
		playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
		mainCamera.enabled = false;
		deliveryPointCamera.enabled = true;

		yield return new WaitForSeconds(cameraShowTime);

		playerRigidbody.constraints = RigidbodyConstraints.None;
		deliveryPointCamera.enabled = false;
		mainCamera.enabled = true;
	}
}
