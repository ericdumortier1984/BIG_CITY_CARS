using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarFuelController : MonoBehaviour
{
	[SerializeField] private float mFuel; // Combustible inicial
	private float mMaxFuel = 100f; // Maximo de combustible
	private float mCurrentFuel; // Combustible actual
	private float mBurnOutFuel = 1f; // Quemamos combustible

	public bool isFuelBurning;

	[SerializeField] private Slider mFuelBar; // Referencia a la barra de combustible
	[SerializeField] private GameObject mItemFuel; // Referencia al item combustible
	[SerializeField] private GameObject mFuelPump; // Referencia a la bomba de combustible

	private void Start()
	{
		mCurrentFuel = mFuel; // Iniciamos con maximo de combustible
		mFuelBar.maxValue = mMaxFuel; // Valor maximo del slider
		mFuelBar.value = mCurrentFuel; // Valor actual del slider
		OnBurningFuel();
	}

	private void Update()
	{
		if (isFuelBurning && mCurrentFuel > 0)
		{
			mCurrentFuel -= mBurnOutFuel * Time.deltaTime; // Quemamos combustible
			mCurrentFuel = Mathf.Clamp(mCurrentFuel, 0, mMaxFuel); // Limite del valor entre 0 y maximo
			mFuelBar.value = mCurrentFuel; // Actualizamos valor del slider
		}
		else
		{
			mCurrentFuel = 0;
		}
	}

	public void OnBurningFuel()
	{
		isFuelBurning = true;
	}

	public void OnfillingFuel()
	{
		mCurrentFuel += 5f; // Incrementar el combustible actual
		mFuelBar.value = mCurrentFuel; // Actualizar el valor del slider
	}

	public void OnUseFuelpump()
	{
		mCurrentFuel += mMaxFuel; // Tanque lleno
		mFuelBar.value = mCurrentFuel;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "ItemFuel")
		{
			OnfillingFuel();
		}

		if (other.tag == "FuelPump")
		{
			OnUseFuelpump();
		}
	}
}
