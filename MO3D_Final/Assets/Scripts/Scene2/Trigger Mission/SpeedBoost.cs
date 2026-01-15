using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
	[Header("Boost Settings")]
	[SerializeField] GameObject rocket;
	[SerializeField] ParticleSystem boostParticle;
	[SerializeField] private float boostVelocity;
	[SerializeField] private float boostingTime;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip collectSFX;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Tunning Car"))
		{
			AudioManager.Instance.PlaySFX(collectSFX);
			WheelController carEngine = other.GetComponent<WheelController>();
			StartCoroutine(BoostUp(carEngine));
		}
	}

	private IEnumerator BoostUp(WheelController carEngine)
	{
		boostParticle.gameObject.SetActive(true);

		carEngine.Acceleration *= boostVelocity;

		if (TryGetComponent<Collider>(out Collider selfCollider))
		selfCollider.enabled = false;

		Renderer[] rends = GetComponentsInChildren<Renderer>(true);
		foreach (Renderer r in rends)
		r.enabled = false;

		yield return new WaitForSeconds(boostingTime);

		carEngine.Acceleration /= boostVelocity;
		
		gameObject.SetActive(false);
	}
}
