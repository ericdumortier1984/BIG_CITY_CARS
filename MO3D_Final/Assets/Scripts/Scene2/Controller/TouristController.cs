using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouristController : MonoBehaviour
{
    [Header("FXs")]
    [SerializeField] private ParticleSystem touristParticle;

    [Header("Seat position")]
    [SerializeField] private Transform seatPosition;

	public void SitTourist()
	{
		touristParticle.transform.position = transform.position;
		touristParticle.gameObject.SetActive(true);
		touristParticle.Play();

		transform.SetParent(seatPosition);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		gameObject.SetActive(true);
	}
}
