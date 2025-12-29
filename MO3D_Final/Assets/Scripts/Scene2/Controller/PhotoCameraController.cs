using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoCameraController : MonoBehaviour
{
	[Header("Rotation")]
	[SerializeField] private float sensitivity;
	[SerializeField] private float minX;
	[SerializeField] private float maxX;
	[SerializeField] private float minY;
	[SerializeField] private float maxY;

	private float rotX;
	private float rotY;

	private bool canMove = false;

	public void EnableControl(bool value)
	{
		canMove = value;
	}

	private void Update()
	{
		if (!canMove) return;

		float mouseX = Input.GetAxis("Mouse X") * sensitivity * 100f * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * sensitivity * 100f * Time.deltaTime;

		rotY += mouseX;
		rotX -= mouseY;

		rotX = Mathf.Clamp(rotX, minX, maxX);
		rotY = Mathf.Clamp(rotY, minY, maxY);

		transform.localRotation = Quaternion.Euler(rotX, rotY, 0f);
	}
}
