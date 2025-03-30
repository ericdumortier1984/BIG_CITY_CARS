using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelectionController : MonoBehaviour
{
    public GameObject[] mCarsToSelect; // Array de autos seleccionables
    public int mSelectedCarIndex = 0; // Indice de autos seleccionados

    public void NextCarSelection() // Metodo para seleccionar siguiente auto del array
    {
        mCarsToSelect[mSelectedCarIndex].SetActive(false); // seteamos falso al principio
        mSelectedCarIndex = (mSelectedCarIndex + 1) % mCarsToSelect.Length; // Calculamos por la cantidad de autos 
        mCarsToSelect[mSelectedCarIndex].SetActive(true); // Luego seteamos verdadero
        Debug.Log("Car selected: " + mSelectedCarIndex);

		// Guardamos el índice seleccionado
		PlayerPrefs.SetInt("mSelectedCarIndex", mSelectedCarIndex);
		PlayerPrefs.Save();

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
	}
}
