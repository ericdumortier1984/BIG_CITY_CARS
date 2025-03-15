using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	[SerializeField] AudioMixer mMusicMixer;
	public AudioClip mBackgroundMusic;
	public AudioClip mGameMusic;

	private AudioSource mAudioSource;

	public static AudioManager mMusicInstance;

	private void Awake()
	{
		if(mMusicInstance == null)
		{
			mMusicInstance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
			
	}

	private void Start()
	{
		mAudioSource = GetComponent<AudioSource>();
		mMusicMixer.SetFloat("musicVolume", PlayerPrefs.GetFloat("musicvolume", 1f));
		PlayBackgroundMusic();
	}

	private void Update()
	{
		HandleSceneMusic();
	}

	private void PlayBackgroundMusic()
	{
		if (mBackgroundMusic != null)
		{
			mAudioSource.clip = mBackgroundMusic;
			mAudioSource.loop = true;
			mAudioSource.Play();
		}
	}

	private void HandleSceneMusic()
	{
		string mCurrentScene = SceneManager.GetActiveScene().name;

		if (mCurrentScene == "SceneGameTpFinal") 
		{
			if (mAudioSource.clip != mGameMusic)
			{
				mAudioSource.clip = mGameMusic;
				mAudioSource.loop = true;
				mAudioSource.Play();
			}
		}
		else
		{
			if (mAudioSource.clip != mBackgroundMusic)
			{
				PlayBackgroundMusic();
			}
		}
	}
}
