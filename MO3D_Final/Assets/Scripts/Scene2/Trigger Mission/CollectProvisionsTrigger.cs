using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectProvisionsTrigger : MonoBehaviour
{
	[Header("Fx")]
	[SerializeField] private ParticleSystem getProvisionParticle;

	private HomeShopping homeShopping;

	private void Start()
	{
		homeShopping = FindObjectOfType<HomeShopping>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Simple Car"))
		{
			getProvisionParticle.transform.position = transform.position;
			getProvisionParticle.gameObject.SetActive(true);
			getProvisionParticle.Play();

			if (homeShopping != null)
			{
				homeShopping.OnProvisionPurchased(this);
			}
			gameObject.SetActive(false);
		}
	}
}
