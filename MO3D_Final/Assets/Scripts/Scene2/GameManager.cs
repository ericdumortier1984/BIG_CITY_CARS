using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
	[Header("Canvas")]
	[SerializeField] private GameObject mPauseMenu;
	[SerializeField] private GameObject mInputCanvas;

	private GameObject mCarFuel;
	private GameObject mItemWaypointCollected;

	private bool isPaused = false;
	private bool isInputCanvas = false;

	private CarFuelController mCarFuelController; 
	private ItemWaypointController mItemWaypointController;

	private void Start()
	{
		mPauseMenu.SetActive(false);
		mInputCanvas.SetActive(false);
		Cursor.visible = false;

		mCarFuelController = FindObjectOfType<CarFuelController>();
		mItemWaypointController = FindObjectOfType<ItemWaypointController>();

		ResetValues();
	}

	private void Update()
	{

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (isPaused)
			{
				ResumeGame();
			}
			if (isInputCanvas)
			{
				Cursor.visible = true;
				PauseGame(); 
				
			}
			else
			{
				Cursor.visible = true;
				PauseGame();
			}
		}

		if (mCarFuelController != null && mCarFuelController.CurrentFuel <= 0.01f)

		{
			EndGame("OutOfFuel");
		}
		if (mItemWaypointController.ItemWaypointCollected == 10)
		{
			EndGame("AllWaypointsCollected");
		}
	}

	private void ResetValues()
	{
		// Reinicia los valores de LevelData al inicio del nivel
		LevelData.CoinsCollectedInLevel = 0;
		LevelData.WaypointsCollectedInLevel = 0;
		LevelData.MedalCollectedInLevel = 0;
	}

	public void ResumeGame()
	{
		isPaused = false;
		Time.timeScale = 1; 
		mPauseMenu.SetActive(false); 
		mInputCanvas.SetActive(false); 
	}

	public void PauseGame()
	{
		isPaused = true;
		Time.timeScale = 0; 
		mPauseMenu.SetActive(true); 
	}

	public void GoToMainMenu()
	{
		Time.timeScale = 1;
		LoaderScene.Load(LoaderScene.mScene.SceneMainMenu); 
	}

	public void RestartGame()
	{
		Time.timeScale = 1;
		LoaderScene.Load(LoaderScene.mScene.SceneGameTpFinal); 
	}

	public void ShowGameInput()
	{
		isInputCanvas = true;
		Time.timeScale = 0;
		mInputCanvas.SetActive(true); 
	}
	
	void EndGame(string mCondition)
	{
		switch (mCondition)
		{
			case "OutOfFuel":
				SceneManager.LoadScene("SceneEndGame");
				Debug.Log("Out of fuel");
				break;

			case "AllWaypointsCollected":
				SceneManager.LoadScene("SceneEndGame");
				Debug.Log("All waypoints collected");
				break;
		}
	}
}
