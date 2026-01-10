using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverIceCreamPoint : MonoBehaviour
{
	[Header("FXs")]
	[SerializeField] private ParticleSystem deliverIcecreamParticle;

	private IceIceCreamBaby iceIceCreamBaby;

	private void Start()
	{
		iceIceCreamBaby = FindObjectOfType<IceIceCreamBaby>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ice Cream Truck"))
		{
			deliverIcecreamParticle.transform.position = transform.position;
			deliverIcecreamParticle.gameObject.SetActive(true);
			deliverIcecreamParticle.Play();

			iceIceCreamBaby.DeliverIceCream();
		}
	}
}
