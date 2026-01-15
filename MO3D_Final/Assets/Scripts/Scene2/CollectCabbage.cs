using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectCabbage : MonoBehaviour
{
	[Header("Spawn Settings")]
	[SerializeField] private GameObject cabbagePrefab;
	[SerializeField] private int maxSpawn = 0;
	[SerializeField] private float spawnRadius = 0f;

	[Header("UI")]
	[SerializeField] private GameObject instructionPanel;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private float textDuration;

	[Header("Mission Manager")]
	[SerializeField] private MissionManager missionManager;
	[SerializeField] private ParticleSystem collectParticle;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip cabbageMissionMusic;     
	[SerializeField] private AudioClip collectSFX;         
	[SerializeField] private AudioClip winSFX;           

	public int spawnCount = 0;
	private static bool isMedal = false;
	private static bool missionMusicStarted = false;

	private void Start()
	{
		SetElements();
	}

	private void SetElements()
	{
		if (spawnCount == 0)
		{
			collectParticle.Play();
			UIMissionManager.Instance.ShowMissionText("COLLECT ALL CABBAGES", textDuration, 50);
		}

		if (!missionMusicStarted)
		{
			AudioManager.Instance.PlayMissionMusic(cabbageMissionMusic);
			missionMusicStarted = true;
		}

		instructionPanel.SetActive(true);
		UIMissionManager.Instance.SetCounter(spawnCount, maxSpawn);
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("FarmCar"))
		{
			AudioManager.Instance.PlaySFX(collectSFX);
			spawnCount++;
			UIMissionManager.Instance.SetCounter(spawnCount, maxSpawn);

			if (spawnCount < maxSpawn)
			{
				collectParticle.transform.position = transform.position;
				collectParticle.Play();
				SpawnCabbage();
			}

			if (spawnCount == maxSpawn)
			{
				instructionPanel.SetActive(false);
				WinMission();
				missionManager.EndMission();
			}

			Destroy(gameObject);
		}
	}

	void SpawnCabbage()
	{
		Vector3 newPos = transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0,
			Random.Range(-spawnRadius, spawnRadius));

		GameObject newCabbage = Instantiate(cabbagePrefab, newPos, Quaternion.identity);
	}

	private void WinMission()
	{
		AudioManager.Instance.PlaySFX(winSFX);
		AudioManager.Instance.PlayGameplayMusic();
		missionMusicStarted = false;
		UIMissionManager.Instance.ShowMissionText("ALL CABBAGES COLLECTED\n + 500 COINS", textDuration, 50);
		UIMissionManager.Instance.HideCounter();

		if (!isMedal)
		{
			MainMenu.Instance.AddMedal(1);
			LevelData.MedalCollectedInLevel += 1;
			MainMenu.Instance.AddCoin(500);
			LevelData.CoinsCollectedInLevel += 500;
			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[0] = true;
			SaveSystem.SaveGame(saveData);
			isMedal = true;
		}
	}
}