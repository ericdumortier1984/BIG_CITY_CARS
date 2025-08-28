using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData // Persistente
{
	public int mCoins = 0;
	public int medals = 0;
	public int totalMission = 24;
	public int basicInfoGame = 7;
	public List<bool> mUnlockedCars = new List<bool>();
	public List<bool> missionCompleted = new List<bool>();
	public List<bool> infoGame = new List<bool>();

	public SaveData()
	{
		mCoins = 0;
		medals = 0;
		mUnlockedCars = new List<bool>();
		missionCompleted = new List<bool>();

		mUnlockedCars.Add(true);

		for (int i = 0; i < totalMission; i++)
		{
			missionCompleted.Add(false);
		}

		for (int i= 0; i < basicInfoGame; i++)
		{
			infoGame.Add(false);
		}
	}
}
