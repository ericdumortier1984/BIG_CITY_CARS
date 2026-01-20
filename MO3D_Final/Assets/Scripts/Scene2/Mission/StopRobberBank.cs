using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class StopRobberBank : MonoBehaviour
{
    [Header("Theft Furgon")]
    [SerializeField] private GameObject theftFurgon;
	[SerializeField] private List<GameObject> moneyBags;
    [SerializeField] private List<Transform> TheftFurgonWaypoints;
    [SerializeField] private float waypointThreshold;
    [SerializeField] private float theftFurgonSpeed;

	[Header("Police Car")]
	[SerializeField] private List<Transform> policeCarWaypoints;

	[Header("Spikes")]
	[SerializeField] private List<GameObject> spikes;

	[Header("UI")]
	[SerializeField] private TMP_FontAsset font;
	[SerializeField] private float textDuration;
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private GameObject InstructionPanel;
	[SerializeField] private GameObject MiniMapInstructionPanel;

	[Header("References")]
    [SerializeField] private MissionManager missionManager;
	[SerializeField] private DamageManager healthTheftFurgon;
	[SerializeField] private GameObject healthBar;
	[SerializeField] private GameObject healthBarText;

    [Header("FX")]
    [SerializeField] private ParticleSystem spawnTheftFurgonParticle;
	[SerializeField] private ParticleSystem destroyTheftFurgonParticle;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip robberBankMusic;
	[SerializeField] private AudioClip alarmSFX;
	[SerializeField] private AudioClip winSFX;
	[SerializeField] private AudioClip loseSFX;

	// BOOL 
	private bool canMoveTheftFurgon = false;
    private bool isTheftFurgonEscape = false;

    // WAYPOINT
    private int currentWaypointIndex = 0;

    // MEDAL
    private static bool isMedal = false;

	// MUSIC MISSION START
	private bool missionMusicStarted = false;

	public void BeginStopRobberBank()
    {
        SetElements();
	}

    private void SetElements()
    {
		AudioManager.Instance.PlaySFX(alarmSFX);

        spawnTheftFurgonParticle.gameObject.SetActive(true);

		foreach (Transform wp in TheftFurgonWaypoints)
		{
			wp.gameObject.SetActive(true);
		}

		foreach (Transform policeCarWp in policeCarWaypoints)
		{
			policeCarWp.gameObject.SetActive(true);
		}

		healthTheftFurgon.BeginDamageSystem();

		healthTheftFurgon.OnGameObjectDestroyed -= WinMission;
		healthTheftFurgon.OnGameObjectDestroyed += WinMission;

		StartCoroutine(NoMoveTheftFurgonCoroutine());
		UIMissionManager.Instance.ShowMissionText("STOP THE ROBBERY BANK", textDuration, 80, font);
		InstructionPanel.SetActive(true);
		MiniMapInstructionPanel.SetActive(true);

		// MUSIC
		if (!missionMusicStarted)
		{
			AudioManager.Instance.PlayMissionMusic(robberBankMusic);
			missionMusicStarted = true;
		}
	}

	private void Update()
	{
        MoveTheftFurgon();
	}

	private void MoveTheftFurgon()
    {
        if (TheftFurgonWaypoints.Count == 0) { return; }
        if (currentWaypointIndex >= TheftFurgonWaypoints.Count) { return; }

        if (canMoveTheftFurgon)
        {
			// DIRECCION
			Transform theftFurgonTargetWp = TheftFurgonWaypoints[currentWaypointIndex];
			Vector3 theftFurgonDirection = (theftFurgonTargetWp.position - theftFurgon.transform.position).normalized;

			// VELOCIDAD
			theftFurgon.transform.position += theftFurgonDirection * theftFurgonSpeed * Time.deltaTime;
			theftFurgon.transform.forward = Vector3.Lerp(theftFurgon.transform.forward, theftFurgonDirection, 10f * Time.deltaTime);

			// DISTANCIA
			float distanceToTheftFurgonWp = Vector3.Distance(theftFurgon.transform.position, theftFurgonTargetWp.position);

			if (distanceToTheftFurgonWp <= waypointThreshold)
			{
				currentWaypointIndex++;

				if (currentWaypointIndex >= TheftFurgonWaypoints.Count)
				{
					LoseMission();
				}
			}
		}
    }

	private void WinMission()
    {
		AudioManager.Instance.PlaySFX(winSFX);
		AudioManager.Instance.PlayGameplayMusic();
		missionMusicStarted = false;

		destroyTheftFurgonParticle.Play();
		healthTheftFurgon.OnGameObjectDestroyed -= WinMission;

		missionManager.EndMission();
		StartCoroutine(ShowWinMessage());
		DisableElements();

		if (!isMedal)
		{
			MainMenu.Instance.AddMedal(1);
			LevelData.MedalCollectedInLevel += 1;
			MainMenu.Instance.AddCoin(5250);
			LevelData.CoinsCollectedInLevel += 5250;

			SaveData saveData = SaveSystem.LoadGame();
			saveData.missionCompleted[6] = true;
			SaveSystem.SaveGame(saveData);

			isMedal = true;
		}
	}

    private void LoseMission()
    {
		AudioManager.Instance.PlaySFX(loseSFX);
		AudioManager.Instance.PlayGameplayMusic();
		missionMusicStarted = false;

		isTheftFurgonEscape = true;
		StartCoroutine(ShowLoseMessage());
		DisableElements();
		missionManager.EndMission();
    }

    private void DisableElements()
    {
        theftFurgon.gameObject.SetActive(false);
		healthBar.gameObject.SetActive(false);
		healthBarText.gameObject.SetActive(false);

		foreach (Transform wp in TheftFurgonWaypoints)
		{
			wp.gameObject.SetActive(false);
		}

		foreach (Transform policeCarWp in policeCarWaypoints)
		{
			policeCarWp.gameObject.SetActive(false);
		}

		foreach (GameObject spk in spikes)
		{
			spk.SetActive(false);
		}

		InstructionPanel.SetActive(false);
		MiniMapInstructionPanel.SetActive(false);
    }

    private IEnumerator NoMoveTheftFurgonCoroutine()
    {
        canMoveTheftFurgon = false;
        yield return new WaitForSeconds(4f);
        canMoveTheftFurgon = true;
    }

	private IEnumerator ShowWinMessage()
	{
		UIMissionManager.Instance.ShowMissionText("YOU ARE A TOUGH COP! + 5250 COINS", textDuration, 80, font);
		missionText.gameObject.SetActive(false);
		yield return new WaitForSeconds(textDuration);
	}

	private IEnumerator ShowLoseMessage()
	{
		UIMissionManager.Instance.ShowMissionText("MONEY WAS ROBBED!", textDuration, 80, font);
		missionText.gameObject.SetActive(false);
		yield return new WaitForSeconds(textDuration);
	}
}
