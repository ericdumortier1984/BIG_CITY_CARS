using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePatient : MonoBehaviour
{
	[Header("FXs")]
	[SerializeField] private ParticleSystem hospitalParticle;

	[Header("Patients")]
	[SerializeField] private List<GameObject> patients;

	[Header("Refrence")]
	[SerializeField] private List<Transform> ambulanceStretchers;

	private Call911 call911;

	private void Start()
	{
		call911 = FindObjectOfType<Call911>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ambulance"))
		{
			hospitalParticle.transform.position = other.transform.position;
			hospitalParticle.gameObject.SetActive(true);
			hospitalParticle.Play();

			for (int i = 0; i < patients.Count; i++)
			{
				if (i >= ambulanceStretchers.Count) break;

				GameObject patient = patients[i];
				Transform stretcher = ambulanceStretchers[i];

				patient.transform.SetParent(stretcher);
				patient.transform.localPosition = new Vector3(0, 2, 0);
				patient.transform.localRotation = Quaternion.Euler(-90f, 0f, 180f);
				patient.SetActive(true);
			}

			call911.TakePatient();
		}
	}
}
