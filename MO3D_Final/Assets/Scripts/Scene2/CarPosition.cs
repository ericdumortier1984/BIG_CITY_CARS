using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPosition : MonoBehaviour
{
    public GameObject mCar; 
	private Vector3 mInitialPosition; 
	private Quaternion mInitialRotation;

	void Start()
    {
        mInitialPosition = mCar.transform.position; 
        mInitialRotation = mCar.transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        mCar.transform.position = mInitialPosition; // Resetear posicion
        mCar.transform.rotation = mInitialRotation; // Resetear rotacion
        mCar.GetComponent<Rigidbody>().velocity = Vector3.zero; // Resetear velocidad
        mCar.GetComponent<Rigidbody>().angularVelocity = Vector3.zero; // Resetear velocidad angular
    }
}
