using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPalletTrigger : MonoBehaviour
{
    private ForkLiftMission forkLiftMission;

	private void Start()
	{
		forkLiftMission = FindObjectOfType<ForkLiftMission>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Pallet"))
		{
			forkLiftMission.OnSetPallet();
			gameObject.SetActive(false);
		}
	}
}
