using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarSelectionController : MonoBehaviour
{
	public static CarSelectionController Instance { get; private set; } // Instancia Singleton

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
    public GameObject[] mCarsToSelect; // Array de autos seleccionables
    public int mSelectedCarIndex = 0; // Indice de autos seleccionados

	[Header("Selected Car Stats")]
	public string mSelectedCarName; // Nombre del auto seleccionado
	public string mSelectedCarPriceText; // Texto del precio del auto seleccionado
	public int mSelectedCarPrice; // Precio del auto seleccionado
	public float mSelectedCarSpeed; // Velocidad del auto seleccionado
	public float mSelectedCarAcceleration; // Aceleratcion del auto seleccionado
	public float mSelectedCarBreaking; // Frenado del auto seleccionado
	public float mSelectedCarHandling; // Manejo del auto seleccionado
	public bool mIsSelectedCarBloqued; // Auto bloqueado o no

	[Header("UI Elements")]
	public TMPro.TextMeshProUGUI mCarNameText; // Referencia al texto del nombre del auto
	public TMPro.TextMeshProUGUI mCarPriceText; // Referencia al texto del precio del auto
	public int mCarPrice; // Referencia al precio del auto
	public Slider mCarSpeedSlider; // Referencia al slider de velocidad
	public Slider mCarAccelerationSlider; // Referencia al slider de aceleración
	public Slider mCarBreakingSlider; // Referencia al slider de frenado
	public Slider mCarHandlingSlider; // Referencia al slider de manejo
	public GameObject mCarBloquedIcon; // Referencia a la imagen de auto bloqueado

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
		mIsSelectedCarBloqued = mCarStats.IsCarBloqued;

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
		if (mIsSelectedCarBloqued)
		{
			mCarBloquedIcon.SetActive(true);
		}
		else
		{
			mCarBloquedIcon.SetActive(false);
			mCarPriceText.text = "READY";
		}
	}

	public void BuyCar()
	{
		// Actualizo estado
		CarStats mCarStats = mCarsToSelect[mSelectedCarIndex].GetComponent<CarStats>();

		if (mCarStats.IsCarBloqued)
		{
			int mCarPrice = mCarStats.CarPrice;

			if (MainMenu.Instance.SpendCoin(mCarPrice))
			{
				mCarStats.IsCarBloqued = false;

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
