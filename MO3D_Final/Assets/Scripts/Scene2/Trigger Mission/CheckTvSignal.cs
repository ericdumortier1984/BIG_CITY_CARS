using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckTvSignal : MonoBehaviour
{
	[Header("Fix Settings")]
	[SerializeField] private GameObject fixSlider;

	private LowSignal lowSignal;
	private float fixTime = 3f;
	private bool isRepairing = false;

	private void Start()
	{
		lowSignal = FindObjectOfType<LowSignal>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (isRepairing) { return; }

		if (other.CompareTag("TV Truck"))
		{
			isRepairing = true;
			StartCoroutine(FixAntenna());
		}
	}

	private IEnumerator FixAntenna()
	{
		fixSlider.SetActive(true);

		float time = 0f;

		while (time < fixTime)
		{
			time += Time.deltaTime;
			yield return null;
		}

		fixSlider.SetActive(false);

		if (lowSignal != null)
			{
				lowSignal.FixTvSignal(this);
			}

		gameObject.SetActive(false);
	}
}
