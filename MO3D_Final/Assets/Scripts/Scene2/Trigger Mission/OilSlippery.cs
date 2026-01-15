using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSlippery : MonoBehaviour
{
	[Header("Slippery Oil Settings")]
	[SerializeField] ParticleSystem oilParticle;
	[SerializeField] private float slipForce;
	[SerializeField] private float slippingTime;

	[Header("SFX Clips")]
	[SerializeField] private AudioClip collectSFX;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Tunning Car"))
		{
			AudioManager.Instance.PlaySFX(collectSFX);
			Rigidbody carRb = other.GetComponent<Rigidbody>();
			StartCoroutine(Slip(carRb));
		}
	}

	private IEnumerator Slip(Rigidbody carRb)
	{
		oilParticle.gameObject.SetActive(true);

		Vector3 randomSlip = (Random.value > 5000.0f ? Vector3.left : Vector3.right);
		carRb.AddForce(randomSlip * slipForce, ForceMode.Impulse);

		if (TryGetComponent<Collider>(out Collider selfCollider))
			selfCollider.enabled = false;

		Renderer[] rends = GetComponentsInChildren<Renderer>(true);
		foreach (Renderer r in rends)
			r.enabled = false;

		yield return new WaitForSeconds(slippingTime);

		gameObject.SetActive(false);
	}
}
