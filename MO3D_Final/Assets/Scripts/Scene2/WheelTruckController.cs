using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTruckController : MonoBehaviour
{
	[SerializeField] WheelCollider mFrontRight;
	[SerializeField] WheelCollider mFrontLeft;
	[SerializeField] WheelCollider mBackRight;
	[SerializeField] WheelCollider mBackLeft;
	[SerializeField] WheelCollider mMiddleLeft;
	[SerializeField] WheelCollider mMiddleRight;

	[SerializeField] Transform mFrontRightTransform;
	[SerializeField] Transform mFrontLeftTransform;
	[SerializeField] Transform mBackRightTransform;
	[SerializeField] Transform mBackLeftTransform;
	[SerializeField] Transform mMiddleLeftTransform;
	[SerializeField] Transform mMiddleRightTransform;

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
	private ItemWaypointController mItemWaypointController; // Referencia al script de items waypoints
	private CarFuelController mCarFuelController; // Referencia al script de combustible
	private CoinsController mCoinsController; // Referencia al script de coins

	private void Start()
	{
		mCarRb = GetComponent<Rigidbody>();
		mCarRb.centerOfMass = mCenterOfMass;

		mCarLight = GetComponent<CarLight>();
		mItemWaypointController = FindObjectOfType<ItemWaypointController>(); // Encontrar el script en la escena
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
		UpdateWheel(mMiddleRight, mMiddleRightTransform);

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

	// Metodo para recoleccion de items waypoints con mi player
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "ItemWaypoint")
		{
			mItemWaypointController.ItemWaypointCounter();
			mItemWaypointController.ItemWaypointTextCounter();
			Destroy(other.gameObject);
			Debug.Log("ItemWaypoint Collected");
		}

		if (other.tag == "ItemFuel")
		{
			mCarFuelController.OnfillingFuel();
			Destroy(other.gameObject);
			Debug.Log("ItemFuelCollected");
		}

		/*
		if (other.tag == "FuelPump")
		{
			mCarFuelController.OnUseFuelPump();
			Debug.Log("Full Tank");
		}*/

		if (other.tag == "Coins")
		{
			mCoinsController.ItemCoinsCounter();
			mCoinsController.ItemCoinsTextCounter();
			Destroy(other.gameObject);
			Debug.Log("Coin Collected");
		}
	}
}
