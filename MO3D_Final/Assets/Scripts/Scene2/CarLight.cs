using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLight : MonoBehaviour
{
    [SerializeField] public Light[] mFrontLight;
    [SerializeField] public Light[] mBackLight;
	[SerializeField] public Light[] mBrakeLight;

	private void Start()
	{
		// Luces apagadas al inicio
		SetLight(mFrontLight, false);
		SetLight(mBackLight, false);
		SetLight(mBrakeLight, false);
	}

	private void Update()
	{
		// Encender delanteras con tecla L
		if (Input.GetKeyDown(KeyCode.L))
		{
			ToggleLight(mFrontLight);
			ToggleLight(mBackLight);
			Debug.Log("Lights On");

		}
	}

	public void ToggleLight(Light[] mLights)
	{
		foreach (Light mLight in mLights)
		{
			mLight.enabled = !mLight.enabled;
		}
	}

	public void SetLight(Light[] mLights, bool mLightState)
	{
		foreach (Light mLight in mLights)
		{
			mLight.enabled = mLightState;
		}
	}
}
