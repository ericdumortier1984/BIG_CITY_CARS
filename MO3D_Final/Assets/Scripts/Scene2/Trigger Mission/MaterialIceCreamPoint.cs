using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialIceCreamPoint : MonoBehaviour
{
	[Header("FXs")]
	[SerializeField] private ParticleSystem materialIcecreamParticle;

	private IceIceCreamBaby iceIceCreamBaby;

	private void Start()
	{
		iceIceCreamBaby = FindObjectOfType<IceIceCreamBaby>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ice Cream Truck"))
		{
			materialIcecreamParticle.transform.position = transform.position;
			materialIcecreamParticle.gameObject.SetActive(true);
			materialIcecreamParticle.Play();

			iceIceCreamBaby.CollectMaterial();
		}
	}
}
