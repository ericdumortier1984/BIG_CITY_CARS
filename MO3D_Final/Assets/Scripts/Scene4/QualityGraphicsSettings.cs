using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class QualityGraphicsSettings : MonoBehaviour
{
	[Header("Quality Settings")]
	[SerializeField] private TMP_Dropdown mGraphicsDropdown;

	private void Start()
	{
		// Guardo la calidad de los graficos
		int mSavedQuality = PlayerPrefs.GetInt("qualityLevel", 5);
		mGraphicsDropdown.value = mSavedQuality;
		QualitySettings.SetQualityLevel(mSavedQuality);
	}

	public void SetGraphicsQuality()
	{
		int qualityLevel = mGraphicsDropdown.value;
		QualitySettings.SetQualityLevel(mGraphicsDropdown.value);
		PlayerPrefs.SetInt("qualityLevel", mGraphicsDropdown.value); // calidad grafica guardada
	}
}
