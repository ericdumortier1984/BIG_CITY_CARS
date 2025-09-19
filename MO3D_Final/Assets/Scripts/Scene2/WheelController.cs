using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
	[Header("Wheel Colliders")]
    [SerializeField] private WheelCollider mFrontRight;
	[SerializeField] private WheelCollider mFrontLeft;
	[SerializeField] private WheelCollider mBackRight;
	[SerializeField] private WheelCollider mBackLeft;

	[Header("Wheel Meshes")]
	[SerializeField] private Transform mFrontRightTransform;
	[SerializeField] private Transform mFrontLeftTransform;
	[SerializeField] private Transform mBackRightTransform;
	[SerializeField] private Transform mBackLeftTransform;

	[Header("Wheel Trails")]
	[SerializeField] private TrailRenderer mBackRightTrailTire;
	[SerializeField] private TrailRenderer mBackLeftTrailTire;

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
	private VehicleIntro vehicleIntro;

	private void Start()
	{
		mCarRb = GetComponent<Rigidbody>();
		mCarRb.centerOfMass = mCenterOfMass;

		mCarLight = GetComponent<CarLight>();

		mItemWaypointController = FindObjectOfType<ItemWaypointController>(); 
		mCarFuelController = FindObjectOfType<CarFuelController>();
		mCoinsController = FindObjectOfType<CoinsController>();
		vehicleIntro = FindObjectOfType<VehicleIntro>();
	}

	private void Update()
	{
		MoveCar();

		UpdateWheel(mFrontRight, mFrontRightTransform);
		UpdateWheel(mFrontLeft, mFrontLeftTransform);
		UpdateWheel(mBackLeft, mBackLeftTransform);
		UpdateWheel(mBackRight, mBackRightTransform);

		OnDrawTrailTire();
	}

	private void MoveCar()
	{
		//if (!vehicleIntro.IsPlayingIntro)
		//{
			mCurrentAcceleration = mAcceleration * Input.GetAxis("Vertical");

			if (Input.GetKey(KeyCode.Space))
			{
				mCurrentBreakForce = mBreakForce;
				mCarLight.BackLightOn = true;
			}
			else
			{
				mCurrentBreakForce = 0.0f;
				mCarLight.BackLightOn = false;
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
		//}
	}

	void UpdateWheel(WheelCollider mCollider, Transform mTransform)
	{
		Vector3 mPosition;
		Quaternion mRotation;
		mCollider.GetWorldPose(out mPosition, out mRotation);

		mTransform.position = mPosition;
		mTransform.rotation = mRotation;
	}

	void OnDrawTrailTire()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			mBackRightTrailTire.GetComponentInChildren<TrailRenderer>().emitting = true;
			mBackLeftTrailTire.GetComponentInChildren<TrailRenderer>().emitting = true;
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
			LevelData.WaypointsCollectedInLevel = mItemWaypointController.ItemWaypointCollected; // Actualiza LevelData
			Destroy(other.gameObject);
			//Debug.Log("ItemWaypoint Collected");
		}

		if (other.tag == "ItemFuel")
		{
			mCarFuelController.OnfillingFuel();
			Destroy(other.gameObject);
			//Debug.Log("ItemFuelCollected");
		}

		if(other.tag == "Coins")
		{
			Destroy(other.gameObject);
		}
	}
}
