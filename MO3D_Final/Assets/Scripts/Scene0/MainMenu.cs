using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public static MainMenu Instance { get; private set; } // Instancia Singleton
	public TMPro.TextMeshProUGUI mCoinText; // Referencia al texto
	private int mCoin;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;

		// Monedas por defecto
		mCoin = PlayerPrefs.GetInt("Coin", 0);
		UpdateCoinUI();

		// Mostrar el cursor
		Cursor.visible = true; 
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			AddCoin(15000);
			
		}

		if ( Input.GetKeyDown(KeyCode.J))
		{
			SpendCoin(1);
			
		}
	}

	public void AddCoin(int mCoinAmount)
	{
		mCoin += mCoinAmount;
		PlayerPrefs.SetInt("Coin", mCoin);
		PlayerPrefs.Save();
		UpdateCoinUI();
	}

	public bool SpendCoin(int mCoinAmount)
	{
		if (mCoin >= mCoinAmount)
		{
			mCoin -= mCoinAmount;
			PlayerPrefs.SetInt("Coin", mCoin);
			PlayerPrefs.Save();
			UpdateCoinUI();
			return true;
		}
		else 
		{
			Debug.Log("Not enough coins");
			return false;
		}
	}

	private void UpdateCoinUI()
	{
		if (mCoinText != null)
		{
			mCoinText.text = "Coins: " + mCoin.ToString();
			Debug.Log("Coins: " + mCoin.ToString());
		}
	}

	public void StartGame()
    {
		// Carga escena de nivel si el auto seleccionado no está bloqueado
		if (CarSelectionController.Instance.mIsSelectedCarBloqued == true)
		{
			Debug.Log("Car is Bloqued");
			return;
		}
		else
		{
			LoaderScene.Load(LoaderScene.mScene.SceneGameTpFinal);
		}
    }

    public void ShowSettings()
    {
        LoaderScene.Load(LoaderScene.mScene.SceneSettings);
    }

    public void Showcredits()
    {
        LoaderScene.Load(LoaderScene.mScene.SceneGameCredits);
    }

    public void ShowMainMenu()
    {
        LoaderScene.Load(LoaderScene.mScene.SceneMainMenu);
    }
    
	public void LoadGame()
	{
		for (int i = 1; i < CarSelectionController.Instance.mCarsToSelect.Length; i++)
		{
			CarStats mCarStats = CarSelectionController.Instance.mCarsToSelect[i].GetComponent<CarStats>();
			
			if (PlayerPrefs.HasKey($"CarUnlocked{i}"))
			{
				mCarStats.IsCarBloqued = PlayerPrefs.GetInt($"CarUnlocked{i}") == 0;
			}
			else
			{
				mCarStats.IsCarBloqued = true;
			}
		}
		Debug.Log("Car Data Loaded");
	}

	public void SaveGame()
	{
		for (int i = 0; i < CarSelectionController.Instance.mCarsToSelect.Length; i++)
		{
			CarStats mCarStats = CarSelectionController.Instance.mCarsToSelect[i].GetComponent<CarStats>();
			PlayerPrefs.SetInt($"CarUnlocked{i}", mCarStats.IsCarBloqued ? 0 : 1);
		}
		PlayerPrefs.Save();
		Debug.Log("Car Data Saved");
	}

	public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
