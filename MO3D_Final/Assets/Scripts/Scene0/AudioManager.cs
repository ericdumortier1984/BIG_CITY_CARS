using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance;

	[Header("Audio Sources")]
	[SerializeField] private AudioSource musicSource;
	[SerializeField] private AudioSource sfxSource;

	[Header("Audio Mixer")]
	[SerializeField] private AudioMixer audioMixer;

	[Header("Music Clips")]
	[SerializeField] private AudioClip mainMenuMusic;
	[SerializeField] private AudioClip gameplayMusic;

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void Start()
	{
		float savedVolume = PlayerPrefs.GetFloat("MasterMusicVolume", 0.75f);
		SetMusicVolume(savedVolume);
	}

	// ================= SCENE MUSIC =================

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		RestoreMusicVolume();

		if (scene.name == "SceneMainMenu")
		{
			PlayMusic(mainMenuMusic);
		}
		else if (scene.name == "SceneGameTpFinal")
		{
			PlayMusic(gameplayMusic);
		}
	}

	private void PlayMusic(AudioClip clip)
	{
		if (clip == null) return;
		if (musicSource.clip == clip && musicSource.isPlaying) return;

		musicSource.clip = clip;
		musicSource.loop = true;
		musicSource.Play();
	}

	// ================= VOLUME =================

	public void SetMusicVolume(float sliderValue)
	{
		sliderValue = Mathf.Clamp01(sliderValue);

		float dB = Mathf.Log10(Mathf.Max(sliderValue, 0.001f)) * 20f;
		audioMixer.SetFloat("MasterMusicVolume", dB);

		PlayerPrefs.SetFloat("MasterMusicVolume", sliderValue);
	}

	public float GetMusicVolume()
	{
		return PlayerPrefs.GetFloat("MasterMusicVolume", 0.75f);
	}

	// ================= SFX =================

	public void PlaySFX(AudioClip clip, float volume = 1f)
	{
		if (clip == null) return;
		sfxSource.PlayOneShot(clip, volume);
	}

	// ================= PAUSE =================

	public void SetPaused(bool paused)
	{
		audioMixer.SetFloat("MasterMusicVolume", paused ? -20f :
			Mathf.Log10(Mathf.Max(GetMusicVolume(), 0.001f)) * 20f);
	}

	private void RestoreMusicVolume()
	{
		float sliderValue = GetMusicVolume();
		float dB = Mathf.Log10(Mathf.Max(sliderValue, 0.001f)) * 20f;
		audioMixer.SetFloat("MasterMusicVolume", dB);
	}

	public void PlayGameplayMusic()
	{
		PlayMusic(gameplayMusic);
	}

	public void PlayMissionMusic(AudioClip missionClip)
	{
		PlayMusic(missionClip);
	}
}

