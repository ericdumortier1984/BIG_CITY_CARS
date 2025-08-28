using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineSwitcher : MonoBehaviour
{
	[Header("Gameplay Cameras")]
	[SerializeField] private CinemachineVirtualCamera[] vcams;
	[SerializeField] private CinemachineVirtualCamera rearCam;
	[SerializeField] private CinemachineVirtualCamera rightSideCam;
	[SerializeField] private CinemachineVirtualCamera leftSideCam;

	private int currentIndex = 0;

	void Start()
	{
		SetActiveCamera(0);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			currentIndex = (currentIndex + 1) % vcams.Length;
			SetActiveCamera(currentIndex);
		}

		if (Input.GetKeyDown(KeyCode.F))
		{
			SetRearCamera();
		}

		if (Input.GetKeyDown(KeyCode.V))
		{
			SetRightSideCamera();
		}

		if (Input.GetKeyDown(KeyCode.X))
		{
			SetLeftSideCamera();
		}

		if (Input.GetKeyUp(KeyCode.V))
		{
			OnDisable();
			SetActiveCamera(currentIndex);
		}

		if (Input.GetKeyUp(KeyCode.X))
		{
			OnDisable();
			SetActiveCamera(currentIndex);
		}

		if (Input.GetKeyUp(KeyCode.F))
		{
			OnDisable();
			SetActiveCamera(currentIndex);
		}
	}

	private void SetActiveCamera(int index)
	{
		for (int i = 0; i < vcams.Length; i++)
		{
			vcams[i].Priority = (i == index) ? 20 : 10;
		}
		//Debug.Log("Switched to VCam: " + vcams[index].name);
	}

	private void SetRearCamera()
	{
		rearCam.Priority = 20;
	}

	private void SetRightSideCamera()
	{
		rightSideCam.Priority = 20;
	}

	private void SetLeftSideCamera()
	{
		leftSideCam.Priority = 20;
	}

	private void OnDisable()
	{
		rearCam.Priority = 10;
		rightSideCam.Priority = 10;
		leftSideCam.Priority = 10;
	}
}
