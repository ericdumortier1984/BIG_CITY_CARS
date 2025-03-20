using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpeedometer : MonoBehaviour
{
    [SerializeField] private Transform mNeedle; // Referencia a aguja de velocimetro
    [SerializeField] private Rigidbody mCarRb; // Referencia al rigidbody del vehiculo

    // Rango de angulos de la aguja del velocimetro
    [SerializeField] private float mMinNeedleAngle = 0f;
    [SerializeField] private float mMaxNeedleAngle = 0f;

    // Velocidad maxima del vehiculo
    [SerializeField] private float mMaxSpeed = 200f;

	private void FixedUpdate()
	{
        UpdateNeedle();
	}

    private void UpdateNeedle()
    {
        // Velocidad del vehiculo en km/h
        float mSpeed = mCarRb.velocity.magnitude * 10f;

        // Angulo de la aguja del velocimetro
        float mNeedleAngle = Mathf.Lerp(mMinNeedleAngle, mMaxNeedleAngle, mSpeed / mMaxSpeed);

        // Rotacion en eje Z de la aguja del velocimetro
        mNeedle.eulerAngles = new Vector3(0, 0, mNeedleAngle); 
    }
}
