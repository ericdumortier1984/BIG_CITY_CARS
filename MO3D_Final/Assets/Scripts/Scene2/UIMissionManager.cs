using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIMissionManager : MonoBehaviour
{
    public static UIMissionManager Instance;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private TextMeshProUGUI counterText;
	[SerializeField] private TMP_FontAsset font;

	[Header("Timer Reference")]
	[SerializeField] private CountdownTimer CountdownTimerInstance;

	private int current;
	private int total;

	private void Awake()
	{
		Instance = this;
	}

	public void ShowMissionText(string message, float duration, int fontSize, TMP_FontAsset fontAsset)
	{
		StopAllCoroutines();

		missionText.alignment = TextAlignmentOptions.Center;
		missionText.fontSize = fontSize;
		missionText.text = message;
		missionText.gameObject.SetActive(true);
		StartCoroutine(HideMissionText(duration));
	}

	public void SetCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.font = font;
		counterText.fontSize = 58;
		counterText.fontWeight = FontWeight.Regular;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);
		counterText.alignment = TextAlignmentOptions.Left;
		counterText.rectTransform.anchoredPosition = new Vector2(120f, 250f);

		UpdateCounter();
	}

	public void SetPackageCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 58;
		counterText.fontWeight = FontWeight.Regular;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);
		counterText.alignment = TextAlignmentOptions.Left;
		counterText.rectTransform.anchoredPosition = new Vector2(120f, 250f);

		UpdatePackageCounter();
	}

	public void SetPassengerCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);
		counterText.fontSize = 58;
		counterText.fontWeight = FontWeight.Regular;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);
		counterText.alignment = TextAlignmentOptions.Left;
		counterText.rectTransform.anchoredPosition = new Vector2(120f, 250f);

		UpdatePassengerCounter();
	}

	public void SetDonutsCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 58;
		counterText.fontWeight = FontWeight.Regular;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);
		counterText.alignment = TextAlignmentOptions.Left;
		counterText.rectTransform.anchoredPosition = new Vector2(120f, 250f);

		UpdateDonutsCounter();
	}

	public void SetFiresCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 58;
		counterText.fontWeight = FontWeight.Regular;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);
		counterText.alignment = TextAlignmentOptions.Left;
		counterText.rectTransform.anchoredPosition = new Vector2(120f, 250f);

		UpdateFiresCounter();
	}

	public void SetProvisionCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 58;
		counterText.fontWeight = FontWeight.Regular;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);
		counterText.alignment = TextAlignmentOptions.Left;
		counterText.rectTransform.anchoredPosition = new Vector2(120f, 250f);

		UpdateProvisionCounter();
	}

	public void SetPalletsCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 58;
		counterText.fontWeight = FontWeight.Regular;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);
		counterText.alignment = TextAlignmentOptions.Left;
		counterText.rectTransform.anchoredPosition = new Vector2(120f, 250f);

		UpdatePalletsCounter();
	}

	public void SetCheckpointCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 58;
		counterText.fontWeight = FontWeight.Heavy;    
		counterText.outlineWidth = 0.25f;           
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);
		counterText.alignment = TextAlignmentOptions.Left;
		counterText.rectTransform.anchoredPosition = new Vector2(120f, 250f);

		UpdateCheckpointCounter();
	}

	public void SetFlagsCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 58;
		counterText.fontWeight = FontWeight.Heavy;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);
		counterText.alignment = TextAlignmentOptions.Left;
		counterText.rectTransform.anchoredPosition = new Vector2(120f, 250f);

		UpdateFlagsCounter();
	}

	public void SetAutopartsCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 58;
		counterText.fontWeight = FontWeight.Heavy;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);
		counterText.alignment = TextAlignmentOptions.Left;
		counterText.rectTransform.anchoredPosition = new Vector2(120f, 250f);

		UpdateAutopartsCounter();
	}

	public void SetSuppliesCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 58;
		counterText.fontWeight = FontWeight.Heavy;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);
		counterText.alignment = TextAlignmentOptions.Left;
		counterText.rectTransform.anchoredPosition = new Vector2(120f, 250f);

		UpdateSuppliesCounter();
	}

	public void SetParkingCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 58;
		counterText.fontWeight = FontWeight.Heavy;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);
		counterText.alignment = TextAlignmentOptions.Left;
		counterText.rectTransform.anchoredPosition = new Vector2(120f, 250f);

		UpdateParkingCounter();
	}

	public void SetFuelPumpCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 58;
		counterText.fontWeight = FontWeight.Heavy;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);
		counterText.alignment = TextAlignmentOptions.Left;
		counterText.rectTransform.anchoredPosition = new Vector2(120f, 250f);

		UpdateFuelPumpCounter();
	}

	public void SetBusStopCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 58;
		counterText.fontWeight = FontWeight.Heavy;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);
		counterText.alignment = TextAlignmentOptions.Left;
		counterText.rectTransform.anchoredPosition = new Vector2(120f, 250f);

		UpdateBusStopCounter();
	}

	public void SetPhotoCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 58;
		counterText.fontWeight = FontWeight.Heavy;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);
		counterText.alignment = TextAlignmentOptions.Left;
		counterText.rectTransform.anchoredPosition = new Vector2(120f, 250f);

		UpdatePhotoCounter();
	}

	public void SetAccidentSceneCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 58;
		counterText.fontWeight = FontWeight.Heavy;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);
		counterText.alignment = TextAlignmentOptions.Left;
		counterText.rectTransform.anchoredPosition = new Vector2(120f, 250f);

		UpdateAccidentSceneCounter();
	}

	public void SetIceCreamCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 58;
		counterText.fontWeight = FontWeight.Heavy;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);
		counterText.alignment = TextAlignmentOptions.Left;
		counterText.rectTransform.anchoredPosition = new Vector2(120f, 250f);

		UpdateIceCreamCounter();
	}

	public void SetMaterialIceCreamCounter(int currentValue, int totalValue)
	{
		current = currentValue;
		total = totalValue;
		counterText.gameObject.SetActive(true);

		counterText.fontSize = 58;
		counterText.fontWeight = FontWeight.Heavy;
		counterText.outlineWidth = 0.25f;
		counterText.outlineColor = new Color32(242, 224, 136, 255);
		counterText.faceColor = new Color32(242, 224, 136, 255);
		counterText.alignment = TextAlignmentOptions.Left;
		counterText.rectTransform.anchoredPosition = new Vector2(120f, 250f);

		UpdateMaterialIceCreamCounter();
	}

	public void UpdateCounter()
	{
		counterText.text = "CABAGGE: " + current + " / " + total;
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

	public void UpdateSuppliesCounter()
	{
		counterText.text = "UNLOAD: " + current + " / " + total;
	}

	public void UpdateParkingCounter()
	{
		counterText.text = "PARKED: " + current + " / " + total;
	}

	public void UpdateFuelPumpCounter()
	{
		counterText.text = "FUEL TANKS: " + current + " / " + total;
	}

	public void UpdateBusStopCounter()
	{
		counterText.text = "BUS STOPS: " + current + " / " + total;
	}

	public void UpdatePhotoCounter()
	{
		counterText.text = "PHOTOS: " + current + " / " + total;
	}

	public void UpdateAccidentSceneCounter()
	{
		counterText.text = "ACCIDENTS: " + current + " / " + total;
	}

	public void UpdateIceCreamCounter()
	{
		counterText.text = "ICE CREAMS: " + current + " / " + total;
	}

	public void UpdateMaterialIceCreamCounter()
	{
		counterText.text = "INGREDIENTS: " + current + " / " + total;
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


