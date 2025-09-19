using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour
{
	[Header("Car Sound Settings")]
	[SerializeField] private float mMinSpeed;
	[SerializeField] private float mMaxSpeed;
	[SerializeField] private float mMinPitch;
	[SerializeField] private float mMaxPitch;
	private float currentSpeed;
	private float pitchFromCar;

	private Rigidbody carRb;
	private AudioSource carEngine;
	private AudioSource carHorn;
	private AudioSource carBrakes;

	private void Start()
	{
		carRb = GetComponent<Rigidbody>();

		AudioSource[] mAudioSources = GetComponents<AudioSource>();
		carEngine = mAudioSources[0]; // Motor
		carHorn = mAudioSources[1];  // Bocina
		carBrakes = mAudioSources[2]; // Frenos
	}

	private void Update()
	{
		SetEngineSound();
		PlayCarHorn();
		PlayCarBrakes();
	}

	private void SetEngineSound()
	{
		currentSpeed = carRb.velocity.magnitude;
		pitchFromCar = carRb.velocity.magnitude / mMaxSpeed;

		if (currentSpeed < mMinSpeed)
		{
			carEngine.pitch = mMinPitch;
		}

		if (currentSpeed > mMinSpeed && currentSpeed < mMaxSpeed)
		{
			carEngine.pitch = mMinPitch + pitchFromCar;
		}

		if (currentSpeed > mMaxSpeed)
		{
			carEngine.pitch = mMaxPitch;
		}
	}

	private void PlayCarHorn()
	{
		if (Input.GetKey(KeyCode.H))
		{
			carHorn.PlayOneShot(carHorn.clip);
		}
	}

	private void PlayCarBrakes()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			if (currentSpeed > mMinSpeed && currentSpeed < mMaxSpeed)
			{
				carBrakes.PlayOneShot(carBrakes.clip);
			}
		}
	}
}