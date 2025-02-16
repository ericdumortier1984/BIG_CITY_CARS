using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovementAndRotation : MonoBehaviour
{
	Rigidbody mRigidBody;
	public float mSpeed = 0.0f; // velocidad de movimiento hacia adelante y atrás
	public float mTurnSpeed = 0.0f; // velocidad de rotación
	public WheelCollider[] mWheelColliders; // Wheel Colliders
	public Transform[] mWheelMeshes; // Wheel Meshes

	private void Start()
	{
		mRigidBody = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		float mVertical = Input.GetAxis("Vertical"); // input del usuario hacia adelante y atrás
		float mHorizontal = Input.GetAxis("Horizontal"); // input del usuario para la rotación

		Move(mVertical, mHorizontal);
		UpdateWheels();
	}

	private void Move(float mVertical, float mHorizontal)
	{
		for (int i = 0; i < mWheelColliders.Length; i++)
		{
			mWheelColliders[i].motorTorque = mVertical * mSpeed;
			if (i < 2)
			{
				mWheelColliders[i].steerAngle = mHorizontal * mTurnSpeed;
			}
		}

		// Rotar el cuerpo del vehículo
		float mAngle = mHorizontal * mTurnSpeed * Time.deltaTime;
		Quaternion mDeltaRotation = Quaternion.Euler(0f, mAngle, 0f);
		mRigidBody.MoveRotation(mRigidBody.rotation * mDeltaRotation);
	}

	private void UpdateWheels()
	{
		for (int i = 0; i < mWheelColliders.Length; i++)
		{
			UpdateWhellPosition(mWheelColliders[i], mWheelMeshes[i]);
		}
	}

	private void UpdateWhellPosition(WheelCollider mCollider, Transform mMesh)
	{
		Vector3 mPosition;
		Quaternion mQuaternion;
		mCollider.GetWorldPose(out mPosition, out mQuaternion);

		mMesh.position = mPosition;
		mMesh.rotation = mQuaternion;
	}
}
