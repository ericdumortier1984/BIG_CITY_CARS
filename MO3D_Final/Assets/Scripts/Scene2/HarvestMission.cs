using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HarvestMission : MonoBehaviour
{
	[Header("Spawn Settings")]
	[SerializeField] private GameObject harvestPrefab;
	
	[Header("UI")]
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private float textDuration;

	[Header("Mini Map Icon")]
	[SerializeField] private GameObject harvestMapIcon;

	[Header("Mission Manager")]
	[SerializeField] private MissionManager missionManager;

	private static int prefabCount = 0;
	private static int prefabTotalCount = 50;

	private void Start()
	{
		prefabCount = 0;
		prefabTotalCount = FindObjectsOfType<HarvestMission>().Length;
		harvestMapIcon.SetActive(true);
		UIMissionManager.Instance.ShowMissionText("RAISE THE HARVEST", textDuration);
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("FarmCar"))
		{
			prefabCount++;

			Destroy(gameObject);
			
			if (prefabCount == prefabTotalCount)
			{
				UIMissionManager.Instance.ShowMissionText("ALL HARVEST IS RAISED", textDuration);
				missionManager.EndMission();
				harvestMapIcon.SetActive(false);
			}
		}
	}
}
