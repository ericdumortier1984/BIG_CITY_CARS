using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
	public GameObject mPauseMenu;
	public GameObject mInputCanvas;
	public GameObject mCarFuel;
	public GameObject mItemWaypointCollected;

	private bool isPaused = false;
	private bool isInputCanvas = false;

	private CarFuelController mCarFuelController; 
	private ItemWaypointController mItemWaypointController;

	private void Start()
	{
		// Menu de pausa desactivado
		if (mPauseMenu != null)
		{
			mPauseMenu.SetActive(false);
			mInputCanvas.SetActive(false);
			Cursor.visible = false;
		}

		mCarFuelController = FindObjectOfType<CarFuelController>();
		mItemWaypointController = FindObjectOfType<ItemWaypointController>();

		// Reinicia los valores de LevelData al inicio del nivel
		LevelData.CoinsCollectedInLevel = 0;
		LevelData.WaypointsCollectedInLevel = 0;
		LevelData.MedalCollectedInLevel = 0;
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
				PauseGame(); // Vuelvo al menu de pausa luego del menu de input
				
			}
			else
			{
				Cursor.visible = true;
				PauseGame();
			}
		}

		if (mCarFuelController != null && mCarFuelController.CurrentFuel == 0)
		{
			EndGame("OutOfFuel");
		}
		if (mItemWaypointController.ItemWaypointCollected == 10)
		{
			EndGame("AllWaypointsCollected");
		}
	}

	public void ResumeGame()
	{
		isPaused = false;
		Time.timeScale = 1; // Tiempo corre normal
		mPauseMenu.SetActive(false); // Menu de pausa desactivado
		mInputCanvas.SetActive(false); // Canvas de Input desactivado
	}

	public void PauseGame()
	{
		isPaused = true;
		Time.timeScale = 0; // Tiempo detenido
		mPauseMenu.SetActive(true); // Menu de pausa activado
	}

	public void GoToMainMenu()
	{
		Time.timeScale = 1;
		LoaderScene.Load(LoaderScene.mScene.SceneMainMenu); // Cambio de escena a MainMenu
	}

	public void RestartGame()
	{
		Time.timeScale = 1;
		LoaderScene.Load(LoaderScene.mScene.SceneGameTpFinal); // Reinicio de SceneGameTpFinal
	}

	public void ShowGameInput()
	{
		isInputCanvas = true;
		Time.timeScale = 0;
		mInputCanvas.SetActive(true); // Canvas de Input activado
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
