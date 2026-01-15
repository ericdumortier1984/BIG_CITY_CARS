using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevel : MonoBehaviour
{
	[Header("Texts")]
	[SerializeField] private TMPro.TextMeshProUGUI mCoinsCollectedText;
	[SerializeField] private TMPro.TextMeshProUGUI mWaypointsCollectedText;
	[SerializeField] private TMPro.TextMeshProUGUI medalCollectedText;

	[Header("SFX")]
	[SerializeField] private AudioClip backSFX;

	private CoinsController mCoinsController;
	private ItemWaypointController mItemWayController;

	private void Start()
	{
		Cursor.visible = true;
		mCoinsController = FindObjectOfType<CoinsController>();
		mItemWayController = FindObjectOfType<ItemWaypointController>();
	}

	private void Update()
	{
		UpdateEndLevelUI();
	}

	private void UpdateEndLevelUI()
	{
		if (mCoinsController != null && mCoinsCollectedText != null)
		{
			mCoinsCollectedText.text = "COINS COLLECTED IN LEVEL: " + LevelData.CoinsCollectedInLevel.ToString();
		}

		if (mItemWayController != null && mWaypointsCollectedText != null)
		{
			mWaypointsCollectedText.text = "WAYPOINTS COLLECTED IN LEVEL: " + LevelData.WaypointsCollectedInLevel.ToString();
		}

		medalCollectedText.text = "MEDALS COLLECTED: " + LevelData.MedalCollectedInLevel.ToString();
	}

	public void OnClickMainMenu()
	{
		AudioManager.Instance.PlaySFX(backSFX);
		LoaderScene.Load(LoaderScene.mScene.SceneMainMenu);
	}

	public void ExitGame()
	{
		AudioManager.Instance.PlaySFX(backSFX);
		Application.Quit();
	}
}
