using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalTrigger : MonoBehaviour
{
    [Header("FXs")]
    [SerializeField] private ParticleSystem hospitalParticle;

    private Call911 call911;

	private void Start()
	{
		call911 = FindObjectOfType<Call911>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ambulance"))
		{
			hospitalParticle.transform.position = transform.position;
			hospitalParticle.gameObject.SetActive(true);
			hospitalParticle.Play();

			Transform[] patientsInside = other.GetComponentsInChildren<Transform>(true); // DENTRO DE LA AMBULANCIA

			foreach (Transform patientInside in patientsInside)
			{
				if (patientInside.CompareTag("Patient"))
				{
					patientInside.gameObject.SetActive(false);
				}
			}

			call911.LeavePatient();
		}
	}
}
