using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameInfoController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayableDirector gameInfoTimeline;
    [SerializeField] CinemachineVirtualCamera gameInfoVirtualCamera;
    [SerializeField] CinemachineVirtualCamera mainVirtualCamera;
    [SerializeField] Rigidbody carRb;
    [SerializeField] CinemachineSwitcher virtualCameraSwitcher;
    [SerializeField] private int vehicleIndex;

    private SaveData saveData;
    private bool isInfoGame = false;

	private void Start()
	{
		saveData = SaveSystem.LoadGame();

		if (saveData.infoGame) 
        {
           Destroy(gameObject); 
        }
	}

	private void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("FarmCar"))
        {
			PlayGameInfo();
		}
	}

	private void PlayGameInfo()
    {
        isInfoGame = true;

        carRb.constraints = RigidbodyConstraints.FreezeAll;
        virtualCameraSwitcher.enabled = false;

        gameInfoTimeline.stopped += GameInfoFinish;
        gameInfoTimeline.Play();
    }

    private void GameInfoFinish(PlayableDirector director)
    {
		carRb.constraints = RigidbodyConstraints.None;
		mainVirtualCamera.Priority = 20;
		gameInfoVirtualCamera.Priority = 0;
		virtualCameraSwitcher.enabled = true;

        saveData.infoGame = true;
        SaveSystem.SaveGame(saveData);

        director.stopped -= GameInfoFinish;
        Destroy(gameObject);
	}

}
