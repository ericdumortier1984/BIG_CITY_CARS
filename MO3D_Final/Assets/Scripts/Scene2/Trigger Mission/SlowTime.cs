using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : MonoBehaviour
{
	[Header("SlowDown Settings")]
	[SerializeField] private ParticleSystem slowParticle;
	[SerializeField] private float slowDownVelocity;
	[SerializeField] private float slowingDownTime;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Tunning Car"))
		{
			WheelController carEngine = other.GetComponent<WheelController>();
			StartCoroutine(SlowVelocity(carEngine));
		}
	}

	private IEnumerator SlowVelocity(WheelController carEngine)
	{
		slowParticle.gameObject.SetActive(true);

		carEngine.Acceleration /= slowDownVelocity;

		if (TryGetComponent<Collider>(out Collider selfCollider))
			selfCollider.enabled = false;

		Renderer[] rends = GetComponentsInChildren<Renderer>(true);
		foreach (Renderer r in rends)
			r.enabled = false;

		yield return new WaitForSeconds(slowingDownTime);

		carEngine.Acceleration *= slowDownVelocity;

		gameObject.SetActive(false);
	}
}
