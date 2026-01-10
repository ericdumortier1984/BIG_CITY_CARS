using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IceIceCreamBaby : MonoBehaviour
{
	[Header("UI")]
	[SerializeField] private float textDuration;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private GameObject instructionPanel;
	[SerializeField] private GameObject ingredientsPanel;
	[SerializeField] private RouteDrawer routeDrawer;

	[Header("Sliders")]
	[SerializeField] private Slider heatSlider;
	[SerializeField] private Slider fabricationSlider;

	[Header("Ingredients")]
	[SerializeField] private List<Toggle> ingredientsCheckboxes;
	[SerializeField] private List<GameObject> ingredientPrefabs;
	[SerializeField] private List<GameObject> iceCreamPrefabs;
	[SerializeField] private List<Transform> ingredientsLocations;
	[SerializeField] private Transform fabricationSpot;

	[Header("Deliver Ice Cream")]
	[SerializeField] private List<Transform> deliveryLocations;

	[Header("Heat Settings")]
	[SerializeField] private float heatIncreaseRate;
	[SerializeField] private float maxHeat = 40f;

	[Header("Fabrication Settings")]
	[SerializeField] private float fabricationRate;

	[Header("References")]
	[SerializeField] private MissionManager missionManager;
	[SerializeField] private Rigidbody playerRigidbody;

	[Header("Mission Cameras")]
	[SerializeField] private Camera mainCamera;
	[SerializeField] private Camera introCamera;
	[SerializeField] private Camera deliverCamera;
	[SerializeField] private float cameraShowTime;

	// BOOL
	private bool sliderOn = false;
	private bool isCollectingMaterials = false;
	private bool canFabricate = false;
	private bool isFabricating = false;
	private bool canDeliver = false;
	private bool hasLost = false;
	private bool objetiveCameraPlayed = false;

	// INT 
	private int materialsIndex = 0;
	private int totalMaterials = 4;
	private int iceCreamIndex = 0;
	private int totalIceCream = 4;

	// MEDAL
	private static bool isMedal = false;

	public void BeginIceIceCreamBaby()
	{
		SetElements();
	}

	private void SetElements()
	{
		// INSTRUCTIONS
		objetiveCameraPlayed = true;
		StartCoroutine(ShowIntroCamera());
		UIMissionManager.Instance.ShowMissionText("LOOK FOR ALL ICE CREAM INGREDIENTS", textDuration, 40);
		instructionPanel.SetActive(true);
		ingredientsPanel.SetActive(true);

		// COUNTER
		UIMissionManager.Instance.SetIceCreamCounter(iceCreamIndex, totalIceCream);
		StartCollectMaterials();

		// ROUTE DRAWER
		routeDrawer.gameObject.SetActive(true);
		routeDrawer.SetTarget(ingredientsLocations[0]);

		// INGREDIENTS LOCATION
		ingredientsLocations[0].gameObject.SetActive(true);

		// SLIDERS
		sliderOn = true;
		heatSlider.value = 20f;
		fabricationSlider.value = 0f;
		heatSlider.gameObject.SetActive(true);
	}

	private void Update()
	{
		UpdateHeatSlider();
		UpdateFabricationSlider();
	}

	private void StartCollectMaterials()
	{
		isCollectingMaterials = true;
		UIMissionManager.Instance.SetMaterialIceCreamCounter(materialsIndex, totalMaterials);
	}

	public void CollectMaterial()
	{
		if (!isCollectingMaterials || hasLost) { return; }

		materialsIndex++;
		UIMissionManager.Instance.SetMaterialIceCreamCounter(materialsIndex, totalMaterials);

		ingredientsLocations[materialsIndex - 1].gameObject.SetActive(false);

		if (materialsIndex < ingredientsLocations.Count)
		{
			ingredientsCheckboxes[materialsIndex - 1].isOn = true;
			ingredientsCheckboxes[materialsIndex - 1].image.color = Color.green;

			ingredientsLocations[materialsIndex].gameObject.SetActive(true);
			routeDrawer.SetTarget(ingredientsLocations[materialsIndex]);
		}

		if (materialsIndex >= ingredientsLocations.Count)
		{
			isCollectingMaterials = false;
			ingredientsPanel.SetActive(false);
			GoToIceCreamShop();
		}
	}

	private void GoToIceCreamShop()
	{
		canFabricate = true;
		UIMissionManager.Instance.ShowMissionText("GO TO THE ICE CREAM SHOP", textDuration, 40);
		fabricationSpot.gameObject.SetActive(true);
		routeDrawer.SetTarget(fabricationSpot);
	}

	public void BeginFabricateIceCream()
	{
		if (!canFabricate || isFabricating) { return; }

		isFabricating = true;
		fabricationSlider.gameObject.SetActive(true);

		playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
	}

	public void StopFabricateIceCream()
	{
		isFabricating = false;

		playerRigidbody.constraints = RigidbodyConstraints.None;
	}

	private void UpdateHeatSlider()
	{
		if (!sliderOn || hasLost) { return; }

		heatSlider.value += heatIncreaseRate * Time.deltaTime;

		if (heatSlider.value >= maxHeat)
		{
			LoseMission();
		}
	}

	private void UpdateFabricationSlider()
	{
		if (!isFabricating) { return; }
		if (!sliderOn || hasLost) { return; }

		fabricationSlider.value += fabricationRate * Time.deltaTime;

		if (fabricationSlider.value >= fabricationSlider.maxValue)
		{
			StopUpdateFabricationSlider();
			fabricationSlider.gameObject.SetActive(false);
			fabricationSpot.gameObject.SetActive(false);
		}
	}

	private void StopUpdateFabricationSlider()
	{
		isFabricating = false;
		canFabricate = false;
		canDeliver = true;

		foreach (GameObject iceCreamPrefab in iceCreamPrefabs)
		{
			iceCreamPrefab.SetActive(true);
		}

		if (deliveryLocations.Count > 0)
		{
			deliveryLocations[0].gameObject.SetActive(true);
			routeDrawer.SetTarget(deliveryLocations[0]);
		}

		StartCoroutine(ShowDeliverCamera());
		UIMissionManager.Instance.ShowMissionText("DELIVER THAT ICE CREAM", textDuration, 40);
		UIMissionManager.Instance.SetIceCreamCounter(iceCreamIndex, totalIceCream);
	}

	public void DeliverIceCream()
	{
		if (!canDeliver || hasLost) { return; }

		iceCreamIndex++;
		UIMissionManager.Instance.SetIceCreamCounter(iceCreamIndex, totalIceCream);

		deliveryLocations[iceCreamIndex - 1].gameObject.SetActive(false);

		if (iceCreamIndex < deliveryLocations.Count)
		{
			deliveryLocations[iceCreamIndex].gameObject.SetActive(true);
			routeDrawer.SetTarget(deliveryLocations[iceCreamIndex]);
		}

		if (iceCreamIndex >= deliveryLocations.Count)
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
			saveData.missionCompleted[25] = true;
			SaveSystem.SaveGame(saveData);

			isMedal = true;
		}
	}

	private void LoseMission()
	{
		hasLost = true;
		missionManager.EndMission();
		StartCoroutine(ShowLoseMessage());
		DisableElements();
	}

	private void DisableElements()
	{
		sliderOn = false;
		instructionPanel.SetActive(false);
		heatSlider.gameObject.SetActive(false);
		routeDrawer.gameObject.SetActive(false);
		UIMissionManager.Instance.HideCounter();

		foreach (Transform ingredientLocation in ingredientsLocations)
		{
			ingredientLocation.gameObject.SetActive(false);
		}

		foreach (Transform delLocation in deliveryLocations)
		{
			delLocation.gameObject.SetActive(false);
		}
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("THAT IS REFRESHING! \n + 15 COINS", textDuration, 50);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowLoseMessage()
	{
		UIMissionManager.Instance.ShowMissionText("ICE ICE CREAM BABY LOSE!", textDuration, 50);
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

	private IEnumerator ShowDeliverCamera()
	{
		playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
		mainCamera.enabled = false;
		deliverCamera.enabled = true;

		yield return new WaitForSeconds(cameraShowTime);

		playerRigidbody.constraints = RigidbodyConstraints.None;
		deliverCamera.enabled = false;
		mainCamera.enabled = true;
	}
}
