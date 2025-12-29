using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FastFood : MonoBehaviour
{
	[Header("Food")]
	[SerializeField] private List<GameObject> foods;

	[Header("Cooking Mode")]
	[SerializeField] private CinemachineVirtualCamera coockingCamera;
	[SerializeField] private List<GameObject> hungryPeoples;
	[SerializeField] private List<Transform> hungryPeoplesPositions;
	[SerializeField] private ParticleSystem foodDeliveredParticle;
	[SerializeField] private Texture2D cookingCursor;

	[Header("UI")]
	[SerializeField] private float textDuration;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private GameObject instructionPanel;
	[SerializeField] private List<GameObject> cookingInstructionPanels;
	[SerializeField] private RouteDrawer routeDrawer;
	[SerializeField] private GameObject needle;
	[SerializeField] private GameObject minimap;
	[SerializeField] private Slider fuelBar;
	[SerializeField] private Image fuelIcon;

	[Header("References")]
	[SerializeField] private Rigidbody carRigidbody;
	[SerializeField] private MissionManager missionManager;
	[SerializeField] private Transform salesPointTrigger;
	[SerializeField] private List<HungryController> hungryController;

	// BOOL 
	private bool hasLost = false;
	private bool isCooking = false;

	// INT
	private int foodIndex = 0;
	private int totalFood = 4;
	private int currentHungryPeopleIndex = -1;

	// MEDAL
	private static bool isMedal = false;

	public bool IsCooking => isCooking;

	public void BeginFastFood()
    {
        SetElements();
    }

    private void SetElements()
    {
        // INSTRUCTIONS
        UIMissionManager.Instance.ShowMissionText("GO TO THE STADIUM TO SALE SOME FOOD", textDuration, 40);
        instructionPanel.SetActive(true);

		// SALES POINT TRIGGER
		salesPointTrigger.gameObject.SetActive(true);

		// ROUTE DRAWER
		routeDrawer.gameObject.SetActive(true);
		routeDrawer.SetTarget(salesPointTrigger);
	}

    void Update()
    {
		OnMouseDown();
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
			saveData.missionCompleted[16] = true;
			SaveSystem.SaveGame(saveData);

			isMedal = true;
		}
	}

	public void LoseMission()
	{
		if (hasLost) { return; }
		
		hasLost = true;
		StartCoroutine(ShowLoseMessage());
		DisableElements();
		missionManager.EndMission();
	}

	public void EnterCookingMode(FoodSalePointTrigger salesTrigger)
	{
		if (isCooking) { return; }

		isCooking = true;
		carRigidbody.constraints = RigidbodyConstraints.FreezeAll;
		coockingCamera.Priority = 21;
		instructionPanel.SetActive(false);
		foreach (GameObject cookingInstructionPanel in cookingInstructionPanels)
		{
			cookingInstructionPanel.SetActive(true);
		}
		Cursor.SetCursor(cookingCursor, Vector2.zero, CursorMode.Auto);
		minimap.SetActive(false);
		needle.SetActive(false);
		fuelBar.gameObject.SetActive(false);
		fuelIcon.gameObject.SetActive(false);

		foreach (GameObject food in foods) 
		{
			food.SetActive(true);
		}

		StartHungryPeople();
	}

	public void ExitCoockingMode()
	{
		isCooking = false;
		coockingCamera.Priority = 19;
		carRigidbody.constraints = RigidbodyConstraints.None;
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		minimap.SetActive(true);
		needle.SetActive(true);
		fuelBar.gameObject.SetActive(true);
		fuelIcon.gameObject.SetActive(true);

		foreach (GameObject hungryPeople in hungryPeoples)
		{
			hungryPeople.SetActive(false);
		}
	}

	private void StartHungryPeople()
	{
		foreach (GameObject hungryPeople in hungryPeoples)
		{
			hungryPeople.SetActive(false);
		}

		currentHungryPeopleIndex = -1;
		NextHungryPeople();
	}

	public void NextHungryPeople()
	{
		currentHungryPeopleIndex++;

		if (currentHungryPeopleIndex >= hungryPeoples.Count)
		{
			WinMission();
		}

		if (currentHungryPeopleIndex > 0)
		{
			hungryPeoples[currentHungryPeopleIndex - 1].SetActive(false);
		}

		GameObject hungryPeople = hungryPeoples[currentHungryPeopleIndex];
		Transform hungryPeoplePos = hungryPeoplesPositions[currentHungryPeopleIndex];

		hungryPeople.transform.position = hungryPeoplePos.position;
		hungryPeople.transform.rotation = hungryPeoplePos.rotation;
		hungryPeople.SetActive(true);

		hungryController[currentHungryPeopleIndex].StartOrdering();
		Instantiate(foodDeliveredParticle, hungryPeoplePos.position, Quaternion.identity);
	}

	private void OnMouseDown()
	{
		if (isCooking)
		{

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			if (Input.GetMouseButtonDown(0))
			{
				GetFood();
			}
		}
	}

	private void GetFood()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit, 100f))
		{
			if (hit.collider.CompareTag("Food"))
			{
				FoodID food = hit.collider.GetComponent<FoodID>();

				if (food == null) { return; }

				HungryController currentHungryPeople = hungryController[currentHungryPeopleIndex];

				currentHungryPeople.ReceiveFood(food.ID);
			}
		}
	}

	private void DisableElements()
	{
		foreach (GameObject food in foods)
		{
			food.SetActive(false);
		}

		instructionPanel.SetActive(false);
		foreach (GameObject cookingInstructionPanel in cookingInstructionPanels)
		{
			cookingInstructionPanel.SetActive(false);
		}
		UIMissionManager.Instance.HideCounter();
		routeDrawer.gameObject.SetActive(false);

		ExitCoockingMode();
	}

	private IEnumerator ShowNextHungrypeople(GameObject hungryPeople)
	{
		hungryPeople.SetActive(false);

		yield return new WaitForSeconds(0.5f);

		NextHungryPeople();
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("THATS FAST! \n + 15 COINS", textDuration, 50);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowLoseMessage()
	{
		UIMissionManager.Instance.ShowMissionText("TIME UP!", textDuration, 50);
		yield return new WaitForSeconds(textDuration);
	}
}
