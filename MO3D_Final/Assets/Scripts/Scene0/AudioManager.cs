using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	[SerializeField] AudioMixer mMusicMixer;
	[SerializeField] AudioSource mSfxAudioSource;

	private AudioSource mAudioSource;
	public static AudioManager mSoundInstance { get; private set; } // Singleton

	public AudioClip mBackgroundMusic;
	public AudioClip mGameMusic;

	private void Awake()
	{
		if(mSoundInstance == null)
		{
			mSoundInstance = this;
			DontDestroyOnLoad(gameObject); // Persistencia a cambios de escenas
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

		if (Input.GetKeyDown(KeyCode.M))
		{
			ToggleMusic();
		}
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

	public void PlaySound(AudioClip mClip) // reproductor de sonidos
	{
		mSfxAudioSource.PlayOneShot(mClip);
	}

	private void ToggleMusic() // muteador de musica
	{
		mAudioSource.mute = !mAudioSource.mute;
	}
}
