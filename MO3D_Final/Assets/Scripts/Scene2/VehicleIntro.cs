using Cinemachine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class VehicleIntro : MonoBehaviour
{
	[Header("Mission")]
	[SerializeField] private List<int> missionIndex = new List<int>();

	[Header("Intro")]
	[SerializeField] private PlayableDirector introTimeline;
	[SerializeField] private CinemachineVirtualCamera introVcam;

	[Header("Gameplay")]
	[SerializeField] private GameObject vehicleController;
	[SerializeField] private Rigidbody playerRigidbody;
	[SerializeField] private CinemachineVirtualCamera mainVcam;
	[SerializeField] private CinemachineSwitcher camSwitcher;

	private SaveData saveData;

	public bool IsPlayingIntro { get; private set; } = true;

	private void Start()
	{
		saveData = SaveSystem.LoadGame();

		if (IsMissionCompleted())
		{
			SkipIntro();
			return;
		}

		PlayIntro();
	}

	private bool IsMissionCompleted()
	{
		foreach (int index in missionIndex)
		{
			if (index < 0 || index >= saveData.missionCompleted.Count)
				continue;

			if (saveData.missionCompleted[index])
				return true;
		}

		return false;
	}

	public void PlayIntro()
	{
		playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
		mainVcam.Priority = 0;
		introVcam.Priority = 21;
		introTimeline.stopped += OnIntroFinished;
		introTimeline.Play();
	}

	private void SkipIntro()
	{
		IsPlayingIntro = false;

		playerRigidbody.constraints = RigidbodyConstraints.None;

		mainVcam.Priority = 19;
		introVcam.Priority = 0;

		camSwitcher.enabled = true;
	}

	private void OnIntroFinished(PlayableDirector director)
	{
		playerRigidbody.constraints = RigidbodyConstraints.None;
		mainVcam.Priority = 19;
		introVcam.Priority = 0;
		camSwitcher.enabled = true;
		IsPlayingIntro = false;
		director.stopped -= OnIntroFinished;
	}
}
