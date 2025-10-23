using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightsController : MonoBehaviour
{
    public enum TrafficLightState { Red, Green }
	public TrafficLightState mCurrentState { get; private set; }

	[Header("Time Settings")]
    [SerializeField] private float mRedDuration = 5f;
    [SerializeField] private float mGreenDuration = 5f;

    [Header("Visual Settings")]
	[SerializeField] private Transform mRedLight;
	[SerializeField] private Transform mGreenLight;
	
    private float timer;
	private bool startWithRed = true;

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

    public void SwitchToRed()
    {
        mCurrentState = TrafficLightState.Red;
        timer = 0f;
        UpdateLights();
    }

    public void SwitchToGreen()
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
