using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
	[Header("SFX")]
	[SerializeField] private AudioClip backSFX;

	public void BackToMainMenu()
	{
		AudioManager.Instance.PlaySFX(backSFX);
		LoaderScene.Load(LoaderScene.mScene.SceneMainMenu);
	}
}
