using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightsController : MonoBehaviour
{
    public enum TrafficLightState
    {
        Red, 
        Green
    }

	[Header("Config Start")]
	public bool startWithRed = true; 

	[Header("Config Time")]
    public float mRedDuration = 5f;
    public float mGreenDuration = 5f;

    [Header("Visual")]
	public Transform mRedLight;
	public Transform mGreenLight;
	public TrafficLightState mCurrentState { get; private set; }

    private float timer;

    private void Start()
    {
        timer = 0f;
		mCurrentState = startWithRed ? TrafficLightState.Red : TrafficLightState.Green;
        UpdateLights();
	}

	private void Update()
	{
        timer += Time.deltaTime;

        switch (mCurrentState)
        {
            case TrafficLightState.Red:
                if (timer >= mRedDuration)
                {
                    SwitchToGreen();
                    //Debug.Log("is green");
                }
                break;
            case TrafficLightState.Green:
                if (timer >= mGreenDuration)
                {
                    SwitchToRed();
                    //Debug.Log("is red");
                }
                break;
        }
	}

    public bool IsRed()
    {
        return mCurrentState == TrafficLightState.Red;
    }

    private void SwitchToRed()
    {
        mCurrentState = TrafficLightState.Red;
        timer = 0f;
        UpdateLights();
    }

    private void SwitchToGreen()
    {
        mCurrentState = TrafficLightState.Green;
        timer = 0f;
        UpdateLights();
    }

    private void UpdateLights()
    {
        if (mRedLight != null)
        {
            mRedLight.gameObject.SetActive(mCurrentState == TrafficLightState.Red);
        }
        if (mGreenLight != null)
        {
            mGreenLight.gameObject.SetActive(mCurrentState == TrafficLightState.Green);
        }
    }
}
