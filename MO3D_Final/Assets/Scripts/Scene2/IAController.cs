using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class IAController : MonoBehaviour
{
	[Header("Wheel Colliders")]
	[SerializeField] WheelCollider mFrontRight;
	[SerializeField] WheelCollider mFrontLeft;
	[SerializeField] WheelCollider mBackRight;
	[SerializeField] WheelCollider mBackLeft;

	[Header("Transforms")]
	[SerializeField] Transform mFrontRightTransform;
	[SerializeField] Transform mFrontLeftTransform;
	[SerializeField] Transform mBackRightTransform;
	[SerializeField] Transform mBackLeftTransform;
 
	private WayPointIAController mWaypointController;
	private IntersectionZone mCurrentIntersectionZone;

	private bool shouldStop = false;
	private bool isSlowingDown = false;
	public bool isStopped { get; private set; } = false;

	[Header("Speeds")]
	[SerializeField] private float mNormalSpeed = 0f;
	[SerializeField] private float mReducedSpeed = 0f;

	[Header("Colllision Detect Range")]
	[SerializeField] private float mDetectRange = 0f;

	[Header("Collision Distances")]
	[SerializeField] private float stopDistance = 0f;      // Distancia para frenar completamente
	[SerializeField] private float slowDownDistance = 0f;  // Distancia para empezar a reducir velocidad

	private IAController carAhead = null;

	private void Start()
	{
		mWaypointController = GetComponent<WayPointIAController>();
		
		if (mWaypointController != null)
			mWaypointController.SetSpeed(mNormalSpeed);
	}

	private void FixedUpdate()
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
		DetectCarAhead();
		
	}

	void UpdateWheel(WheelCollider mCollider, Transform mTransform)
	{
		Vector3 mPosition;
		Quaternion mRotation;

		mCollider.GetWorldPose(out mPosition, out mRotation);
		mTransform.position = mPosition;
		mTransform.rotation = mRotation;
	}

	// En intersecciones
	public void SlowDown()
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
				//SlowDown();
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
			//if (mWaypointController != null)
			//mWaypointController.SetSpeed(mNormalSpeed);
			//isStopped = false;
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
	
	private bool DetectCarAhead()
	{
		RaycastHit mHit;
		Vector3 mOrigin = transform.position + Vector3.up * 0.15f;
		Vector3 mDirection = transform.forward;

		Debug.DrawRay(mOrigin, mDirection * mDetectRange, Color.red, 0.1f);

		if (Physics.Raycast(mOrigin, mDirection, out mHit, mDetectRange))
		{
			// Verificamos que el coche detectado no sea uno mismo
			return mHit.collider.CompareTag("IACar") && mHit.collider.gameObject != gameObject;

		}

		return false;
	}

	private IAController GetCarAhead()
	{
		RaycastHit mHit;
		Vector3 mOrigin = transform.position + Vector3.up * 0.15f;
		Vector3 mDirection = transform.forward;

		if (Physics.Raycast(mOrigin, mDirection, out mHit, mDetectRange))
		{
			if (mHit.collider.CompareTag("IACar") && mHit.collider.gameObject != gameObject)
			{
				mWaypointController.SetSpeed(mNormalSpeed);
				return mHit.collider.GetComponent<IAController>();
			}
		}

		return null;
	}


	private void UpdateIACar()
	{
		if (shouldStop || isStopped) // Auto frenado por semáforo
			return;

		if (mWaypointController == null)
			return;

		if (!shouldStop && mWaypointController != null)
		{
			IAController carAhead = GetCarAhead();

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
			}
			else
			{
				mWaypointController.SetSpeed(mNormalSpeed);
				isStopped = false;
			}

			//if (!DetectCarAhead())
			//{
				//isStopped = false;
			//}

			mWaypointController.MoveToWaypoint();
			isStopped = false;
		}
	}
}
