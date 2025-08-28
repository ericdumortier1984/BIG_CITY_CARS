using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarFuelController : MonoBehaviour
{
	[Header("Fuel Settings")]
	[SerializeField] private float fuel; 
	[SerializeField] private float maxFuel; 
	[SerializeField] private float burnOutFuel; 
	[SerializeField] private Slider fuelBar; 
	[SerializeField] private GameObject itemFuel; 

	public bool isFuelBurning;
	private float currentFuel; 
	private CoinsController coinsController; 

	public float CurrentFuel { get { return currentFuel; }set { currentFuel = value; } }

	private void Start()
	{
		currentFuel = fuel; 
		fuelBar.maxValue = maxFuel; 
		fuelBar.value = currentFuel; 
		fuelBar.interactable = false;
		OnBurningFuel();
	}

	private void Update()
	{
		if (isFuelBurning && currentFuel > 0)
		{
			currentFuel -= burnOutFuel * Time.deltaTime;
			currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel); 
			fuelBar.value = currentFuel; 
		}
		else
		{
			currentFuel = 0;
		}
	}

	public void OnBurningFuel()
	{
		isFuelBurning = true;
	}

	public void OnfillingFuel()
	{
		currentFuel += 5f; 
		fuelBar.value = currentFuel; 
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "ItemFuel")
		{
			OnfillingFuel();
		}
	}
}
