using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryMaterialTrigger : MonoBehaviour
{
	[Header("FXs")]
	[SerializeField] ParticleSystem deliveryParticle;

    private HeavyLoad heavyLoad;

	private void Start()
	{
		heavyLoad = FindObjectOfType<HeavyLoad>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Red Truck"))
		{
			deliveryParticle.transform.position = transform.position;
			deliveryParticle.gameObject.SetActive(true);
			deliveryParticle.Play();

			heavyLoad.DeliveryMaterial();
		}
	}
}
