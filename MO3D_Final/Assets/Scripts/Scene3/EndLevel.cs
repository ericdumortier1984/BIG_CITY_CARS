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
	}

	private void UpdateEndLevelUI()
	{
		if (mCoinsController != null && mCoinsCollectedText != null)
		{
			//mCoinsCollectedText.text = "COINS COLLECTED IN LEVEL: " + mCoinsController.mItemCoinsCollected.ToString();
			mCoinsCollectedText.text = "COINS COLLECTED IN LEVEL: " + LevelData.CoinsCollectedInLevel.ToString();
		}

		if (mItemWayController != null && mWaypointsCollectedText != null)
		{
			//mWaypointsCollectedText.text = "WAYPOINT COLLECTED IN LEVEL: " + mItemWayController.ItemWaypointCollected.ToString();
			mWaypointsCollectedText.text = "WAYPOINTS COLLECTED IN LEVEL: " + LevelData.WaypointsCollectedInLevel.ToString();
		}
	}
}
