using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{

	public GameObject mWheel;
    public WheelCollider mWheelCollider;
    public float mTorque = 200.0f;

	private void Start()
	{
		mWheelCollider = this.GetComponent<WheelCollider>();
	}

	private void Update()
	{
		float acceleration = Input.GetAxis("Vertical");
		Drive(acceleration);
	}

	//void Drive(float mAcceleration)
	void Drive(float mAcceleration)

	{
		mAcceleration = Mathf.Clamp(mAcceleration, -1.0f, 1.0f);
		float torque = mAcceleration * mTorque;
		mWheelCollider.motorTorque = torque;

		Quaternion mQuaternion;
		Vector3 mPosition;
		mWheelCollider.GetWorldPose(out mPosition, out mQuaternion);
		mWheel.transform.position = mPosition;
		mWheel.transform.rotation = mQuaternion;
	}
}
