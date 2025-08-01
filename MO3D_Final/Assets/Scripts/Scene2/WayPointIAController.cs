using System.Collections.Generic;
using UnityEngine;

public class WayPointIAController : MonoBehaviour
{
	[SerializeField] private List<Transform> mWaypoint;
	[SerializeField] float mThresHold = 1.0f;

	float mSpeed = 0.0f;

	private int mCurrentWayPointIndex = 0;

	private void Start()
	{
		foreach (Transform waypoint in mWaypoint)
		{
			MeshRenderer renderer = waypoint.GetComponent<MeshRenderer>();

			// Solo en editor
			if (renderer != null)
			{
				renderer.enabled = false;
			}
		}
	}

	private void FixedUpdate()
	{
		MoveToWaypoint();
	}

	private void OnDrawGizmos()
	{
		if (mWaypoint == null || mWaypoint.Count == 0)
		{
			return;
		}

		Gizmos.color = Color.cyan;

		for (int i = 0; i < mWaypoint.Count; i++)
		{
			Gizmos.DrawSphere(mWaypoint[i].position, mThresHold);
			Gizmos.DrawLine(mWaypoint[i].position, mWaypoint[(i + 1) % mWaypoint.Count].position);
		}
	}

	public void MoveToWaypoint()
	{
		if (mWaypoint.Count == 0)
		{
			return;
		}

		Transform mTarget = mWaypoint[mCurrentWayPointIndex]; // Posicion actual waypoint
		Vector3 mDirection = (mTarget.position - transform.position).normalized; // Calcula la nueva direccion al siguiente
		Quaternion mTargetRotation = Quaternion.LookRotation(mDirection); // Crea la rotacion

		// Rotar y mover
		transform.rotation = Quaternion.Slerp(transform.rotation, mTargetRotation, mSpeed * Time.deltaTime);
		transform.position += transform.forward * mSpeed * Time.deltaTime;

		if (Vector3.Distance(transform.position, mTarget.position) < mThresHold)
		{
			mCurrentWayPointIndex = (mCurrentWayPointIndex + 1) % mWaypoint.Count;
		}
	}

	public void SetSpeed(float newSpeed)
	{
		mSpeed = newSpeed; 
	}
		
}


	
