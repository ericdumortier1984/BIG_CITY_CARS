using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SchoolSucks : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private float textDuration;
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private GameObject instructionPanel;
	[SerializeField] private Slider bookSlider;
	[SerializeField] private float bookDecreaseRate;
    [SerializeField] private RouteDrawer routeDrawer;

    [Header("References")]
    [SerializeField] private MissionManager missionManager;
	[SerializeField] private List<Transform> busStops;
	[SerializeField] private Transform schoolPosition;
	[SerializeField] private List<GameObject> schoolBoys;
	[SerializeField] private GameObject bookPrefab;
	[SerializeField] private Rigidbody playerRigidbody;

	[Header("Throw Book Settings")]
	[SerializeField] private List<Transform> busSeats;
	[SerializeField] private float throwInterval;
	[SerializeField] private float thrownForce;

	[Header("Intro Camera")]
	[SerializeField] private Camera mainCamera;
	[SerializeField] private Camera introCamera;
	[SerializeField] private float cameraShowTime;

	// BOOL
	private bool hasLost = false;
	private bool sliderOn = false;
	private bool allPickedUp = false;
	private bool objetiveCameraPlayed = false;

	// INT 
	private int busStopIndex = 0;
	private int totalBusStop = 12;

	// FLOAT
	private float currentSliderValue = 1f;
	private float maxSliderValue = 0f;
	private float throwTimer = 0f;

	// LIST
	private List<Transform> bookThrowPos = new List<Transform>();

	// MEDAL
	private static bool isMedal = false;

    public void BeginSchoolSucks()
    {
        SetElements();
    }

    private void SetElements()
    {
		// INSTRUCTIONS
		objetiveCameraPlayed = true;
		StartCoroutine(ShowIntroCamera());
		UIMissionManager.Instance.ShowMissionText("PICKUP ALL SCHOOL BOYS", textDuration, 40);
		instructionPanel.SetActive(true);

		// BUS STOPS
		foreach (Transform busStop in busStops)
		{
			busStop.gameObject.SetActive(true);
		}

		// SCHOOL BOYS
		foreach (GameObject schoolBoy in schoolBoys)
		{
			schoolBoy.gameObject.SetActive(true);
		}

		// COUNTER
		UIMissionManager.Instance.SetBusStopCounter(busStopIndex, totalBusStop);

		// SLIDER
		bookSlider.gameObject.SetActive(true);
		sliderOn = true;

		// ROUTE DRAWER
		routeDrawer.gameObject.SetActive(true);
		routeDrawer.SetTarget(busStops[0]);
	}

	private void Update()
	{
		ThrowBooks();
	}

	public void PickUpPassenger(PickUpSchoolBoy schoolBoy)
	{
		if (busStopIndex == 0)
		{
			UIMissionManager.Instance.ShowMissionText("DONT LET THROW AWAY ALL BOOKS", textDuration, 40);
		}
		SchoolBoySeatPos();
		busStops[busStopIndex].gameObject.SetActive(false);
		busStopIndex++;
		UIMissionManager.Instance.SetBusStopCounter(busStopIndex, totalBusStop);

		if (busStopIndex < busStops.Count)
		{
			routeDrawer.SetTarget(busStops[busStopIndex]);
		}
		else
		{
			UIMissionManager.Instance.ShowMissionText("GO AHEAD TO SCHOOL", textDuration, 40);
			allPickedUp = true;
			schoolPosition.gameObject.SetActive(true);
			routeDrawer.SetTarget(schoolPosition);
		}

	}

	public void BusArrive()
	{
		WinMission();
	}

	private void SchoolBoySeatPos()
	{
		int seatIndex = busStopIndex;
		GameObject boy = schoolBoys[seatIndex];
		boy.transform.SetParent(busSeats[seatIndex], false);
		boy.transform.SetParent(busSeats[seatIndex], false);
		boy.transform.localPosition = Vector3.zero;
		boy.transform.localRotation = Quaternion.identity;

		if (!bookThrowPos.Contains(busSeats[seatIndex]))
		{
			bookThrowPos.Add(busSeats[seatIndex]);
		}
	}

	private void ThrowBooks()
	{
		if (!sliderOn || hasLost) { return; }

		throwTimer += Time.deltaTime;

		if (throwTimer >= throwInterval)
		{
			throwTimer = 0f;
			RandomSeat();
		}
	}

	private void RandomSeat()
	{
		int randomSeatIndex = Random.Range(0, bookThrowPos.Count);
		Transform randomSeat = bookThrowPos[randomSeatIndex];

		GameObject book = Instantiate(bookPrefab, randomSeat.position, Quaternion.identity);
		Rigidbody bookRb = book.GetComponent<Rigidbody>();

		if (bookRb != null)
		{
			int randomSide = Random.value > 0.5f ? 1 : -1;
			Vector3 throwDirection = (transform.right * randomSide + Vector3.up).normalized;
			bookRb.AddForce(throwDirection * thrownForce, ForceMode.Impulse);

			Destroy(book, 2f);
		}

		currentSliderValue -= bookDecreaseRate;
		currentSliderValue = Mathf.Clamp01(currentSliderValue);
		bookSlider.value = currentSliderValue;

		if (currentSliderValue <= 0)
		{
			LoseMission();
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
			saveData.missionCompleted[22] = true;
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
		sliderOn = false;
		instructionPanel.SetActive(false);
		routeDrawer.gameObject.SetActive(false);
		bookSlider.gameObject.SetActive(false);
		UIMissionManager.Instance.HideCounter();
		schoolPosition.gameObject.SetActive(false);

		foreach (Transform busStop in busStops)
		{
			busStop.gameObject.SetActive(false);
		}

		foreach (GameObject schoolBoy in schoolBoys)
		{
			schoolBoy.gameObject.SetActive(false);
		}
	}

    private IEnumerator ShowWinMessage()
    {
		UIMissionManager.Instance.ShowMissionText("GO STUDY, CHILDRENS! \n + 15 COINS", textDuration, 50);
		yield return new WaitForSeconds(textDuration);
	}

    private IEnumerator ShowLoseMessage()
    {
		UIMissionManager.Instance.ShowMissionText("THERE IS NO MORE BOOKS!", textDuration, 50);
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
