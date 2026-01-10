using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HarvestMission : MonoBehaviour
{
	[Header("Spawn Settings")]
	[SerializeField] private GameObject harvestPrefab;
	[SerializeField] private ParticleSystem collectParticle;

	[Header("UI")]
	[SerializeField] private GameObject instructionPanel;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private float textDuration;

	[Header("Mini Map Icon")]
	[SerializeField] private GameObject harvestMapIcon;

	[Header("Mission Manager")]
	[SerializeField] private MissionManager missionManager;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip cabbageMissionMusic;
	[SerializeField] private AudioClip collectSFX;
	[SerializeField] private AudioClip winSFX;

	private static int prefabCount = 0;
	private static int prefabTotalCount = 98;
	private static bool isMedal = false;
	private static bool missionMusicStarted = false;

	private void Start()
	{
		if (!missionMusicStarted)
		{
			AudioManager.Instance.PlayMissionMusic(cabbageMissionMusic);
			missionMusicStarted = true;
		}

		prefabCount = 0;
		prefabTotalCount = FindObjectsOfType<HarvestMission>().Length;
		harvestMapIcon.SetActive(true);
		instructionPanel.SetActive(true);
		UIMissionManager.Instance.ShowMissionText("RAISE THE HARVEST", textDuration, 50);
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Tractor"))
		{
			collectParticle.transform.position = transform.position;
			collectParticle.Play();

			AudioManager.Instance.PlaySFX(collectSFX);

			prefabCount++;
			Destroy(gameObject);
			
			if (prefabCount == prefabTotalCount)
			{
				UIMissionManager.Instance.ShowMissionText("ALL HARVEST IS RAISED\n + 5 COINS", textDuration, 50);
				instructionPanel.SetActive(false);

				AudioManager.Instance.PlaySFX(winSFX);
				AudioManager.Instance.PlayGameplayMusic();
				missionMusicStarted = false;

				if (!isMedal)
				{
					MainMenu.Instance.AddMedal(1);
					LevelData.MedalCollectedInLevel += 1;
					MainMenu.Instance.AddCoin(5);
					LevelData.CoinsCollectedInLevel += 5;
					SaveData saveData = SaveSystem.LoadGame();
					saveData.missionCompleted[1] = true;
					SaveSystem.SaveGame(saveData);
					isMedal = true;
				}
				missionManager.EndMission();
				harvestMapIcon.SetActive(false);
			}
		}
	}
}
