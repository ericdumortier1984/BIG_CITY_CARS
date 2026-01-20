using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoalRaceTunningTrigger : MonoBehaviour
{
    [Header("Settings")]
	[SerializeField] private TMP_FontAsset font;
	[SerializeField] private AlmostFerrari almostFerrari;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Tunning Car") && almostFerrari.IsCheckpointCompleted())
		{
			almostFerrari.WinMission();
		}
		else if (other.CompareTag("Tunning Car") && !almostFerrari.IsCheckpointCompleted())
		{
			UIMissionManager.Instance.ShowMissionText("YOU MISSED A CHECKPOINT", 2.0f, 50, font);
		}
		else if (other.gameObject == almostFerrari.GetRacers())
		{
			almostFerrari.LoseMission();
		}
	}
}