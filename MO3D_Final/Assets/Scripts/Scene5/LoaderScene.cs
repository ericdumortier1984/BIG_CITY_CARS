using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LoaderScene
{

    public enum mScene
    { 
      // Enumeramos las escenas a cargar
      SceneMainMenu,
      SceneSettings,
      SceneGameCredits,
	  SceneGameTpFinal,
	  SceneEndGame,
      SceneLoading
    }
    private static mScene mSceneToload;

    public static void Load(mScene mSceneToLoad)
    {
        LoaderScene.mSceneToload = mSceneToLoad;
        SceneManager.LoadScene(mScene.SceneLoading.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(mSceneToload.ToString());
    }
}
