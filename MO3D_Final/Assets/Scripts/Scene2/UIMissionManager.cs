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

		counterText.fontSize = 36;
		counterText.fontWeight = FontWeight.Regular;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);

		UpdatePackageCounter();
	}

	public void SetPassengerCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);
		counterText.fontSize = 36;
		counterText.fontWeight = FontWeight.Regular;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);

		UpdatePassengerCounter();
	}

	public void SetDonutsCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 36;
		counterText.fontWeight = FontWeight.Regular;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);

		UpdateDonutsCounter();
	}

	public void SetFiresCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 36;
		counterText.fontWeight = FontWeight.Regular;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);

		UpdateFiresCounter();
	}

	public void SetProvisionCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 36;
		counterText.fontWeight = FontWeight.Regular;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);

		UpdateProvisionCounter();
	}

	public void SetPalletsCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 36;
		counterText.fontWeight = FontWeight.Regular;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);

		UpdatePalletsCounter();
	}

	public void SetCheckpointCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 36;
		counterText.fontWeight = FontWeight.Heavy;    
		counterText.outlineWidth = 0.25f;           
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);

		UpdateCheckpointCounter();
	}

	public void SetFlagsCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 36;
		counterText.fontWeight = FontWeight.Heavy;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);

		UpdateFlagsCounter();
	}

	public void SetAutopartsCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 36;
		counterText.fontWeight = FontWeight.Heavy;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);

		UpdateAutopartsCounter();
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

	public void UpdateProvisionCounter()
	{
		counterText.text = "PROVISIONS: " + current + " / " + total;
	}

	public void UpdatePalletsCounter()
	{
		counterText.text = "PALLETS: " + current + " / " + total;
	}
	public void UpdateCheckpointCounter()
	{
		counterText.text = "CHECKPOINTS: " + current + " / " + total;
	}

	public void UpdateFlagsCounter()
	{
		counterText.text = "FLAGS: " + current + " / " + total;
	}

	public void UpdateAutopartsCounter()
	{
		counterText.text = "AUTOPARTS: " + current + " / " + total;
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


