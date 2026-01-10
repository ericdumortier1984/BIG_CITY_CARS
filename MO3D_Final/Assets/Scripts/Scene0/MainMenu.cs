using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public static MainMenu Instance { get; private set; }
	private SaveData mCurrentSave = new SaveData();

	[Header("TMP UI")]
	public TMPro.TextMeshProUGUI mCoinText;
	public TMPro.TextMeshProUGUI medalText;

	[Header("Reset Confirmation")]
	public GameObject mConfirmResetPanel;
	public Button mYesButton;
	public Button mNoButton;

	[Header("SFX")]
	[SerializeField] private AudioClip backSFX;

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
		//mCurrentSave = new SaveData();
		mCurrentSave = SaveSystem.LoadGame();
		UpdateCoinUI();
		UpdateMedalUI();
	}

	public void AddMedal(int medalAmount)
	{
		mCurrentSave.medals += medalAmount;
		SaveSystem.SaveGame(mCurrentSave);
		UpdateMedalUI();
	}

	public void AddCoin(int mCoinAmount)
	{
		mCurrentSave.mCoins += mCoinAmount;
		SaveSystem.SaveGame(mCurrentSave);
		UpdateCoinUI();
	}

	public void UpdateMedalUI()
	{
		medalText.text = "MEDALS: " + mCurrentSave.medals + " /24 ";
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
			//Debug.Log("Not enough coins");
			return false;
		}
	}

	private void UpdateCoinUI()
	{
		if (mCoinText != null)
		{
			mCoinText.text = "COINS: " + mCurrentSave.mCoins;
			//Debug.Log("Coins: " + mCurrentSave.mCoins.ToString());
		}
	}

	public void StartGame()
    {
		if (CarSelectionController.Instance.mIsSelectedCarBlocked == true)
		{
			//Debug.Log("Car is Blocked");
			return;
		}
		else
		{
			AudioManager.Instance.PlaySFX(backSFX);
			LoaderScene.Load(LoaderScene.mScene.SceneGameTpFinal);
		}
    }

    public void ShowSettings()
    {
		AudioManager.Instance.PlaySFX(backSFX);
		LoaderScene.Load(LoaderScene.mScene.SceneSettings);
    }

    public void Showcredits()
    {
		AudioManager.Instance.PlaySFX(backSFX);
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
		UpdateMedalUI();
		//Debug.Log("GameLoaded");

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
		AudioManager.Instance.PlaySFX(backSFX);
		OnResetOptions();
	}

	public void OnResetOptions()
	{
		mConfirmResetPanel.SetActive(true);
		mYesButton.onClick.RemoveAllListeners();
		mNoButton.onClick.RemoveAllListeners();

		mYesButton.onClick.AddListener(() => {
			AudioManager.Instance.PlaySFX(backSFX);
			SaveSystem.ResetGame();
			mCurrentSave = new SaveData(); 
			
			for (int i = 0; i < CarSelectionController.Instance.mCarsToSelect.Length; i++)
			{
				var mCarStats = CarSelectionController.Instance.mCarsToSelect[i].GetComponent<CarStats>();
				mCarStats.IsCarBlocked = i != 0; 
			}

			UpdateCoinUI();
			UpdateMedalUI();
			mConfirmResetPanel.SetActive(false);
			//Debug.Log("Game data reset");
		});

		mNoButton.onClick.AddListener(() => {
			AudioManager.Instance.PlaySFX(backSFX);
			mConfirmResetPanel.SetActive(false);
		});
	}

	public void ExitGame()
    {
		AudioManager.Instance.PlaySFX(backSFX);
		Application.Quit();
        Debug.Log("Quit Game");
    }
}
