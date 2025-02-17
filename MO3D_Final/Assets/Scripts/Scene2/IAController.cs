using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : MonoBehaviour
{

	[SerializeField] WheelCollider mFrontRight;
	[SerializeField] WheelCollider mFrontLeft;
	[SerializeField] WheelCollider mBackRight;
	[SerializeField] WheelCollider mBackLeft;

	[SerializeField] Transform mFrontRightTransform;
	[SerializeField] Transform mFrontLeftTransform;
	[SerializeField] Transform mBackRightTransform;
	[SerializeField] Transform mBackLeftTransform;

	private void FixedUpdate()
	{


		UpdateWheel(mFrontRight, mFrontRightTransform);
		UpdateWheel(mFrontLeft, mFrontLeftTransform);
		UpdateWheel(mBackLeft, mBackLeftTransform);
		UpdateWheel(mBackRight, mBackRightTransform);
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
}
