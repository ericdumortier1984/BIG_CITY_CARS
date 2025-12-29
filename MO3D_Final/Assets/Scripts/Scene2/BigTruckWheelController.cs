using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTruckWheelController : MonoBehaviour
{
	[Header("Wheel Colliders")]
	[SerializeField] private WheelCollider mFrontRight;
	[SerializeField] private WheelCollider mFrontLeft;
	[SerializeField] private WheelCollider mBackRight;
	[SerializeField] private WheelCollider mBackLeft;
	[SerializeField] private WheelCollider mMiddleLeft;
	[SerializeField] private WheelCollider mMiddleRight;
	[SerializeField] private WheelCollider mCenterMiddleLeft;
	[SerializeField] private WheelCollider mCenterMiddleRight;
	[SerializeField] private WheelCollider mLastMiddleLeft;
	[SerializeField] private WheelCollider mLastMiddleRight;

	[Header("Wheel Meshes")]
	[SerializeField] private Transform mFrontRightTransform;
	[SerializeField] private Transform mFrontLeftTransform;
	[SerializeField] private Transform mBackRightTransform;
	[SerializeField] private Transform mBackLeftTransform;
	[SerializeField] private Transform mMiddleLeftTransform;
	[SerializeField] private Transform mMiddleRightTrandform;
	[SerializeField] private Transform mCenterMiddleLeftTransform;
	[SerializeField] private Transform mCenterMiddleRightTransform;
	[SerializeField] private Transform mLastMiddleLeftTransform;
	[SerializeField] private Transform mLastMiddleRightTrandform;

	[Header("Wheel Trails")]
	[SerializeField] private GameObject mBackRightTrailTire;
	[SerializeField] private GameObject mBackLeftTrailTire;

	[Header("Car Settings")]
	[SerializeField] private float mAcceleration = 0.0f;
	[SerializeField] private float mBreakForce = 0.0f;
	[SerializeField] private float mMaxTurnAngle = 0.0f;
	[SerializeField] private Vector3 mCenterOfMass;

	private float mCurrentAcceleration = 0.0f;
	private float mCurrentBreakForce = 0.0f;
	private float mCurrentTurnAngle = 0.0f;

	private Rigidbody mCarRb; 
	private CarLight mCarLight; 
	private ItemWaypointController mItemWaypointController; 
	private CarFuelController mCarFuelController; 
	private CoinsController mCoinsController; 

	private void Start()
	{
		mCarRb = GetComponent<Rigidbody>();
		mCarRb.centerOfMass = mCenterOfMass;

		mCarLight = GetComponent<CarLight>();
		mItemWaypointController = FindObjectOfType<ItemWaypointController>();
		mCarFuelController = FindObjectOfType<CarFuelController>();
		mCoinsController = FindObjectOfType<CoinsController>();
	}

	private void FixedUpdate()
	{
		MoveCar();

		UpdateWheel(mFrontRight, mFrontRightTransform);
		UpdateWheel(mFrontLeft, mFrontLeftTransform);
		UpdateWheel(mBackLeft, mBackLeftTransform);
		UpdateWheel(mBackRight, mBackRightTransform);
		UpdateWheel(mMiddleLeft, mMiddleLeftTransform);
		UpdateWheel(mMiddleRight, mMiddleRightTrandform);
		UpdateWheel(mCenterMiddleRight, mCenterMiddleRightTransform);
		UpdateWheel(mCenterMiddleLeft, mCenterMiddleLeftTransform);
		UpdateWheel(mLastMiddleRight, mLastMiddleRightTrandform);
		UpdateWheel(mLastMiddleLeft, mLastMiddleLeftTransform);

		OnDrawTrailTire();
	}

	private void MoveCar()
	{
		mCurrentAcceleration = mAcceleration * Input.GetAxis("Vertical");

		if (Input.GetKey(KeyCode.Space))
		{
			mCurrentBreakForce = mBreakForce;
			mCarLight.BackLightOn = true; // Encender luces traseras
		}
		else
		{
			mCurrentBreakForce = 0.0f;
			mCarLight.BackLightOn = false; // Apagar luces traseras
		}

		// Aplico velocidad a las ruedas delanteras
		mFrontRight.motorTorque = mCurrentAcceleration;
		mFrontLeft.motorTorque = mCurrentAcceleration;

		// Aplico freno a todas las ruedas
		mFrontRight.brakeTorque = mCurrentBreakForce;
		mFrontLeft.brakeTorque = mCurrentBreakForce;
		mBackRight.brakeTorque = mCurrentBreakForce;
		mBackLeft.brakeTorque = mCurrentBreakForce;

		// Giro
		mCurrentTurnAngle = mMaxTurnAngle * Input.GetAxis("Horizontal");
		mFrontRight.steerAngle = mCurrentTurnAngle;
		mFrontLeft.steerAngle = mCurrentTurnAngle;
	}

	// Metodo para actualizar el movimiento de las ruedas con el mesh
	void UpdateWheel(WheelCollider mCollider, Transform mTransform)
	{
		// Getting el estado del collider
		Vector3 mPosition;
		Quaternion mRotation;
		mCollider.GetWorldPose(out mPosition, out mRotation);

		// Setting el estado del transform
		mTransform.position = mPosition;
		mTransform.rotation = mRotation;
	}

	void OnDrawTrailTire()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			mBackRightTrailTire.GetComponentInChildren<TrailRenderer>().emitting = true;
			mBackLeftTrailTire.GetComponentInChildren<TrailRenderer>().emitting = true;
			//Debug.Log("Drawing trail tire");
		}
		else
		{
			mBackRightTrailTire.GetComponentInChildren<TrailRenderer>().emitting = false;
			mBackLeftTrailTire.GetComponentInChildren<TrailRenderer>().emitting = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "ItemWaypoint")
		{
			mItemWaypointController.ItemWaypointCounter();
			mItemWaypointController.ItemWaypointTextCounter();
			Destroy(other.gameObject);
			//Debug.Log("ItemWaypoint Collected");
		}

		if (other.tag == "ItemFuel")
		{
			mCarFuelController.OnfillingFuel();
			Destroy(other.gameObject);
			//Debug.Log("ItemFuelCollected");
		}

		if (other.tag == "Coins")
		{
			Destroy(other.gameObject);
		}
	}
}
