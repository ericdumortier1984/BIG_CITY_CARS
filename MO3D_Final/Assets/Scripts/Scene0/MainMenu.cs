using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public static MainMenu Instance { get; private set; } // Instancia Singleton
	public TMPro.TextMeshProUGUI mCoinText; // Referencia al texto
	//private int mCoin;

	private bool mIsGameLoaded = false;

	private SaveData mCurrentSave = new SaveData(); // Singleton de save data

	private void Awake()
	{
		/*
		if (PlayerPrefs.HasKey("Coin"))
		{
			PlayerPrefs.DeleteAll();
		}*/

		// Mostrar el cursor
		Cursor.visible = true;

		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;

		// PlayerPrefs.GetInt("Coin", mCoin);
		//mCoin = PlayerPrefs.GetInt("Coin", 0);

		// Cargar datos al iniciar
		///mCurrentSave = SaveSystem.LoadGame(); 
		mCurrentSave = new SaveData();

		mCurrentSave.mCoins = 0;

		mCoinText.text = "Coins: -"; // o simplemente dejalo vacío

		//UpdateCoinUI();
	}

	public void AddCoin(int mCoinAmount)
	{
		if (!mIsGameLoaded)
		{
			Debug.Log("Intent add coins");
			return;
		}

		//mCoin += mCoinAmount;
		mCurrentSave.mCoins += mCoinAmount;
		SaveSystem.SaveGame(mCurrentSave);
		//PlayerPrefs.SetInt("Coin", mCoin);
		//PlayerPrefs.Save();
		UpdateCoinUI();
	}

	public bool SpendCoin(int mCoinAmount)
	{

		if (!mIsGameLoaded)
		{
			Debug.LogWarning("Intent spend coins");
			return false;
		}

		//if (mCoin >= mCoinAmount)
		if (mCurrentSave.mCoins >= mCoinAmount)
		{
			//mCoin -= mCoinAmount;
			mCurrentSave.mCoins -= mCoinAmount;
			SaveSystem.SaveGame(mCurrentSave);
			//PlayerPrefs.SetInt("Coin", mCoin);
			//PlayerPrefs.Save();
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
			//mCoin = PlayerPrefs.GetInt("Coin", 0);
			//mCoinText.text = "Coins: " + mCoin.ToString();
			mCoinText.text = "Coins: " + mCurrentSave.mCoins;
			Debug.Log("Coins: " + mCurrentSave.mCoins.ToString());
		}
	}

	public void StartGame()
    {
		// Carga escena de nivel si el auto seleccionado no está bloqueado
		if (CarSelectionController.Instance.mIsSelectedCarBlocked == true)
		{
			Debug.Log("Car is Blocked");
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

		mCurrentSave = SaveSystem.LoadGame();
		mIsGameLoaded = true; // Habilita la edición de monedas
		UpdateCoinUI();
		Debug.Log("GameLoaded");

		//for (int i = 1; i < CarSelectionController.Instance.mCarsToSelect.Length; i++)
		//{
		//CarStats mCarStats = CarSelectionController.Instance.mCarsToSelect[i].GetComponent<CarStats>();

		//if (PlayerPrefs.HasKey($"CarUnlocked{i}"))
		//{
		//mCarStats.IsCarBlocked = PlayerPrefs.GetInt($"CarUnlocked{i}") == 0;
		//}
		//else
		//{
		//mCarStats.IsCarBlocked = true;
		//}
		//}

		// Inicializa los autos desbloqueados según SaveData
		for (int i = 0; i < CarSelectionController.Instance.mCarsToSelect.Length; i++)
		{
			var mCarStats = CarSelectionController.Instance.mCarsToSelect[i].GetComponent<CarStats>();

			if (i < mCurrentSave.mUnlockedCars.Count)
			{
				mCarStats.IsCarBlocked = !mCurrentSave.mUnlockedCars[i];
			}
			else
			{
				// Por defecto bloqueado si no hay dato guardado
				mCarStats.IsCarBlocked = true;
			}
		}
		//Debug.Log("Car Data Loaded");
	}

	public void SaveGame()
	{

		// Asegura que la lista tenga la misma cantidad de autos
		mCurrentSave.mUnlockedCars.Clear();

		for (int i = 1; i < CarSelectionController.Instance.mCarsToSelect.Length; i++)
		{
			var mCarStats = CarSelectionController.Instance.mCarsToSelect[i].GetComponent<CarStats>();
			mCurrentSave.mUnlockedCars.Add(!mCarStats.IsCarBlocked); // true = desbloqueado
		}


		SaveSystem.SaveGame(mCurrentSave);

		//for (int i = 0; i < CarSelectionController.Instance.mCarsToSelect.Length; i++)
		//{
			//CarStats mCarStats = CarSelectionController.Instance.mCarsToSelect[i].GetComponent<CarStats>();
			//PlayerPrefs.SetInt($"CarUnlocked{i}", mCarStats.IsCarBlocked ? 0 : 1);
		//}
		//PlayerPrefs.Save();
		//Debug.Log("Car Data Saved");
	}

	public void OnSaveButtonPressed()
	{
		SaveGame();
	}

	public void OnLoadButtonPressed()
	{
		LoadGame();
	}

	public void OnResetButtonPressed()
	{
		SaveSystem.ResetGame();
		mCurrentSave = new SaveData(); // reinicia los datos en memoria

		// Bloquea todos los autos excepto el primero
		for (int i = 0; i < CarSelectionController.Instance.mCarsToSelect.Length; i++)
		{
			var mCarStats = CarSelectionController.Instance.mCarsToSelect[i].GetComponent<CarStats>();
			mCarStats.IsCarBlocked = i != 0; // primer auto desbloqueado
		}

		UpdateCoinUI();
		Debug.Log("Game data reset.");
	}

	public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
