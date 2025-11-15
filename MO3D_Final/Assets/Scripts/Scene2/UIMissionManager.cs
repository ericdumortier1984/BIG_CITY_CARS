using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMissionManager : MonoBehaviour
{
    public static UIMissionManager Instance;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private TextMeshProUGUI counterText;

	[Header("Timer Reference")]
	[SerializeField] private CountdownTimer CountdownTimerInstance;

	private int current;
	private int total;

	private void Awake()
	{
		Instance = this;
	}

	public void ShowMissionText(string message, float duration, int fontSize)
	{
		StopAllCoroutines();
		missionText.fontSize = fontSize;
		missionText.text = message;
		missionText.gameObject.SetActive(true);
		StartCoroutine(HideMissionText(duration));
	}

	public void SetCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		UpdateCounter();
	}

	public void SetPackageCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);
		UpdatePackageCounter();
	}

	public void SetPassengerCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);
		UpdatePassengerCounter();
	}

	public void SetDonutsCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);
		counterText.fontSize = 36;
		UpdateDonutsCounter();
	}

	public void SetFiresCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);
		counterText.fontSize = 36;
		UpdateFiresCounter();
	}

	public void UpdateCounter()
	{
		counterText.text = current + " / " + total;
	}
	public void UpdatePackageCounter()
	{
		counterText.text = "PACKAGE: " + current + " / " + total;
	}

	public void UpdatePassengerCounter()
	{
		counterText.text = "    PASSENGER: " + current + " / " + total;
	}

	public void UpdateDonutsCounter()
	{
		counterText.text = "DONUTS: " + current + " / " + total;
	}

	public void UpdateFiresCounter()
	{
		counterText.text = "FIRES: " + current + " / " + total;
	}

	public void HideCounter()
	{
		counterText.gameObject.SetActive(false);
	}

	public void ShowTimer(bool show)
	{
		CountdownTimerInstance.gameObject.SetActive(show);
	}

	private IEnumerator HideMissionText(float time)
	{
		yield return new WaitForSeconds(time);
		missionText.gameObject.SetActive(false);
	}
}


