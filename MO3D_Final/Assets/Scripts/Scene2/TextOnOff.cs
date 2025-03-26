using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextOnOff : MonoBehaviour
{
    public TextMeshProUGUI mTMP_Text;
    public float mToggleInterval = 1.0f; // Intervalo prende y apaga 
    private bool mIsTextOn = true;

	private void Start()
	{
		InvokeRepeating("ToggleText", 0, mToggleInterval);
	}

	void ToggleText()
	{
		mIsTextOn = !mIsTextOn;
		mTMP_Text.enabled = mIsTextOn;
	}
}
