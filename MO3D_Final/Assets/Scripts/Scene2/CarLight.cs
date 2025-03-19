using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLight : MonoBehaviour
{
    [SerializeField] public Light[] mFrontLight;
    [SerializeField] public Light[] mBackLight;

	private void Start()
	{
		// Luces delanteras apagadas al inicio
		SetLight(mFrontLight, false);
	}

	private void FixedUpdate()
	{
		// Encender luces delanteras con tecla L
		if (Input.GetKeyDown(KeyCode.L))
		{
			ToggleLight(mFrontLight);
			Debug.Log("Front Lights On");
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
