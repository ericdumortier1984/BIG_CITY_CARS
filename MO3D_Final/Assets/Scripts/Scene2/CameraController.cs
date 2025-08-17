using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Camera[] mCamera; // Referencia a las camaras
	private int mCameraIndex = 0; // Indice de la camara actual

	private void Start()
	{
		for (int i = 0; i < mCamera.Length; i++)
		{
			mCamera[i].gameObject.SetActive(i == mCameraIndex); // Primer camara activa
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			ChangeCamera(); // Cambiar camaras al presionar la tecla C
			Debug.Log("Camera changed to: " + mCamera[mCameraIndex].name);
		}
	}

	private void ChangeCamera()
	{
		mCamera[mCameraIndex].gameObject.SetActive(false); // Desactiva camara actual
		mCameraIndex = (mCameraIndex + 1) % mCamera.Length; // Siguiente camara
		mCamera[mCameraIndex].gameObject.SetActive(true); // Activa siguiente camara
	}
}
