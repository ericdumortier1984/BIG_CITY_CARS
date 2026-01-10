using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private Slider musicSlider;

	private void Start()
	{
		musicSlider.minValue = 0.05f;
		musicSlider.maxValue = 1f;

		musicSlider.value = AudioManager.Instance.GetMusicVolume();
		musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
	}
}
