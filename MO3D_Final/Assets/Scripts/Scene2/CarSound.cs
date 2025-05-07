using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour
{
	public float mMinSpeed;
	public float mMaxSpeed;
	private float mCurrentSpeed;

	private Rigidbody mCarRb;
	private AudioSource mCarEngine;
	private AudioSource mCarHorn;
	private AudioSource mCarBrakes;

	public float mMinPitch;
	public float mMaxPitch;
	private float mPitchFromCar;

	private void Start()
	{
		mCarRb = GetComponent<Rigidbody>();
	
		///////////// Array de AudioSources ////////////
		///
		AudioSource[] mAudioSources = GetComponents<AudioSource>();
		mCarEngine = mAudioSources[0]; // Motor
		mCarHorn = mAudioSources[1];  // Bocina
		mCarBrakes = mAudioSources[2]; // Frenos
	}

	private void FixedUpdate()
	{
		SetEngineSound();
		PlayCarHorn();
		PlayCarBrakes();
	}

	private void SetEngineSound()
	{
		////// Aumenta el pitch del sonido del motor dependiendo de la velocidad del vehiculo //////
		///
		mCurrentSpeed = mCarRb.velocity.magnitude;
		mPitchFromCar = mCarRb.velocity.magnitude / mMaxSpeed;

		if (mCurrentSpeed < mMinSpeed)
		{
			mCarEngine.pitch = mMinPitch;
		}

		if (mCurrentSpeed > mMinSpeed && mCurrentSpeed < mMaxSpeed)
		{
			mCarEngine.pitch = mMinPitch + mPitchFromCar;
		}

		if (mCurrentSpeed > mMaxSpeed)
		{
			mCarEngine.pitch = mMaxPitch;
		}
	}

	private void PlayCarHorn()
	{
		// Bocina del vehiculo con tecla H
		if (Input.GetKey(KeyCode.H))
		{
			mCarHorn.PlayOneShot(mCarHorn.clip);
		}
	}

	private void PlayCarBrakes()
	{
		// Frenos del vehiculo con tecla Espacio
		if (Input.GetKey(KeyCode.Space))
		{
			if (mCurrentSpeed > mMinSpeed && mCurrentSpeed < mMaxSpeed)
			{
				mCarBrakes.PlayOneShot(mCarBrakes.clip);
			}
		}
	}
}