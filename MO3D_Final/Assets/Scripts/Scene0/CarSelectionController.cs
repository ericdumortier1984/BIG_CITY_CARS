using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarSelectionController : MonoBehaviour
{
	public static CarSelectionController Instance { get; private set; } // Singleton

	public void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
	}

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
	public GameObject mCarBloquedIcon; 
	public GameObject mMedalIcon; 

	private void Start()
	{
		MainMenu.Instance.LoadGame();
		UpdateCarStats();
	}

	public void NextCarSelection() // Metodo para seleccionar siguiente auto del array
    {
        mCarsToSelect[mSelectedCarIndex].SetActive(false); // seteamos falso al principio
        mSelectedCarIndex = (mSelectedCarIndex + 1) % mCarsToSelect.Length; // Calculamos por la cantidad de autos 
        mCarsToSelect[mSelectedCarIndex].SetActive(true); // Luego seteamos verdadero
        Debug.Log("Car selected: " + mSelectedCarIndex);

		// Guardamos el índice seleccionado
		PlayerPrefs.SetInt("mSelectedCarIndex", mSelectedCarIndex);
		PlayerPrefs.Save();

		UpdateCarStats();
	}

	public void PreviousCarSelection() // Metodo para seleccionar el anterior auto del array
    {
		mCarsToSelect[mSelectedCarIndex].SetActive(false); // seteamos falso al principio
        mSelectedCarIndex--; // Restamos el indice
        if (mSelectedCarIndex < 0) // Si el indice es menor a cero
		{
			mSelectedCarIndex += mCarsToSelect.Length; // Comienza el ciclo nuevamente
		}
		mCarsToSelect[mSelectedCarIndex].SetActive(true); // Luego seteamos verdadero
		Debug.Log("Car selected: " + mSelectedCarIndex);

		// Guardamos el índice seleccionado
		PlayerPrefs.SetInt("mSelectedCarIndex", mSelectedCarIndex);
		PlayerPrefs.Save();

		UpdateCarStats();
	}

	private void UpdateCarStats()
	{
		CarStats mCarStats = mCarsToSelect[mSelectedCarIndex].GetComponent<CarStats>(); // Referencia al script de las estadisticas de los autos

		mSelectedCarName = mCarStats.CarName;
		mSelectedCarPriceText = mCarStats.CarPriceText;
		mSelectedCarPrice = mCarStats.CarPrice;
		mSelectedCarSpeed = mCarStats.CarSpeed;
		mSelectedCarAcceleration = mCarStats.CarAcceleration;
		mSelectedCarBreaking = mCarStats.CarBreaking;
		mSelectedCarHandling = mCarStats.CarHandling;
		mIsSelectedCarBlocked = mCarStats.IsCarBlocked;

		// Actualizar UI de estadisticas del auto seleccionado
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

		// Mostrar u ocultar candado + precio
		if (mIsSelectedCarBlocked)
		{
			mCarBloquedIcon.SetActive(true);
		}
		else
		{
			mCarBloquedIcon.SetActive(false);
			mCarPriceText.text = "READY";
		}

		if (mCarStats.MedalEarned)
		{
			mMedalIcon.SetActive(true);
		}
		else 
		{

			mMedalIcon.SetActive(false);
		}
	}

	public void BuyCar()
	{
		// Actualizo estado
		CarStats mCarStats = mCarsToSelect[mSelectedCarIndex].GetComponent<CarStats>();

		if (mCarStats.IsCarBlocked)
		{
			int mCarPrice = mCarStats.CarPrice;

			if (MainMenu.Instance.SpendCoin(mCarPrice))
			{
				mCarStats.IsCarBlocked = false;

				// Actualizo estadisticas e interfaz
				UpdateCarStats();

				// Guardar datos
				MainMenu.Instance.SaveGame();

				// Debug
				Debug.Log("Car Unlocked: " + mSelectedCarName);
			}
		}
	}			
}
