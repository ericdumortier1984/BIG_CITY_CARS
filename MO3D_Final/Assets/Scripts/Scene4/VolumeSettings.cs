using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class VolumeSettings : MonoBehaviour
{
    public Slider mMusicVolume;
    public AudioMixer mAudioMixer;

	private void Start()
	{
		// Guardo el volumen de la musica
		float mSavedVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
		mMusicVolume.value = mSavedVolume;
		mAudioMixer.SetFloat("musicVolume", mSavedVolume);
	}

	public void ChangeMusicVolume(float mValue)
	{
		float mVolume = mMusicVolume.value;
		mAudioMixer.SetFloat("musicVolume", mVolume);
		PlayerPrefs.SetFloat("musicVolume", mVolume); // musica guardada
	}
}
