using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOffRoadFlag : MonoBehaviour
{
	[Header("FX")]
	[SerializeField] private ParticleSystem getParticle;

	private OffRoadAventure offRoadAdventure;

	private void Start()
	{
		offRoadAdventure = FindObjectOfType<OffRoadAventure>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Off Road"))
		{
			getParticle.transform.position = transform.position;
			getParticle.gameObject.SetActive(true);
			getParticle.Play();

			if (offRoadAdventure != null)
			{
				offRoadAdventure.GetFlag(this);
			}
			gameObject.SetActive(false);
		}
	}
}
