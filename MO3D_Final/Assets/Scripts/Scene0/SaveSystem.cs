using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem
{
    private static string mSavePath => Path.Combine(Application.persistentDataPath, "save_data.json");

    public static void SaveGame(SaveData mData)
    {
        string mJson = JsonUtility.ToJson(mData, true);
        File.WriteAllText(mSavePath, mJson);
        Debug.Log("Game saved to: " + mSavePath);
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(mSavePath))
        {
            string mJson = File.ReadAllText(mSavePath);
			SaveData mData = JsonUtility.FromJson<SaveData>(mJson);
			return mData;
        }
        else
        {
            Debug.Log("No save file found at: " + mSavePath);
            return new SaveData();
        }
    }

	public static void ResetGame()
	{
		if (File.Exists(mSavePath))
		{
			File.Delete(mSavePath);
			Debug.Log("Save file deleted: " + mSavePath);
		}
		else
		{
			Debug.Log("No save file to delete.");
		}
	}
}
