using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour
{
	/* El sonido del motor no funciona adecuadamente si uso el singleton de AudioManager, 
	   asi que le asigno un AudioSource al GameObject*/

	[SerializeField] private AudioClip mHornSound;

	public float mMinSpeed;
	public float mMaxSpeed;
	private float mCurrentSpeed;

    private Rigidbody mCarRb;
	private AudioSource mCarSound;

	public float mMinPitch;
	public float mMaxPitch;
	private float mPitchFromCar;


	private void Start()
	{
		mCarRb = GetComponent<Rigidbody>();
		mCarSound = GetComponent<AudioSource>();
	}

	private void FixedUpdate()
	{
		SetEngineSound();
		PlayCarHorn();
	}

	private void SetEngineSound()
	{
		mCurrentSpeed = mCarRb.velocity.magnitude;
		mPitchFromCar = mCarRb.velocity.magnitude / mMaxSpeed;

		if (mCurrentSpeed < mMinSpeed)
		{
			mCarSound.pitch = mMinPitch;
		}

		if (mCurrentSpeed > mMinSpeed && mCurrentSpeed < mMaxSpeed)
		{
			mCarSound.pitch = mMinPitch + mPitchFromCar;
		}

		if (mCurrentSpeed > mMaxSpeed)
		{
			mCarSound.pitch = mMaxPitch;
		}
	}

	private void PlayCarHorn()
	{
		// Bocina del vehiculo con tecla H
		if (Input.GetKey(KeyCode.H))
		{
			AudioManager.mSoundInstance.PlaySound(mHornSound);
		}
	}
}
