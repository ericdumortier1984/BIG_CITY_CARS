using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class VehicleIntro : MonoBehaviour
{
	[Header("Intro")]
	[SerializeField] private PlayableDirector introTimeline;
	[SerializeField] private CinemachineVirtualCamera introVcam;

	[Header("Gameplay")]
	[SerializeField] private GameObject vehicleController;
	[SerializeField] private CinemachineVirtualCamera mainVcam;
	[SerializeField] private CinemachineSwitcher camSwitcher;

	public bool IsPlayingIntro { get; private set; } = true;

	private void Start()
	{
		mainVcam.Priority = 0;
		introVcam.Priority = 21;
		introTimeline.stopped += OnIntroFinished;
		introTimeline.Play();
	}

	private void OnIntroFinished(PlayableDirector director)
	{
		mainVcam.Priority = 20;
		introVcam.Priority = 0;
		camSwitcher.enabled = true;
		IsPlayingIntro = false;
		director.stopped -= OnIntroFinished;
	}
}
