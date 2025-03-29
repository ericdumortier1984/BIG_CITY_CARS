using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    //public int mScore;
	public GameObject mPauseMenu;
	public GameObject mInputCanvas;
	public GameObject mCarFuel;
	public GameObject mItemWaypointCollected;

	private bool isPaused = false;

	private CarFuelController mCarFuelController; // Referencia al script de combustible
	private ItemWaypointController mItemWaypointController;
	
	private void Start()
	{
		// Menu de pausa desactivado
		if (mPauseMenu != null)
		{
			mPauseMenu.SetActive(false);
			mInputCanvas.SetActive(false);
		}

		mCarFuelController = FindObjectOfType<CarFuelController>();
		mItemWaypointController = FindObjectOfType<ItemWaypointController>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (isPaused)
			{
				ResumeGame();
			}
			else 
			{
				PauseGame();
			}
		}

		if (mCarFuelController.CurrentFuel == 0)
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
		Time.timeScale = 0;
		mInputCanvas.SetActive(true); // Canvas de Input activado
	}

	void EndGame(string mCondition)
	{
		switch (mCondition)
		{
			case "OutOfFuel":
				//PlayerPrefs.SetInt("SCORE", mScore);
				SceneManager.LoadScene("SceneEndGame");
				Debug.Log("Out of fuel, try again");
				break;

			case "AllWaypointsCollected":
				//PlayerPrefs.SetInt("SCORE", mScore);
				SceneManager.LoadScene("SceneEndGame");
				Debug.Log("All waypoints collected, excellent");
				break;
		}
	}
}
