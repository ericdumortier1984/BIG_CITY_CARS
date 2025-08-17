using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public static MainMenu Instance { get; private set; }
	public TMPro.TextMeshProUGUI mCoinText;
	private SaveData mCurrentSave = new SaveData(); 

	[Header("Reset Confirmation")]
	public GameObject mConfirmResetPanel;
	public Button mYesButton;
	public Button mNoButton;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;

		Cursor.visible = true;
		mConfirmResetPanel.SetActive(false);
		mCurrentSave = new SaveData();
		UpdateCoinUI();
	}

	public void AddCoin(int mCoinAmount)
	{
		mCurrentSave.mCoins += mCoinAmount;
		SaveSystem.SaveGame(mCurrentSave);
		UpdateCoinUI();
	}

	public bool SpendCoin(int mCoinAmount)
	{
		if (mCurrentSave.mCoins >= mCoinAmount)
		{
			mCurrentSave.mCoins -= mCoinAmount;
			SaveSystem.SaveGame(mCurrentSave);
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
			mCoinText.text = "COINS: " + mCurrentSave.mCoins;
			Debug.Log("Coins: " + mCurrentSave.mCoins.ToString());
		}
	}

	public void StartGame()
    {
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
		SaveGame();
		LoaderScene.Load(LoaderScene.mScene.SceneMainMenu);
    }
    
	public void LoadGame()
	{
		mCurrentSave = SaveSystem.LoadGame();
		UpdateCoinUI();
		Debug.Log("GameLoaded");

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

	/*public void SaveGame()
	{

		// Asegura que la lista tenga la misma cantidad de autos
		mCurrentSave.mUnlockedCars.Clear();

		for (int i = 1; i < CarSelectionController.Instance.mCarsToSelect.Length; i++)
		{
			var mCarStats = CarSelectionController.Instance.mCarsToSelect[i].GetComponent<CarStats>();
			mCurrentSave.mUnlockedCars.Add(!mCarStats.IsCarBlocked); // true = desbloqueado
		}

		SaveSystem.SaveGame(mCurrentSave);
	}*/


	public void SaveGame()
	{
		mCurrentSave.mUnlockedCars.Clear();

		for (int i = 0; i < CarSelectionController.Instance.mCarsToSelect.Length; i++) // INCLUIR EL ÍNDICE 0
		{
			var mCarStats = CarSelectionController.Instance.mCarsToSelect[i].GetComponent<CarStats>();
			mCurrentSave.mUnlockedCars.Add(!mCarStats.IsCarBlocked); // true = desbloqueado
		}

		SaveSystem.SaveGame(mCurrentSave);
	}


	public void OnResetButtonPressed()
	{
		OnResetOptions();
	}

	public void OnResetOptions()
	{
		mConfirmResetPanel.SetActive(true);
		mYesButton.onClick.RemoveAllListeners();
		mNoButton.onClick.RemoveAllListeners();

		mYesButton.onClick.AddListener(() => {
			SaveSystem.ResetGame();
			mCurrentSave = new SaveData(); 
			
			for (int i = 0; i < CarSelectionController.Instance.mCarsToSelect.Length; i++)
			{
				var mCarStats = CarSelectionController.Instance.mCarsToSelect[i].GetComponent<CarStats>();
				mCarStats.IsCarBlocked = i != 0; 
			}
			UpdateCoinUI();
			mConfirmResetPanel.SetActive(false);
			Debug.Log("Game data reset.");
		});

		mNoButton.onClick.AddListener(() => {
			mConfirmResetPanel.SetActive(false);
		});
	}

	public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
