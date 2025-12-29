using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolBusArrive : MonoBehaviour
{
	[Header("FXs")]
	[SerializeField] private ParticleSystem schoolStopParticle;

	private SchoolSucks schoolSucks;

	private void Start()
	{
		schoolSucks = FindObjectOfType<SchoolSucks>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("School Bus"))
		{
			schoolStopParticle.transform.position = transform.position;
			schoolStopParticle.gameObject.SetActive(true);
			schoolStopParticle.Play();

			schoolSucks.BusArrive();
		}
	}
}
