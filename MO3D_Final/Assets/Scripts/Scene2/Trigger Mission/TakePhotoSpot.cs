using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePhotoSpot : MonoBehaviour
{
	[Header("FXs")]
	[SerializeField] private ParticleSystem spotParticle;

	[Header("Photo Target")]
	[SerializeField] private Transform photoTarget;

	[Header("Photo Camera")]
	[SerializeField] private Camera photoCamera;

	private CityTour cityTour;

	private void Start()
	{
		cityTour = FindObjectOfType<CityTour>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Bus"))
		{
			spotParticle.transform.position = transform.position;
			spotParticle.gameObject.SetActive(true);
			spotParticle.Play();

			cityTour.EnterPhotoMode(this);
		}
	}

	public Transform GetPhotoTarget()
	{
		return photoTarget;
	}

	public Camera GetPhotoCamera()
	{
		return photoCamera;
	}

	public void DisableSpot()
	{
		gameObject.SetActive(false);
	}
}
