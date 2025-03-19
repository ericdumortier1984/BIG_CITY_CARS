using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField] WheelCollider mFrontRight;
	[SerializeField] WheelCollider mFrontLeft;
	[SerializeField] WheelCollider mBackRight;
	[SerializeField] WheelCollider mBackLeft;

	[SerializeField] Transform mFrontRightTransform;
	[SerializeField] Transform mFrontLeftTransform;
	[SerializeField] Transform mBackRightTransform;
	[SerializeField] Transform mBackLeftTransform;

	// Marcas de frenado en ruedas traseras
	[SerializeField] GameObject mBackRightTrailTire;
	[SerializeField] GameObject mBackLeftTrailTire;

	public float mAcceleration = 1.0f;
	public float mBreakForce = 1.0f;
	public float mMaxTurnAngle = 1.0f;

	private float mCurrentAcceleration = 0.0f;
	private float mCurrentBreakForce = 0.0f;
	private float mCurrentTurnAngle = 0.0f;

	public Vector3 mCenterOfMass;

	private Rigidbody mCarRb; // Referencia al rigidbody del vehiculo
	private CarLight mCarLight; // Referencia al script de luces

	private void Start()
	{
		mCarRb = GetComponent<Rigidbody>();
		mCarRb.centerOfMass = mCenterOfMass;

		mCarLight = GetComponent<CarLight>();
	}

	private void FixedUpdate()
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
		// Aceleracion del vehiculo con teclas A y S u flechas arriba y abajo
		Debug.Log("Acceleration");
		mCurrentAcceleration = mAcceleration * Input.GetAxis("Vertical");

		// Freno del vehiculo con tecla Espacio
		if (Input.GetKey(KeyCode.Space))
		{
			Debug.Log("Breaking and show back lights");
			mCurrentBreakForce = mBreakForce;
			mCarLight.SetLight(mCarLight.mBackLight, true); // Encender luces traseras
		}
		else
		{
			mCurrentBreakForce = 0.0f;
			mCarLight.SetLight(mCarLight.mBackLight, false); // Apagar luces traseras
		}

		// Aplico velocidad a las ruedas delanteras
		mFrontRight.motorTorque = mCurrentAcceleration;
		mFrontLeft.motorTorque = mCurrentAcceleration;

		// Aplico freno a todas las ruedas
		mFrontRight.brakeTorque = mCurrentBreakForce;
		mFrontLeft.brakeTorque = mCurrentBreakForce;
		mBackRight.brakeTorque = mCurrentBreakForce;
		mBackLeft.brakeTorque = mCurrentBreakForce;

		// Aplico giro a las dos ruedas delanteras con un torque maximo
		// Uso de teclas A y D u flechas izquierda y derecha para el giro
		Debug.Log("Turning");
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
		// Dibujo las marcas de las llantas
		if (Input.GetKey(KeyCode.Space))
		{
			mBackRightTrailTire.GetComponentInChildren<TrailRenderer>().emitting = true;
			mBackLeftTrailTire.GetComponentInChildren<TrailRenderer>().emitting = true;
			Debug.Log("Drawing trail tire");
		}
		else 
		{
			mBackRightTrailTire.GetComponentInChildren<TrailRenderer>().emitting = false;
			mBackLeftTrailTire.GetComponentInChildren<TrailRenderer>().emitting = false;
		}
	}
}
