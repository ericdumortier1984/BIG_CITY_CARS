using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using TMPro;

public class VehicleIntro : MonoBehaviour
{
	[Header("Intro")]
	[SerializeField] private PlayableDirector introTimeline;
	[SerializeField] private CinemachineVirtualCamera introVcam;

	[Header("Gameplay")]
	[SerializeField] private GameObject vehicleController;
	[SerializeField] private Rigidbody playerRigidbody;
	[SerializeField] private CinemachineVirtualCamera mainVcam;
	[SerializeField] private CinemachineSwitcher camSwitcher;

	public bool IsPlayingIntro { get; private set; } = true;

	private void Start()
	{
		PlayIntro();
	}

	public void PlayIntro()
	{
		playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
		mainVcam.Priority = 0;
		introVcam.Priority = 21;
		introTimeline.stopped += OnIntroFinished;
		introTimeline.Play();
	}

	private void OnIntroFinished(PlayableDirector director)
	{
		playerRigidbody.constraints = RigidbodyConstraints.None;
		mainVcam.Priority = 20;
		introVcam.Priority = 0;
		camSwitcher.enabled = true;
		IsPlayingIntro = false;
		director.stopped -= OnIntroFinished;
	}
}
