using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
	public int mCoins = 0;
	public List<bool> mUnlockedCars = new List<bool>();

	public SaveData()
	{
		mCoins = 0;
		mUnlockedCars = new List<bool>();

		// Desbloquear solo el primer auto
		mUnlockedCars.Add(true); // primer auto desbloqueado por defecto
		// Los demás se agregarán dinámicamente si existen más autos
	}
}
