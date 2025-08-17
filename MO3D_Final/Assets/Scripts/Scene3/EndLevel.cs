using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevel : MonoBehaviour
{
	private CoinsController mCoinsController;
	private ItemWaypointController mItemWayController;

	public TMPro.TextMeshProUGUI mCoinsCollectedText; // Referencia al texto de monedas recolectadas
	public TMPro.TextMeshProUGUI mWaypointsCollectedText; // Referencia al texto de waypoints recolectados
	public TMPro.TextMeshProUGUI mMissonCompletedText; // Referencia al texto de misiones completadas

	[System.Serializable]
	public class  Mission
	{
		public string mDescription;
	}
	public List<Mission> mCarMission = new List<Mission>();

	public int mCarIndex;

	private bool IsMissionCompleted = false;

	private void Start()
	{
		mCoinsController = FindObjectOfType<CoinsController>();
		if (mCoinsController == null) 
		{
			Debug.Log("CoinsController script no encontrado en escena"); 
		}

		mItemWayController = FindObjectOfType<ItemWaypointController>();
		if (mItemWayController == null)
		{
			Debug.Log("ItemWaypointController script no encontrado en escena");
		}
	}

	private void Update()
	{
		UpdateEndLevelUI();
		CheckMission();
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

		if (!IsMissionCompleted)
		{
			IsMissionCompleted = true;
		}
	}

	private void CheckMission()
	{
		if (mCarIndex < 0 || mCarIndex >= mCarMission.Count)
		{
			Debug.LogWarning("Empty list");
			return;
		}

		Mission mMission = mCarMission[mCarIndex];


	}

	public void OnClickMainMenu()
	{
		LoaderScene.Load(LoaderScene.mScene.SceneMainMenu);
	}
}
