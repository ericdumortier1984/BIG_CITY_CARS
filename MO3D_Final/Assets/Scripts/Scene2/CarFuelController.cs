using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarFuelController : MonoBehaviour
{
	[Header("Fuel Settings")]
	[SerializeField] private float totalGameTime;
	[SerializeField] private Slider fuelBar; 
	[SerializeField] private GameObject itemFuel;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip collectSFX;

	private float currentFuel;
	private float burnOutFuel = 1f; 
	public bool isFuelBurning = true;

	private CoinsController coinsController; 

	public float CurrentFuel { get { return currentFuel; }set { currentFuel = value; } }

	private void Start()
	{
		currentFuel = totalGameTime;

		fuelBar.maxValue = totalGameTime;
		fuelBar.value = currentFuel;
		fuelBar.interactable = false;
	}

	private void Update()
	{
		OnBurningFuel();
	}

	private void OnBurningFuel()
	{
		isFuelBurning = true;

		if (isFuelBurning && currentFuel > 0)
		{
			currentFuel -= burnOutFuel * Time.deltaTime;
			currentFuel = Mathf.Clamp(currentFuel, 0, totalGameTime);
			fuelBar.value = currentFuel;
		}
		else if (currentFuel <= 0)
		{
			currentFuel = 0;
		}
	}

	public void OnfillingFuel()
	{
		currentFuel += 15f;
		currentFuel = Mathf.Clamp(currentFuel, 0, totalGameTime);
		fuelBar.value = currentFuel;
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "ItemFuel")
		{
			AudioManager.Instance.PlaySFX(collectSFX);
			OnfillingFuel();
			Destroy(other.gameObject);
		}
	}
}
