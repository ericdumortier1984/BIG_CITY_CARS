using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Call911 : MonoBehaviour
{
	[Header("UI")]
	[SerializeField] private float textDuration;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private GameObject instructionPanel;
	[SerializeField] private RouteDrawer routeDrawer;

	[Header("References")]
	[SerializeField] private MissionManager missionManager;
	[SerializeField] private Transform hospitalTarget;
	[SerializeField] private List<GameObject> accidentsScenes;
	[SerializeField] private List<GameObject> accidentSceneTriggers;
	[SerializeField] private Rigidbody playerRigidbody;

	[Header("Intro Camera")]
	[SerializeField] private Camera mainCamera;
	[SerializeField] private Camera introCamera;
	[SerializeField] private float cameraShowTime;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip call911Music;
	[SerializeField] private AudioClip collectSFX;
	[SerializeField] private AudioClip winSFX;

	// BOOL
	private bool hasPatient = false;
	private bool objetiveCameraPlayed = false;

	// INT 
	private int accidentSceneTriggerIndex = 0;
	private int accidentSceneIndex = 0;
	private int totalAccidentScenes = 4;
	private int patientIndex = 0;
	private int patientToSafe = 8;

	// MEDAL
	private static bool isMedal = false;

	// MUSIC MISSION START
	private bool missionMusicStarted = false;

	public void BeginCall911()
	{
		SetElements();
	}

	private void SetElements()
	{
		// INSTRUCTIONS
		objetiveCameraPlayed = true;
		StartCoroutine(ShowIntroCamera());
		UIMissionManager.Instance.ShowMissionText("GET TO THE ACCIDENT SCENE", textDuration, 40);
		instructionPanel.SetActive(true);

		// MUSIC
		if (!missionMusicStarted)
		{
			AudioManager.Instance.PlayMissionMusic(call911Music);
			missionMusicStarted = true;
		}

		// COUNTER
		UIMissionManager.Instance.SetAccidentSceneCounter(accidentSceneIndex, totalAccidentScenes);

		// ACCIDENT SCENE
		accidentsScenes[0].SetActive(true);

		// ROUTE DRAWER
		routeDrawer.gameObject.SetActive(true);
		routeDrawer.SetTarget(accidentsScenes[0].transform);
	}


	public void TakePatient()
	{
		AudioManager.Instance.PlaySFX(collectSFX);
		hasPatient = true;
		accidentSceneTriggers[accidentSceneTriggerIndex].SetActive(false);
		accidentSceneTriggerIndex++;
		hospitalTarget.gameObject.SetActive(true);
		UIMissionManager.Instance.ShowMissionText("TAKE THOSE PATIENTS TO THE HOSPITAL", textDuration, 40);
		hospitalTarget.gameObject.SetActive(true);
		routeDrawer.SetTarget(hospitalTarget);

		
	}

	public void LeavePatient()
	{
		if (!hasPatient) { return; }

		AudioManager.Instance.PlaySFX(collectSFX);
		hasPatient = false;
		accidentsScenes[accidentSceneIndex].SetActive(false);
		patientToSafe--;
		accidentSceneIndex++;

		if (accidentSceneIndex >= accidentsScenes.Count)
		{
			WinMission();
		}
		else 
		{
			UIMissionManager.Instance.SetAccidentSceneCounter(accidentSceneIndex, totalAccidentScenes);
			accidentsScenes[accidentSceneIndex].SetActive(true);
			UIMissionManager.Instance.ShowMissionText("NEXT ACCIDENT SCENE", textDuration, 40);
			hospitalTarget.gameObject.SetActive(false);
			routeDrawer.SetTarget(accidentsScenes[accidentSceneIndex].transform);
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
			MainMenu.Instance.AddCoin(4500);
			LevelData.CoinsCollectedInLevel += 4500;

			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[24] = true;
			SaveSystem.SaveGame(saveData);

			isMedal = true;
		}
	}

	private void DisableElements()
	{
		hospitalTarget.gameObject.SetActive(false);
		instructionPanel.SetActive(false);
		routeDrawer.gameObject.SetActive(false);
		UIMissionManager.Instance.HideCounter();

		foreach (GameObject accidentScene in accidentsScenes)
		{
			accidentScene.SetActive(false);
		}
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("YOUR A GREAT AMBULANCE DRIVER! \n + 4500 COINS", textDuration, 50);
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
