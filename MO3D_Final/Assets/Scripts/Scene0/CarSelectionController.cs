using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarSelectionController : MonoBehaviour
{
	[Header("Car Selection")]
    public GameObject[] mCarsToSelect; // Array de autos seleccionables
    public int mSelectedCarIndex = 0; // Indice de autos seleccionados

	[Header("Selected Car Stats")]
	public string mSelectedCarName; // Nombre del auto seleccionado
	public string mSelectedCarPrice; // Precio del auto seleccionado
	public float mSelectedCarSpeed; // Velocidad del auto seleccionado
	public float mSelectedCarAcceleration; // Aceleratcion del auto seleccionado
	public float mSelectedCarBreaking; // Frenado del auto seleccionado
	public float mSelectedCarHandling; // Manejo del auto seleccionado

	[Header("UI Elements")]
	public TMPro.TextMeshProUGUI mCarNameText; // Referencia al texto del nombre del auto
	public TMPro.TextMeshProUGUI mCarPriceText; // Referencia al texto del precio del auto
	public Slider mCarSpeedSlider; // Referencia al slider de velocidad
	public Slider mCarAccelerationSlider; // Referencia al slider de aceleración
	public Slider mCarBreakingSlider; // Referencia al slider de frenado
	public Slider mCarHandlingSlider; // Referencia al slider de manejo

	private void Start()
	{
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
		mSelectedCarPrice = mCarStats.CarPrice;
		mSelectedCarSpeed = mCarStats.CarSpeed;
		mSelectedCarAcceleration = mCarStats.CarAcceleration;
		mSelectedCarBreaking = mCarStats.CarBreaking;
		mSelectedCarHandling = mCarStats.CarHandling;

		// Actualizar UI de estadisticas dela uto seleccionado
		mCarNameText.text = mSelectedCarName;
		mCarPriceText.text = mSelectedCarPrice;
		mCarSpeedSlider.value = mSelectedCarSpeed;
		mCarAccelerationSlider.value = mSelectedCarAcceleration;
		mCarBreakingSlider.value = mSelectedCarBreaking;
		mCarHandlingSlider.value = mSelectedCarHandling;
	}
}
