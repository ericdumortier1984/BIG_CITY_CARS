using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LittleAndFurious : MonoBehaviour
{
	[Header("Mission Setttings")]
	[SerializeField] private float minScale;
	[SerializeField] private float maxScale;
	[SerializeField] private float grownScale;
	[SerializeField] private float decreaseScale;

	[Header("References")]
	[SerializeField] private GameObject miniCar;
	[SerializeField] private List<GameObject> growthPill;
	[SerializeField] private MissionManager missionManager;

	[Header("UI")]
	[SerializeField] private TMP_FontAsset font;
	[SerializeField] private float textDuration;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private GameObject instructionPanel;
	[SerializeField] private Slider sliderSize;
	[SerializeField] private TextMeshProUGUI minSizeText;
	[SerializeField] private TextMeshProUGUI maxSizeText;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip littleAndFuriousMusic;
	[SerializeField] private AudioClip collectSFX;
	[SerializeField] private AudioClip winSFX;
	[SerializeField] private AudioClip loseSFX;

	// BOOL
	private bool startDecreasing = false;
	private bool hasLost = false;

	// VECTOR3
	private Vector3 initialScale;

	// MEDAL
	private static bool isMedal = false;
	 
	// MUSIC MISSION START
	private bool missionMusicStarted = false;

	public void BeginLittlendFurious()
	{
		SetElements();
	}

	private void SetElements()
	{
		// INSTRUCTIONS
		UIMissionManager.Instance.ShowMissionText("TAKE THE GROWTH PILLS, YOU ARE GETTING SMALLER", textDuration, 60, font);
		missionManager.StartCoroutine(DelayInstructionPanel());

		// MUSIC
		if (!missionMusicStarted)
		{
			AudioManager.Instance.PlayMissionMusic(littleAndFuriousMusic);
			missionMusicStarted = true;
		}

		// SCALE
		startDecreasing = true;
		initialScale = miniCar.transform.localScale;

		// SLIDER SIZE
		sliderSize.minValue = minScale;
		sliderSize.maxValue = maxScale;
		sliderSize.value = miniCar.transform.localScale.x;
		sliderSize.gameObject.SetActive(true);

		// SLIDER TEXTS
		minSizeText.gameObject.SetActive(true);
		maxSizeText.gameObject.SetActive(true);
	}

	private void Update()
	{
		DecreaseCar();
		UpdateSliderSize();
		LoseMission();
	}

	private void UpdateSliderSize()
	{
		sliderSize.value = miniCar.transform.localScale.x;
	}

	public void GrowthPillCollected(CollectCapsuleTrigger growthPill)
	{
		AudioManager.Instance.PlaySFX(collectSFX);

		growthPill.gameObject.SetActive(false);
		miniCar.transform.localScale += Vector3.one * grownScale;

		if (miniCar.transform.localScale.x >= maxScale)
		{
			WinMission();
		}
	}

	private void DecreaseCar()
	{
		if (!startDecreasing) return;
		Vector3 scale = miniCar.transform.localScale;

		float decreaseAmount = decreaseScale * Time.deltaTime;
		miniCar.transform.localScale = Vector3.Lerp(miniCar.transform.localScale,miniCar.transform.localScale - Vector3.one * decreaseAmount,Time.deltaTime * 5f);

		// CLAMP
		scale.x = Mathf.Max(scale.x, 0f);
		scale.y = Mathf.Max(scale.y, 0f);
		scale.z = Mathf.Max(scale.z, 0f);

		miniCar.transform.localScale = scale;
	}

	private void WinMission()
	{
		AudioManager.Instance.PlaySFX(winSFX);
		AudioManager.Instance.PlayGameplayMusic();
		missionMusicStarted = false;

		missionManager.EndMission();
		StartCoroutine(ShowWinMessage());
		DisableElements();

		if (!isMedal)
		{
			MainMenu.Instance.AddMedal(1);
			LevelData.MedalCollectedInLevel += 1;
			MainMenu.Instance.AddCoin(1000);
			LevelData.CoinsCollectedInLevel += 1000;

			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[9] = true;
			SaveSystem.SaveGame(saveData);

			isMedal = true;
		}
	}

	private void LoseMission()
	{
		if (hasLost) { return; }
		if (miniCar.transform.localScale.x <= minScale)
		{
			AudioManager.Instance.PlaySFX(loseSFX);
			AudioManager.Instance.PlayGameplayMusic();
			missionMusicStarted = false;

			hasLost = true;
			StartCoroutine(ShowLoseMessage());
			DisableElements();
			missionManager.EndMission();
		}
	}

	private void DisableElements()
	{
		foreach (GameObject pill in growthPill)
		{
			pill.SetActive(false);
		}
		startDecreasing = false;
		sliderSize.gameObject.SetActive(false);
		minSizeText.gameObject.SetActive(false);
		maxSizeText.gameObject.SetActive(false);
		instructionPanel.SetActive(false);
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("LIKE A MONSTER TRUCK + 1000 COINS", textDuration, 80, font);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowLoseMessage()
	{
		UIMissionManager.Instance.ShowMissionText("YOUR VERY TINY", textDuration, 80, font);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator DelayInstructionPanel()
	{
		instructionPanel.SetActive(false);
		yield return new WaitForSeconds(12);
		instructionPanel.SetActive(true);
	}
}
