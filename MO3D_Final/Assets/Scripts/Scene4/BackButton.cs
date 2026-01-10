using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
	[Header("SFX")]
	[SerializeField] private AudioClip backSFX;

	public void BackToMainMenu()
	{
		AudioManager.Instance.PlaySFX(backSFX);
		LoaderScene.Load(LoaderScene.mScene.SceneMainMenu);
	}
}
