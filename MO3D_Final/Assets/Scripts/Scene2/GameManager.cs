using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public int mScore;
	public GameObject mPauseMenu;

	private bool isPaused = false;

	private void Start()
	{
		// Menu de pausa desactivado
		if (mPauseMenu != null)
		{
			mPauseMenu.SetActive(false);
		}

		StartCoroutine(mGameCountDown()); // Iniciar coroutine
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
	}

	public void ResumeGame()
	{
		isPaused = false;
		Time.timeScale = 1; // Tiempo corre normal
		mPauseMenu.SetActive(false); // Menu de pausa desactivado
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
		LoaderScene.Load(LoaderScene.mScene.SceneMainMenu);
	}

	IEnumerator mGameCountDown()
	{
		yield return new WaitForSeconds(180.0f); // Esperar 5 segundos

		EndGame(); // Mostrar puntaje
	}

	void EndGame()
    {
        PlayerPrefs.SetInt("SCORE", mScore);
        SceneManager.LoadScene("SceneEndGame");
    }
}
