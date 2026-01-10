using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EndGameReason
{
	OutOfFuel,
	AllWaypointsCollected
}

public class GameManager : MonoBehaviour
{
	[Header("UI")]
	[SerializeField] private GameObject pauseMenu;
	[SerializeField] private GameObject inputCanvas;
	[SerializeField] private List<GameObject> hudGameObjects;

	private CarFuelController carFuelController;
	private ItemWaypointController itemWaypointController;

	private bool isPaused;
	private bool isInputOpen;

	private void Start()
	{
		InitializeUI();
		ResetLevelValues();

		carFuelController = FindObjectOfType<CarFuelController>();
		itemWaypointController = FindObjectOfType<ItemWaypointController>();
	}

	private void Update()
	{
		HandlePauseInput();
		CheckGameConditions();
	}

	private void InitializeUI()
	{
		pauseMenu.SetActive(false);
		inputCanvas.SetActive(false);
		Cursor.visible = false;
		Time.timeScale = 1f;
	}

	private void ResetLevelValues()
	{
		LevelData.CoinsCollectedInLevel = 0;
		LevelData.WaypointsCollectedInLevel = 0;
		LevelData.MedalCollectedInLevel = 0;
	}

	private void HandlePauseInput()
	{
		if (!Input.GetKeyDown(KeyCode.Escape)) return;

		if (isPaused)
			ResumeGame();
		else
			PauseGame();
	}

	public void PauseGame()
	{
		isPaused = true;
		Time.timeScale = 0f;
		Cursor.visible = true;
		pauseMenu.SetActive(true);

		foreach (GameObject hudGameObject in hudGameObjects)
		{
			hudGameObject.SetActive(false);
		}

		AudioManager.Instance.SetPaused(true);
	}

	public void ResumeGame()
	{
		isPaused = false;
		Time.timeScale = 1f;
		Cursor.visible = false;
		pauseMenu.SetActive(false);
		inputCanvas.SetActive(false);

		foreach (GameObject hudGameObject in hudGameObjects)
		{
			hudGameObject.SetActive(true);
		}

		AudioManager.Instance.SetPaused(false);
	}

	private void CheckGameConditions()
	{
		if (carFuelController != null && carFuelController.CurrentFuel <= 0.01f)
		{
			EndGame(EndGameReason.OutOfFuel);
		}

		if (itemWaypointController != null &&
			itemWaypointController.ItemWaypointCollected >= 10)
		{
			EndGame(EndGameReason.AllWaypointsCollected);
		}
	}

	private void EndGame(EndGameReason reason)
	{
		SceneManager.LoadScene("SceneEndGame");

		switch (reason)
		{
			case EndGameReason.OutOfFuel:
				Debug.Log("Out of fuel");
				break;

			case EndGameReason.AllWaypointsCollected:
				Debug.Log("All waypoints collected");
				break;
		}
	}

	public void GoToMainMenu()
	{
		Time.timeScale = 1f;
		LoaderScene.Load(LoaderScene.mScene.SceneMainMenu);
	}

	public void RestartGame()
	{
		Time.timeScale = 1f;
		LoaderScene.Load(LoaderScene.mScene.SceneGameTpFinal);
	}

	public void ShowGameInput()
	{
		isInputOpen = true;
		Time.timeScale = 0f;
		inputCanvas.SetActive(true);
	}
}

