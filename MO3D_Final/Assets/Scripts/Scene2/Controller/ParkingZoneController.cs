using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParkingZoneController : MonoBehaviour
{
	[Header("FXs")]
	[SerializeField] private ParticleSystem unloadparticle;

	[Header("Slider")]
	[SerializeField] private Slider unloadSlider;

	// REFERENCES
	private BoxCollider parkingZoneCollider;
	private ParkingOdyssey parkingOdyssey;

	// BOOL
	private bool isTruckInside = false;

	// FLOAT
	private float unloadTime = 3f;

	private void Start()
	{
		parkingZoneCollider = GetComponent<BoxCollider>();
		parkingOdyssey = FindObjectOfType<ParkingOdyssey>();
	}

	private void OnTriggerStay(Collider other)
	{
		if (IsParked(other))
		{
			StartCoroutine(UnloadWeight());
		}
	}

	private bool IsParked(Collider truckCollider)
	{
		Bounds parkingZoneBounds = parkingZoneCollider.bounds;
		Bounds truckBounds = truckCollider.bounds;

		return parkingZoneBounds.Contains(truckBounds.min) &&
			parkingZoneBounds.Contains(truckBounds.max);
	}

	private IEnumerator UnloadWeight()
	{
		unloadSlider.gameObject.SetActive(true);

		float time = 0f;
		while (time < unloadTime)
		{
			time += Time.deltaTime;
			unloadSlider.value = time / unloadTime;
			yield return null;
		}

		unloadSlider.gameObject.SetActive(false);

		unloadparticle.transform.position = transform.position;
		unloadparticle.gameObject.SetActive(true);
		unloadparticle.Play();

		isTruckInside = true;
		parkingOdyssey.TruckParked(this);
	}
}
