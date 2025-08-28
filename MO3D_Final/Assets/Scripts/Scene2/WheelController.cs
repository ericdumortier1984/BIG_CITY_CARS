using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
	[Header("Wheel Colliders")]
    [SerializeField] WheelCollider mFrontRight;
	[SerializeField] WheelCollider mFrontLeft;
	[SerializeField] WheelCollider mBackRight;
	[SerializeField] WheelCollider mBackLeft;

	[Header("Wheel Meshes")]
	[SerializeField] Transform mFrontRightTransform;
	[SerializeField] Transform mFrontLeftTransform;
	[SerializeField] Transform mBackRightTransform;
	[SerializeField] Transform mBackLeftTransform;

	[Header("Wheel Trails")]
	[SerializeField] GameObject mBackRightTrailTire;
	[SerializeField] GameObject mBackLeftTrailTire;

	///////////// Variables de configuración del vehículo /////////////
	///
	// Iniciales
	public float mAcceleration = 1.0f;
	public float mBreakForce = 1.0f;
	public float mMaxTurnAngle = 1.0f;
	public Vector3 mCenterOfMass;

	// Actuales
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
		if (!vehicleIntro.IsPlayingIntro)
		{
			mCurrentAcceleration = mAcceleration * Input.GetAxis("Vertical");

			if (Input.GetKey(KeyCode.Space))
			{
				//Debug.Log("Breaking and show back lights");
				mCurrentBreakForce = mBreakForce;
				mCarLight.SetLight(mCarLight.mBrakeLight, true);
			}
			else
			{
				mCurrentBreakForce = 0.0f;
				mCarLight.SetLight(mCarLight.mBrakeLight, false);
			}

			// Aplico velocidad a las ruedas delanteras
			mFrontRight.motorTorque = mCurrentAcceleration;
			mFrontLeft.motorTorque = mCurrentAcceleration;

			// Aplico freno a todas las ruedas
			mFrontRight.brakeTorque = mCurrentBreakForce;
			mFrontLeft.brakeTorque = mCurrentBreakForce;
			mBackRight.brakeTorque = mCurrentBreakForce;
			mBackLeft.brakeTorque = mCurrentBreakForce;

			mCurrentTurnAngle = mMaxTurnAngle * Input.GetAxis("Horizontal");
			mFrontRight.steerAngle = mCurrentTurnAngle;
			mFrontLeft.steerAngle = mCurrentTurnAngle;
		}
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
