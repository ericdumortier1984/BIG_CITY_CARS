using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  
    public void StartGame()
    {
        LoaderScene.Load(LoaderScene.mScene.SceneGameTpFinal);
    }

    public void ShowSettings()
    {
        LoaderScene.Load(LoaderScene.mScene.SceneSettings);
    }

    public void Showcredits()
    {
        LoaderScene.Load(LoaderScene.mScene.SceneGameCredits);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
