using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalMoreDonutsTrigger : MonoBehaviour
{
    private MoreDonuts moreDonuts;

	private void Start()
	{
		moreDonuts = FindObjectOfType<MoreDonuts>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Police"))
		{
			moreDonuts.WinMission();
		}
	}
}
