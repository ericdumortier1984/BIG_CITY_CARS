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

	private int current;
	private int total;

	private void Awake()
	{
		Instance = this;
	}

	public void ShowMissionText(string message, float duration)
	{
		StopAllCoroutines();
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

	public void UpdateCounter()
	{
		counterText.text = current + " / " + total;
	}

	public void HideCounter()
	{
		counterText.gameObject.SetActive(false);
	}

	private IEnumerator HideMissionText(float time)
	{
		yield return new WaitForSeconds(time);
		missionText.gameObject.SetActive(false);
	}
}


