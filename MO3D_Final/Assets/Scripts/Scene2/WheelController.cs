using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour, IPController
{
	[Header("Sound")]
	[SerializeField] private AudioClip collectSound;

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
	[SerializeField] private float offRoadForce = 0.0f;

	[Header("Crash Settings")]
	[SerializeField] private float crashForce = 15f;
	[SerializeField] private float speedLossOnCrash = 0.5f;
	[SerializeField] private float upwardCrashForce = 1.5f;

	[Header("Spin Settings")]
	[SerializeField] private float spinTorque = 8f;
	[SerializeField] private float minSpinVelocity = 3f;
	[SerializeField] private float spinDamping = 0.95f;

	[Header("Bounce Settings")]
	[SerializeField] private float bounceForce = 6f;
	[SerializeField] private float maxBounceForce = 10f;
	[SerializeField] private float minBounceSpeed = 2f;

	private Vector3 mCenterOfMass;

	private float mCurrentAcceleration = 0.0f;
	private float mCurrentBreakForce = 0.0f;
	private float mCurrentTurnAngle = 0.0f;

	private Rigidbody mCarRb;
	private CarLight mCarLight;
	private ItemWaypointController mItemWaypointController;
	private CarFuelController mCarFuelController;
	private CoinsController mCoinsController;

	public float Acceleration { get => mAcceleration; set => mAcceleration = value; }
	public Rigidbody Rigidbody => mCarRb;

	private void Start()
	{
		mCarRb = GetComponent<Rigidbody>();
		mCarRb.centerOfMass = mCenterOfMass;

		mCarLight = GetComponent<CarLight>();

		mItemWaypointController = FindObjectOfType<ItemWaypointController>();
		mCarFuelController = FindObjectOfType<CarFuelController>();
		mCoinsController = FindObjectOfType<CoinsController>();
	}

	private void Update()
	{
		MoveCar();

		UpdateWheel(mFrontRight, mFrontRightTransform);
		UpdateWheel(mFrontLeft, mFrontLeftTransform);
		UpdateWheel(mBackLeft, mBackLeftTransform);
		UpdateWheel(mBackRight, mBackRightTransform);

		OnDrawTrailTire();
		OffRoadForce();
	}

	private void MoveCar()
	{
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
	}

	private void OffRoadForce()
	{
		if (Input.GetAxis("Vertical") > 0 && mCarRb.velocity.magnitude < 5f
			&& CompareTag("Off Road"))
		{
			mCarRb.AddForce(transform.forward * offRoadForce, ForceMode.Acceleration);
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
			AudioManager.Instance.PlaySFX(collectSound);
			mItemWaypointController.ItemWaypointCounter();
			mItemWaypointController.ItemWaypointTextCounter();
			LevelData.WaypointsCollectedInLevel = mItemWaypointController.ItemWaypointCollected; // Actualiza LevelData
			Destroy(other.gameObject);
		}

		if (other.tag == "ItemFuel")
		{
			mCarFuelController.OnfillingFuel();
			Destroy(other.gameObject);
		}

		if (other.tag == "Coins")
		{
			Destroy(other.gameObject);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!collision.gameObject.CompareTag("IACar")) return;

		Rigidbody otherRb = collision.rigidbody;
		if (otherRb == null) return;

		ContactPoint[] contacts = new ContactPoint[collision.contactCount];
		collision.GetContacts(contacts);

		Vector3 avgNormal = Vector3.zero;
		foreach (var contact in contacts)
		{
			avgNormal += contact.normal;
		}
			
		avgNormal.Normalize();

		Vector3 impactDirection = -avgNormal;

		// CHOQUE
		mCarRb.AddForce(impactDirection * crashForce, ForceMode.Impulse);
		otherRb.AddForce(-impactDirection * crashForce, ForceMode.Impulse);

		mCarRb.velocity *= speedLossOnCrash;
		otherRb.velocity *= speedLossOnCrash;

		// REBOTE
		float relativeSpeed = collision.relativeVelocity.magnitude;

		if (relativeSpeed > minBounceSpeed)
		{
			Vector3 bounceDir = -avgNormal;

			float bounceAmount = Mathf.Clamp(relativeSpeed * bounceForce, 0f, maxBounceForce);

			mCarRb.AddForce(bounceDir * bounceAmount, ForceMode.Impulse);
		}

		// TROMPO
		float impactSpeed = Vector3.Dot(mCarRb.velocity, impactDirection);

		if (Mathf.Abs(impactSpeed) > minSpinVelocity)
		{
			float spinDirection = Vector3.Cross(impactDirection, transform.forward).y;

			Vector3 spinTorqueVector = Vector3.up * spinDirection * spinTorque * Mathf.Abs(impactSpeed);
			mCarRb.AddTorque(spinTorqueVector, ForceMode.Impulse);

		}
	}
}
