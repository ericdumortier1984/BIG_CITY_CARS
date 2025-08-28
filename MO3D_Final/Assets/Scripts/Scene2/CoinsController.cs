using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsController : MonoBehaviour
{
	[Header("UI")]
	public TMPro.TextMeshProUGUI mItemCoinsText;
	public Slider mItemCoinsSlider;

	[Header("Settings")]
	public int mItemCoinsToCollect;
	public int mItemCoinsCollected;

	private void Start()
	{
		mItemCoinsCollected = LevelData.CoinsCollectedInLevel;

		mItemCoinsSlider.maxValue = mItemCoinsToCollect;
		mItemCoinsSlider.value = mItemCoinsCollected;
		mItemCoinsSlider.interactable = false;

		UpdateHUD();
	}

	public void ItemCoinsCounter()
	{
		AddItemCoins(1);
	}

	public void AddItemCoins(int amount)
	{
		mItemCoinsCollected += amount;
		mItemCoinsCollected = Mathf.Clamp(mItemCoinsCollected, 0, mItemCoinsToCollect);

		// Actualizar HUD
		mItemCoinsSlider.value = mItemCoinsCollected;
		UpdateHUD();

		SaveData saveData = SaveSystem.LoadGame();
		saveData.mCoins += amount;  // solo sumamos monedas
		SaveSystem.SaveGame(saveData);

		LevelData.CoinsCollectedInLevel = mItemCoinsCollected;
	}

	public void UpdateHUD()
	{
		mItemCoinsText.text = mItemCoinsCollected.ToString() + " / " + mItemCoinsToCollect.ToString();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Coins"))
		{
			AddItemCoins(1);
			Destroy(other.gameObject);
		}
	}
}