using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCarSelectionController : MonoBehaviour
{
	public GameObject[] mCarsToSelect; // Array de autos seleccionables

	private CarSelectionController mCarSelectionController;
	private CarFuelController mCarFuelController;

	private void Start()
	{
		mCarSelectionController = FindObjectOfType<CarSelectionController>();
		mCarFuelController = FindObjectOfType<CarFuelController>();

		// Validamos y cargamos el índice almacenado
		int mSelectedCarIndex = PlayerPrefs.HasKey("mSelectedCarIndex") ? PlayerPrefs.GetInt("mSelectedCarIndex") : 0;

		// Aplicamos el índice cargado
		mCarsToSelect[mSelectedCarIndex].SetActive(true);

		// Sincronizamos el índice con CarSelectionController
		if (mCarSelectionController != null)
		{
			mCarSelectionController.mSelectedCarIndex = mSelectedCarIndex;
		}
	}
}
