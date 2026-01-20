using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarSelectionController : MonoBehaviour
{
	public static CarSelectionController Instance { get; private set; } 

	[Header("Car Selection")]
    public GameObject[] mCarsToSelect; 
    public int mSelectedCarIndex = 0; 

	[Header("Selected Car Stats")]
	public string mSelectedCarName; 
	public string mSelectedCarPriceText; 
	public int mSelectedCarPrice; 
	public float mSelectedCarSpeed; 
	public float mSelectedCarAcceleration; 
	public float mSelectedCarBreaking; 
	public float mSelectedCarHandling; 
	public bool mIsSelectedCarBlocked; 

	[Header("UI Elements")]
	public TMPro.TextMeshProUGUI mCarNameText; 
	public TMPro.TextMeshProUGUI mCarPriceText; 
	public int mCarPrice; 
	public Slider mCarSpeedSlider; 
	public Slider mCarAccelerationSlider;
	public Slider mCarBreakingSlider; 
	public Slider mCarHandlingSlider; 

	[Header("Select / Buy Button")]
	[SerializeField] private Button selectBuyButton;
	[SerializeField] private TextMeshProUGUI selectBuyButtonText;

	[Header("SFX")]
	[SerializeField] private AudioClip backSFX;

	public void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
	}

	private void Start()
	{
		MainMenu.Instance.LoadGame();
		UpdateCarStats();
	}

	public void NextCarSelection() 
    {
		AudioManager.Instance.PlaySFX(backSFX);

		mCarsToSelect[mSelectedCarIndex].SetActive(false); 
        mSelectedCarIndex = (mSelectedCarIndex + 1) % mCarsToSelect.Length;  
        mCarsToSelect[mSelectedCarIndex].SetActive(true); 
        Debug.Log("Car selected: " + mSelectedCarIndex);

		PlayerPrefs.SetInt("mSelectedCarIndex", mSelectedCarIndex);
		PlayerPrefs.Save();

		UpdateCarStats();
	}

	public void PreviousCarSelection() 
    {
		AudioManager.Instance.PlaySFX(backSFX);

		mCarsToSelect[mSelectedCarIndex].SetActive(false); 
        mSelectedCarIndex--; 
        if (mSelectedCarIndex < 0) 
		{
			mSelectedCarIndex += mCarsToSelect.Length; 
		}
		mCarsToSelect[mSelectedCarIndex].SetActive(true); 
		Debug.Log("Car selected: " + mSelectedCarIndex);

		PlayerPrefs.SetInt("mSelectedCarIndex", mSelectedCarIndex);
		PlayerPrefs.Save();

		UpdateCarStats();
	}

	private void UpdateCarStats()
	{
		CarStats mCarStats = mCarsToSelect[mSelectedCarIndex].GetComponent<CarStats>(); 

		mSelectedCarName = mCarStats.CarName;
		mSelectedCarPriceText = mCarStats.CarPriceText;
		mSelectedCarPrice = mCarStats.CarPrice;
		mSelectedCarSpeed = mCarStats.CarSpeed;
		mSelectedCarAcceleration = mCarStats.CarAcceleration;
		mSelectedCarBreaking = mCarStats.CarBreaking;
		mSelectedCarHandling = mCarStats.CarHandling;
		mIsSelectedCarBlocked = mCarStats.IsCarBlocked;

		
		mCarNameText.text = mSelectedCarName;
		mCarPriceText.text = mSelectedCarPriceText;
		mCarPrice = mSelectedCarPrice;
		mCarSpeedSlider.value = mSelectedCarSpeed;
		mCarSpeedSlider.interactable = false;
		mCarAccelerationSlider.value = mSelectedCarAcceleration;
		mCarAccelerationSlider.interactable = false;
		mCarBreakingSlider.value = mSelectedCarBreaking;
		mCarBreakingSlider.interactable = false;
		mCarHandlingSlider.value = mSelectedCarHandling;
		mCarHandlingSlider.interactable = false;

		if (mIsSelectedCarBlocked)
		{
			selectBuyButtonText.text = "BUY";
		}
		else
		{
			mCarPriceText.text = "READY";
			selectBuyButtonText.text = "SELECT";
		}
	}

	public void BuyCar()
	{
		AudioManager.Instance.PlaySFX(backSFX);
	
		CarStats mCarStats = mCarsToSelect[mSelectedCarIndex].GetComponent<CarStats>();

		if (mCarStats.IsCarBlocked)
		{
			int mCarPrice = mCarStats.CarPrice;

			if (MainMenu.Instance.SpendCoin(mCarPrice))
			{
				mCarStats.IsCarBlocked = false;

				UpdateCarStats();

				MainMenu.Instance.SaveGame();
			}
		}
	}			
}
