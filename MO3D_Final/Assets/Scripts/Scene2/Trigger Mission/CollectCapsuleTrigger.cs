using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCapsuleTrigger : MonoBehaviour
{
	[Header("Fx")]
	[SerializeField] private ParticleSystem getGrowthPillParticle;

	private LittleAndFurious littleAndFurious;

	private void Start()
	{
		littleAndFurious = FindObjectOfType<LittleAndFurious>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Mini Car"))
		{
			getGrowthPillParticle.transform.position = transform.position;
			getGrowthPillParticle.gameObject.SetActive(true);
			getGrowthPillParticle.Play();

			if (littleAndFurious != null)
			{
				littleAndFurious.GrowthPillCollected(this);
			}
			gameObject.SetActive(false);
		}
	}
}
