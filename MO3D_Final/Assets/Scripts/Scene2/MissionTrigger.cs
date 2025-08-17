using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    public MissionData missionData;
	public MissionManager missionManager;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("FarmCar"))
		{
			if (!missionManager.CanStartMission())
			{
				return;
			}
			missionManager.StartMissionCabagge(missionData);
			gameObject.SetActive(false);
		}
	}
}

