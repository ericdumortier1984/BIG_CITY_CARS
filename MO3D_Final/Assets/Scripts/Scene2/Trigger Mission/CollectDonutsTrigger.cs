using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectDonutsTrigger : MonoBehaviour
{
	[Header("Fx")]
	[SerializeField] private ParticleSystem getDonutsParticle;

	private MoreDonuts moreDonuts;

	private void Start()
	{
		moreDonuts = FindObjectOfType<MoreDonuts>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Police"))
		{
			getDonutsParticle.transform.position = transform.position;
			getDonutsParticle.gameObject.SetActive(true);
			getDonutsParticle.Play();

			if (moreDonuts != null)
			{
				moreDonuts.DonutCollected(this);
			}
			gameObject.SetActive(false);
		}
	}
}
