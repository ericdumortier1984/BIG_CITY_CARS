using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBounce : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float amplitude = 0.5f; 
	[SerializeField] private float frequency = 2f; 
	private Vector3 startPos;

	void Start()
	{
		startPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
	}

	void Update()
	{
		float bounce = startPos.z + Mathf.Sin(Time.time * frequency) * amplitude;
		transform.localPosition = new Vector3(startPos.x, startPos.y, bounce);
	}
}
