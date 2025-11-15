using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapEmission : MonoBehaviour
{
	private Renderer myRender;

	[SerializeField] private Color emissionColor ;
	[SerializeField] private float pulseSpeed ;
	[SerializeField] private float intensity;

	private void Start()
	{
		myRender = GetComponent<Renderer>();
	}

	private void Update()
	{
		if (myRender == null) return; 

		float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
		Color finalColor = emissionColor * (pulse * intensity);
		myRender.material.SetColor("_EmissionColor", finalColor);
	}
}
