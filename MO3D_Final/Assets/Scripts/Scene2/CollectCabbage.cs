using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class CollectHay : MonoBehaviour
{
	[Header("Spawn Settings")]
	[SerializeField] private GameObject cabbagePrefab;
	[SerializeField] private int maxSpawn = 0;
	[SerializeField] private float spawnRadius = 0f;

	[Header("UI")]
	[SerializeField] private TextMeshProUGUI missionText;
	[SerializeField] private float textDuration;

	[Header("Mission Manager")]
	[SerializeField] private MissionManager missionManager;

	private static int spawnCount = 0;

	private void Start()
	{
		if (spawnCount == 0)
		{
			UIMissionManager.Instance.ShowMissionText("COLLECT ALL CABBAGES", textDuration);
			UIMissionManager.Instance.SetCounter(spawnCount, maxSpawn);
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("FarmCar"))
		{
			spawnCount++;
			UIMissionManager.Instance.SetCounter(spawnCount, maxSpawn);

			if (spawnCount < maxSpawn)
			{
				SpawnCabbage();
			}

			if (spawnCount == maxSpawn)
			{
				UIMissionManager.Instance.ShowMissionText("ALL CABBAGES COLLECTED", textDuration);
				UIMissionManager.Instance.HideCounter();

				missionManager.EndMission();
			}

			Destroy(gameObject);
		}
	}

	void SpawnCabbage()
	{
		Vector3 newPos = transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0,
			Random.Range(-spawnRadius, spawnRadius));

		GameObject newCabbage = Instantiate(cabbagePrefab, newPos, Quaternion.identity);
	}
}