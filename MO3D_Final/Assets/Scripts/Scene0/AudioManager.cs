using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	public static AudioManager mSoundInstance { get; private set; } // Utilizo el singleton para acceder al sonido desde mi menu Game Settings

	[SerializeField] AudioMixer mMusicMixer;
	[SerializeField] AudioSource mSfxAudioSource;

	private AudioSource mAudioSource;
	public AudioClip mBackgroundMusic;
	public AudioClip mLevelMusic;

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
		ToggleMusic();
	}

	private void PlayBackgroundMusic()
	{
		if (mBackgroundMusic != null)
		{
			mAudioSource.clip = mBackgroundMusic;
			mAudioSource.Play();
		}
	}

	private void PlayLevelMusic()
	{
		if (mLevelMusic != null)
		{
			mAudioSource.clip = mLevelMusic;
			mAudioSource.Play();
		}
	}

	private void HandleSceneMusic()
	{
		/////// Cambio de musica dependiendo de la escena /////////
		///
		string mCurrentScene = SceneManager.GetActiveScene().name;

		if (mCurrentScene == "SceneGameTpFinal") 
		{
			if (mAudioSource.clip != mLevelMusic)
			{
				mAudioSource.clip = mLevelMusic;
				PlayLevelMusic();
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
		if (Input.GetKeyDown(KeyCode.M))
		{
			mAudioSource.mute = !mAudioSource.mute;
		}
	}
}
