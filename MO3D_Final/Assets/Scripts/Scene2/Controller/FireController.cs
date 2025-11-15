using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
	[Header("Fire Zone Trigger")]
	[SerializeField] private FireZoneTrigger fireZone;

	private ParticleSystem fireParticles;
	private bool isExtinguished = false;

	private void Awake()
	{
		if (fireParticles == null)
		{
			fireParticles = GetComponentInChildren<ParticleSystem>();
		}
	}

	private void OnParticleCollision(GameObject other)
	{
		if (other.CompareTag("Water Stream"))
		{
			Extinguish();
		}
	}

	public void Extinguish()
	{
		if (isExtinguished) { return; }
		isExtinguished = true;

		if (fireParticles != null && fireParticles.isPlaying)
		{
			fireParticles.Stop();
		}

		if (fireZone != null)
		{
			fireZone.gameObject.SetActive(false);
		}
		
		StartCoroutine(DelayDisableFire());
	}

	private IEnumerator DelayDisableFire()
	{
		yield return new WaitForSeconds(1f);
		gameObject.SetActive(false);

		TooHot mission = FindObjectOfType<TooHot>();

		if (mission != null)
		{
			mission.OnFireExtinguished();
			mission.ExitWaterMode();
		}
	}
}
