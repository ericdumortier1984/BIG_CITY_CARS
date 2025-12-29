using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSchoolBoy : MonoBehaviour
{
    [Header("FXs")]
    [SerializeField] private ParticleSystem busStopParticle;

	private SchoolSucks schoolSucks;

	private void Start()
	{
		schoolSucks = FindObjectOfType<SchoolSucks>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("School Bus"))
		{
			busStopParticle.transform.position = transform.position;
			busStopParticle.gameObject.SetActive(true);
			busStopParticle.Play();

			schoolSucks.PickUpPassenger(this);
		}
	}
}
