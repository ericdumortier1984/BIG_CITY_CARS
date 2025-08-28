using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextOnOff : MonoBehaviour
{
	public enum TextMode { Timed, Blinking, Delayed }
	private bool mIsTextOn = true;
	private SaveData saveData;

	[Header("References")]
	public TextMeshProUGUI mTMP_Text;
	public Image icon = null;

	[Header("One-Time Settings")]
	[SerializeField] private bool isOneTime = false;
	[SerializeField] private int OneTimeID = -1;

	[Header("Settings")]
	[SerializeField] private TextMode mode = TextMode.Timed;
	[SerializeField] private float mToggleInterval = 1.0f;
	[SerializeField] private float timeInScreen = 0f;
	[SerializeField] private float delay = 0f;
   

	private void Start()
	{
		saveData = SaveSystem.LoadGame();

		if (isOneTime && OneTimeID >= 0 && saveData.infoGame[OneTimeID])
		{
			gameObject.SetActive(false);
			return;
		}

		if (isOneTime && OneTimeID >= 0)
		{
			saveData.infoGame[OneTimeID] = true;
			SaveSystem.SaveGame(saveData);
		}

		mTMP_Text.enabled = false;

		if (icon != null)
			icon.enabled = false;

		if (mode == TextMode.Timed)
		{
			StartCoroutine(ShowText());
		}
		else if (mode == TextMode.Blinking)
		{
			InvokeRepeating(nameof(ToggleText), 0f, mToggleInterval);
		}
		else if (mode == TextMode.Delayed)
		{
			StartCoroutine(Delayedtext());
		}
	}

	void ToggleText()
	{
		mIsTextOn = !mIsTextOn;
		mTMP_Text.enabled = mIsTextOn;
	}

	private IEnumerator ShowText()
	{
		mTMP_Text.enabled = true;

		if (icon != null)
			icon.enabled = true;

		yield return new WaitForSeconds(timeInScreen);

		mTMP_Text.enabled = false;

		if (icon != null)
			icon.enabled = false;
	}

	private IEnumerator Delayedtext()
	{
		yield return new WaitForSeconds(delay);

		mTMP_Text.enabled = true;
		if (icon != null)
			icon.enabled = true;

		yield return new WaitForSeconds(timeInScreen);

		mTMP_Text.enabled = false;

		if (icon != null)
			icon.enabled = false;
	}
}
