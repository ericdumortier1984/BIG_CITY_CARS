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

		// Validamos y cargamos el �ndice almacenado
		int mSelectedCarIndex = PlayerPrefs.HasKey("mSelectedCarIndex") ? PlayerPrefs.GetInt("mSelectedCarIndex") : 0;

		// Aplicamos el �ndice cargado
		mCarsToSelect[mSelectedCarIndex].SetActive(true);

		// Sincronizamos el �ndice con CarSelectionController
		if (mCarSelectionController != null)
		{
			mCarSelectionController.mSelectedCarIndex = mSelectedCarIndex;
		}
	}
}
