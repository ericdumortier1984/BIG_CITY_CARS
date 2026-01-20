using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelaxTrip : MonoBehaviour
{
	[Header("UI")]
	[SerializeField] private TMP_FontAsset font;
	[SerializeField] private float textDuration;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private GameObject instructionPanel;

	[Header("Slider")]
	[SerializeField] private Slider relaxSlider;
	[SerializeField] private TextMeshProUGUI barText;
	[SerializeField] private float relaxTimeRatioDown;
	[SerializeField] private float relaxTimeRatioUp;
	[SerializeField] private float minRelax;
	[SerializeField] private float maxRelax;

	[Header("FX")]
	[SerializeField] private ParticleSystem smokeParticle;
	[SerializeField] private float minSmokeParticleSize;
	[SerializeField] private float maxSmokeParticleSize;

	[Header("References")]
	[SerializeField] private MissionManager missionManager;
	[SerializeField] private List<GameObject> cigarettes;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip relaxTripMusic;
	[SerializeField] private AudioClip collectSFX;
	[SerializeField] private AudioClip winSFX;
	[SerializeField] private AudioClip loseSFX;

	//BOOL
	private bool hasLost = false;

	// MEDAL
	private static bool isMedal = false;

	// MUSIC MISSION START
	private bool missionMusicStarted = false;

	public void BeginRelaxTrip()
	{
		SetElements();
	}

	private void SetElements()
	{
		// INSTRUCTIONS
		UIMissionManager.Instance.ShowMissionText("GET THE CIGARETTES TO FILL THAT BAR", textDuration, 80, font);
		missionManager.StartCoroutine(DelayInstructionPanel());

		// MUSIC
		if (!missionMusicStarted)
		{
			AudioManager.Instance.PlayMissionMusic(relaxTripMusic);
			missionMusicStarted = true;
		}

		// SLIDER SIZE
		relaxSlider.gameObject.SetActive(true);

		// SMOKE;
		relaxSlider.minValue = minRelax;
		relaxSlider.maxValue = maxRelax;
		smokeParticle.gameObject.SetActive(true);

		// TEXT
		barText.gameObject.SetActive(true);
	}

	private void Update()
	{
		UpdateSlider();
		LoseMission();
	}

	private void UpdateSlider()
	{
		relaxSlider.value -= relaxTimeRatioDown * Time.deltaTime;
		UpdateSmoke();
	}

	private void UpdateSmoke()
	{
		float smokeSize = Mathf.InverseLerp(minRelax, maxRelax, relaxSlider.value);
		float newSmokeSize = Mathf.Lerp(minSmokeParticleSize, maxSmokeParticleSize, smokeSize);

		var SmokeParticleMain = smokeParticle.main;
		SmokeParticleMain.startSize = newSmokeSize;
	}

	public void CollectCigarettes(CollectCigarette cigarette)
	{
		AudioManager.Instance.PlaySFX(collectSFX);

		cigarette.gameObject.SetActive(false);

		relaxSlider.value += relaxTimeRatioUp;
		UpdateSmoke();

		if (relaxSlider.value >= maxRelax)
		{
			WinMission();
		}
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
			MainMenu.Instance.AddCoin(3000);
			LevelData.CoinsCollectedInLevel += 3000;

			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[13] = true;
			SaveSystem.SaveGame(saveData);

			isMedal = true;
		}
	}

	private void LoseMission()
	{
		if (hasLost) { return; }
		if (relaxSlider.value <= minRelax)
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
		foreach (GameObject cigarette in cigarettes)
		{
			cigarette.SetActive(false);
		}
		smokeParticle.gameObject.SetActive(false);
		relaxSlider.gameObject.SetActive(false);
		barText.gameObject.SetActive(false);
		instructionPanel.SetActive(false);
	}

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("THATS A REAL TRIP! + 3000 COINS", textDuration, 80, font);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowLoseMessage()
	{
		UIMissionManager.Instance.ShowMissionText("OH NO I SEE THE REALITY!", textDuration, 80, font);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator DelayInstructionPanel()
	{
		instructionPanel.SetActive(false);
		yield return new WaitForSeconds(5);
		instructionPanel.SetActive(true);
	}
}
