using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    public GameObject controlPanel;
    public GameObject audiopanel;

	public void Awake()
	{
        controlPanel.SetActive(false);
        audiopanel.SetActive(false);
	}

	public void ShowControls()
    {
        controlPanel.SetActive(true);
        audiopanel.SetActive(false);
    }

    public void ShowAudio()
    {
        audiopanel.SetActive(true);
        controlPanel.SetActive(false);
    }

    public void BackToSettings()
    {
		controlPanel.SetActive(false);
		audiopanel.SetActive(false);
	}
}
