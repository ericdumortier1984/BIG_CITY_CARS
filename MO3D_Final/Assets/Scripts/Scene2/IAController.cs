using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : MonoBehaviour
{
	[Header("Wheel Colliders")]
	[SerializeField] private WheelCollider mFrontRight;
	[SerializeField] private WheelCollider mFrontLeft;
	[SerializeField] private WheelCollider mBackRight;
	[SerializeField] private WheelCollider mBackLeft;

	[Header("Transforms")]
	[SerializeField] private Transform mFrontRightTransform;
	[SerializeField] private Transform mFrontLeftTransform;
	[SerializeField] private Transform mBackRightTransform;
	[SerializeField] private Transform mBackLeftTransform;

	[Header("Speeds")]
	[SerializeField] private float mNormalSpeed = 0f;
	[SerializeField] private float mReducedSpeed = 0f;

	[Header("Colllision Detect Range")]
	[SerializeField] private float mDetectRange = 0f;

	[Header("Collision Distances")]
	[SerializeField] private float stopDistance = 0f;    
	[SerializeField] private float slowDownDistance = 0f; 

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

	[Header("Crash Stop")]
	[SerializeField] private float crashStopTime = 1.2f;

	[Header("Crash Sound")]
	[SerializeField] private AudioClip hitSound;
	[SerializeField] private AudioClip hornSound;

	// FLOAT
	private float crashTimer = 0f;
	
	// BOOL
	private bool stoppedByAccident = false;
	private bool shouldStop = false;
	private bool isSlowingDown = false;
	private bool crashed = false;

	public bool isStopped { get; private set; } = false;

	// REFERENCES
	private IAController carAhead = null;
	private IAController detectedCarAhead;
	private WayPointIAController mWaypointController;
	private IntersectionZone mCurrentIntersectionZone;
	private Rigidbody mCarRb;

	private void Start()
	{
		mCarRb = GetComponent<Rigidbody>();
		mWaypointController = GetComponent<WayPointIAController>();
		
		if (mWaypointController != null)
			mWaypointController.SetSpeed(mNormalSpeed);
	}

	private void Update()
	{
		if (isStopped && mCurrentIntersectionZone != null && mCurrentIntersectionZone.IsGreenLight())
		{
			mWaypointController.SetSpeed(mNormalSpeed);
			isStopped = false;
			mCurrentIntersectionZone = null;
		}

		UpdateWheel(mFrontRight, mFrontRightTransform);
		UpdateWheel(mFrontLeft, mFrontLeftTransform);
		UpdateWheel(mBackLeft, mBackLeftTransform);
		UpdateWheel(mBackRight, mBackRightTransform);

		UpdateIACar();
	}

	void UpdateWheel(WheelCollider mCollider, Transform mTransform)
	{
		Vector3 mPosition;
		Quaternion mRotation;

		mCollider.GetWorldPose(out mPosition, out mRotation);
		mTransform.position = mPosition;
		mTransform.rotation = mRotation;
	}

	public void SlowDown() // INTERSECCIONES
	{
		shouldStop = true;
	}

	public void GoAhead()
	{
		shouldStop = false;
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<IntersectionZone>(out var zone))
		{
			if (zone.IsRedLight())
			{
				mWaypointController.SetSpeed(0f);
				isStopped = true;
				mCurrentIntersectionZone = zone;
			}
		}
			
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent<IntersectionZone>(out var zone))
		{
			if (mCurrentIntersectionZone == zone)
			{
				mCurrentIntersectionZone = null;
			}

			if (!isStopped && mWaypointController != null)
			{
				mWaypointController.SetSpeed(mNormalSpeed);
			}
		}
	}

	private IAController DetectCarAhead()
	{
		float sideOffset = 0.2f;

		Vector3 center = transform.position + Vector3.up * 0.1f;
		Vector3 left = center - transform.right * sideOffset;
		Vector3 right = center + transform.right * sideOffset;

		Debug.DrawRay(center, transform.forward * mDetectRange, Color.red);
		Debug.DrawRay(left, transform.forward * mDetectRange, Color.red);
		Debug.DrawRay(right, transform.forward * mDetectRange, Color.red);

		if (RayHitCar(center)) return detectedCarAhead;
		if (RayHitCar(left)) return detectedCarAhead;
		if (RayHitCar(right)) return detectedCarAhead;

		return null;
	}

	private bool RayHitCar(Vector3 origin)
	{
		if (Physics.Raycast(origin, transform.forward, out RaycastHit hit, mDetectRange))
		{
			if (hit.collider.CompareTag("IACar") && hit.collider.gameObject != gameObject)
			{
				detectedCarAhead = hit.collider.GetComponent<IAController>();
				return true;
			}
		}
		return false;
	}


	private bool DetectAccidentAhead()
	{
		RaycastHit hit;
		Vector3 origin = transform.position + Vector3.up * 0.1f;
		Vector3 direction = transform.forward;

		Debug.DrawRay(origin, direction * mDetectRange, Color.yellow, 0.1f);

		if (Physics.Raycast(origin, direction, out hit, mDetectRange))
		{
			if (hit.collider.CompareTag("AccidentScene")) // MISION CALL 911
			{
				return true;
			}
		}

		return false;
	}

	private void UpdateIACar()
	{
		if (crashed)
		{
			crashTimer -= Time.deltaTime;

			mWaypointController.SetSpeed(0f);

			if (crashTimer <= 0f)
			{
				crashed = false;
				isStopped = false;
				mWaypointController.SetSpeed(mNormalSpeed);
			}

			return;
		}

		if (mCurrentIntersectionZone != null && mCurrentIntersectionZone.IsRedLight())
		{
			mWaypointController.SetSpeed(0f);
			isStopped = true;
			return;
		}

		if (DetectAccidentAhead())
		{
			mWaypointController.SetSpeed(0f);
			isStopped = true;
			return;
		}

		IAController carAhead = DetectCarAhead();

		if (carAhead != null)
		{
			float distance = Vector3.Distance(transform.position, carAhead.transform.position);

			if (carAhead.isStopped || distance < stopDistance)
			{
				mWaypointController.SetSpeed(0f);
				isStopped = true;
			}
			else if (distance < slowDownDistance)
			{
				mWaypointController.SetSpeed(mReducedSpeed);
				isStopped = false;
			}
			else
			{
				mWaypointController.SetSpeed(mNormalSpeed);
				isStopped = false;
			}
		}
		else
		{
			mWaypointController.SetSpeed(mNormalSpeed);
			isStopped = false;
		}

		mWaypointController.MoveToWaypoint();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (crashed) return;

		IPController playerVehicle = collision.gameObject.GetComponent<IPController>();

		if (playerVehicle == null) { return; }

		Rigidbody otherRb = playerVehicle.Rigidbody;

		if (otherRb == null) { return; }

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

		AudioManager.Instance.PlaySFX(hitSound);
		AudioManager.Instance.PlaySFX(hornSound);

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
			float spinDir = Vector3.Cross(impactDirection, transform.forward).y;
			mCarRb.AddTorque(Vector3.up * spinDir * spinTorque,ForceMode.Impulse);
		}

		crashed = true;
		crashTimer = crashStopTime;
		mWaypointController.SetSpeed(0f);
		isStopped = true;
	}
}
