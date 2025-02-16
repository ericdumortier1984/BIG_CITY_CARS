using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public int mScore;

	private void Start()
	{
		StartCoroutine(mGameCountDown()); // Iniciar coroutine
	}

	IEnumerator mGameCountDown()
	{
		yield return new WaitForSeconds(60.0f); // Esperar 5 segundos

		EndGame(); // Mostrar puntaje
	}

	void EndGame()
    {
        PlayerPrefs.SetInt("SCORE", mScore);
        SceneManager.LoadScene("SceneEndGame");
    }
}
