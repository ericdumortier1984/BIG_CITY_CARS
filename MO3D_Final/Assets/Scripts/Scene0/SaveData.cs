using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
	public int mCoins = 0;
	public List<bool> mUnlockedCars = new List<bool>();
	public List<bool> mMissionsCompleted = new List<bool>();

	public SaveData()
	{
		mCoins = 0;
		mUnlockedCars = new List<bool>();
		mMissionsCompleted = new List<bool>();

		mUnlockedCars.Add(true);
		mMissionsCompleted.Add(false);
	}
}
