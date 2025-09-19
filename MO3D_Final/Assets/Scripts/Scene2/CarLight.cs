using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLight : MonoBehaviour
{
	[SerializeField] private Light[] mFrontLight;
	[SerializeField] private Light[] mBackLight;
	[SerializeField] private Light[] mBrakeLight;

	public bool FrontLightOn { set { SetLight(mFrontLight, value); }}
	public bool BackLightOn { set { SetLight(mBackLight, value); }}
	public bool BrakeLightOn { set { SetLight(mBrakeLight, value); }}

	private void Start()
	{
		SetLight(mFrontLight, false);
		SetLight(mBackLight, false);
		SetLight(mBrakeLight, false);
	}

	private void Update()
	{
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
