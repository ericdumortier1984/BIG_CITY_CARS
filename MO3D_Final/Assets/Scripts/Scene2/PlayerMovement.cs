using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody mRigidBody;
	public float mSpeed = 5.0f; // velocidad de movimiento hacia adelante y atras
	public float mTurnSpeed = 100.0f; // velocidad de rotacion

	private void Start()
	{
		mRigidBody = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		float mVertical = Input.GetAxis("Vertical"); // input del usuario hacia adelante y atras
		float mHorizontal = Input.GetAxis("Horizontal"); // input del usuario para la rotacion

		Vector3 mDrive = transform.forward * mVertical * mSpeed * Time.deltaTime;
		mRigidBody.MovePosition(mRigidBody.position + mDrive); // Mueve el rigidBody

		float mTurn = mHorizontal * mTurnSpeed * Time.deltaTime;
		Quaternion mTurnRotation = Quaternion.Euler(0.0f, mTurn, 0.0f);
		mRigidBody.MoveRotation(mRigidBody.rotation * mTurnRotation); // Rota el rigidBody
	}
}
