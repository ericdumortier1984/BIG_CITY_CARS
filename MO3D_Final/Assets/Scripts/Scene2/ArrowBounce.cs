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
		startPos = transform.localPosition;
	}

	void Update()
	{
		float bounce = startPos.y + 1 + Mathf.Sin(Time.time * frequency) * amplitude;
		transform.localPosition = new Vector3(startPos.x, startPos.y, bounce);
	}
}
