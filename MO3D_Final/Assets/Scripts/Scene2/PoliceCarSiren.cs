using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceCarSiren : MonoBehaviour
{
    private AudioSource mPoliceSiren;

	private void Start()
	{
		AudioSource[] mAudioSources = GetComponents<AudioSource>();
		mPoliceSiren = mAudioSources[0];
	}

	private void FixedUpdate()
	{
		PlayPoliceSiren();
	}

	private void PlayPoliceSiren()
    {
		if (Input.GetKey(KeyCode.P) && !mPoliceSiren.isPlaying)
		{
			mPoliceSiren.Play();
		}
		else if (!Input.GetKey(KeyCode.P) && mPoliceSiren.isPlaying)
		{
			mPoliceSiren.Stop();
		}

	}
}
