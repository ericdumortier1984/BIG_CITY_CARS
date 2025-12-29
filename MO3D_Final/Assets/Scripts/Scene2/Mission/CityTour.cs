using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CityTour : MonoBehaviour
{
	[Header("UI")]
	[SerializeField] private float textDuration;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private GameObject instructionPanel;
	[SerializeField] private GameObject targetPanel;
	[SerializeField] private List<Toggle> targetCheckboxes;
	[SerializeField] private RouteDrawer routeDrawer;
	[SerializeField] private Image photoFrame;

	[Header("References")]
	[SerializeField] private MissionManager missionManager;
	[SerializeField] private Rigidbody playerRigidbody;
	[SerializeField] private List<Transform> photoSpots;

	[Header("Tourists")]
	[SerializeField] private List<TouristController> tourists;

	[Header("Photo Mode")]
	[SerializeField] private Camera mainCamera;
	
	// BOOL
	private bool hasLost = false;
	private bool isPhotoMode = false;
	private bool photoTaken = false;

	// INT 
	private int photosIndex = 0;
	private int totalPhotos = 5;

	// CAMERA
	private Camera currentPhotoCamera;
	private PhotoCameraController currentPhotoController;

	// MEDAL
	private static bool isMedal = false;

	public void BeginCityTour()
	{
		SetElements();
	}

	private void SetElements()
	{
		// INSTRUCTIONS
		UIMissionManager.Instance.ShowMissionText("LOOK FOR PHOTO SPOTS", textDuration, 40);
		instructionPanel.SetActive(true);
		targetPanel.SetActive(true);

		// TOURISTS
		SetTourist();

		// PHOTO SPOT
		for (int i = 0; i < photoSpots.Count; i++)
		{
			photoSpots[i].gameObject.SetActive(i == 0);
		}

		// COUNTER
		UIMissionManager.Instance.SetPhotoCounter(photosIndex, totalPhotos);

		//ROUTE DRAWER
		routeDrawer.gameObject.SetActive(true);
		routeDrawer.SetTarget(photoSpots[photosIndex]);
	}

	private void Update()
	{
		
	}

	private void SetTourist()
	{
		foreach (TouristController tourist in tourists)
		{
			tourist.SitTourist();
		}
	}

	public void EnterPhotoMode(TakePhotoSpot photoSpot)
	{
		StartCoroutine(StartPhotoMode(photoSpot));
	}

	private void ExitPhotomode()
	{
		playerRigidbody.constraints = RigidbodyConstraints.None;
		mainCamera.enabled = true;
		currentPhotoCamera.enabled = false;
		photoFrame.gameObject.SetActive(false);
		currentPhotoController.EnableControl(false);
	}

	private bool IsTargetInPhoto(Transform target, Camera photoCamera)
	{
		Ray ray = photoCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 100f))
		{
			return hit.transform == target;
		}

		return false;
	}

	public void PhotoTaken(TakePhotoSpot photoSpot)
	{
		photoSpot.DisableSpot();

		photosIndex++;
		UIMissionManager.Instance.SetPhotoCounter(photosIndex, totalPhotos);

		if (photosIndex < photoSpots.Count)
		{
			targetCheckboxes[photosIndex - 1].isOn = true;
			targetCheckboxes[photosIndex - 1].image.color = Color.green;

			photoSpots[photosIndex].gameObject.SetActive(true);
			routeDrawer.SetTarget(photoSpots[photosIndex]);
		}
		else
		{
			WinMission();
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
			saveData.missionCompleted[23] = true;
			SaveSystem.SaveGame(saveData);

			isMedal = true;
		}
	}

	private void LoseMission()
	{
		if (hasLost) { return; }

		hasLost = true;
		StartCoroutine(ShowLoseMessage());
		DisableElements();
		missionManager.EndMission();
	}

	private void DisableElements()
	{
		instructionPanel.SetActive(false);
		targetPanel.SetActive(false);
		routeDrawer.gameObject.SetActive(false);
		UIMissionManager.Instance.HideCounter();

		foreach (Transform photoSpot in photoSpots)
		{
			photoSpot.gameObject.SetActive(false);
		}
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("NICE PHOTOS! \n + 15 COINS", textDuration, 50);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowLoseMessage()
	{
		UIMissionManager.Instance.ShowMissionText("OUT OF FUEL!", textDuration, 50);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator StartPhotoMode(TakePhotoSpot photoSpot)
	{
		photoTaken = false;
		isPhotoMode = true;

		playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
		mainCamera.enabled = false;
		currentPhotoCamera = photoSpot.GetPhotoCamera();
		currentPhotoCamera.enabled = true;
		currentPhotoController = currentPhotoCamera.GetComponent<PhotoCameraController>();
		currentPhotoController.EnableControl(true);
		photoFrame.gameObject.SetActive(true);
		UIMissionManager.Instance.ShowMissionText("FOCUS AND PRESS SPACE BAR", textDuration, 40);

		while (!photoTaken)
		{
			bool targetInFocus = IsTargetInPhoto(photoSpot.GetPhotoTarget(),currentPhotoCamera);
			Color targetFrameColor = targetInFocus ? Color.green : Color.black;
			targetFrameColor.a = 0.6f;
			photoFrame.color = targetFrameColor;

			// FOTO
			if (targetInFocus && Input.GetKeyDown(KeyCode.Space))
			{
				photoTaken = true;
			}

			yield return null;
		}

		ExitPhotomode();
		PhotoTaken(photoSpot);
	}
}
