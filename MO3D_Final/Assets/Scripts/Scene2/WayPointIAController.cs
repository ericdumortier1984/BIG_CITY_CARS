using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointIAController : MonoBehaviour
{
    [SerializeField] private List<Transform> mWaypoint;
    [SerializeField] float mSpeed = 1.0f;
    private int mCurrentWayPointIndex = 0;
    private void Start()
	{
		// Desactivar el componente MeshRenderer de los waypoints
		foreach (Transform waypoint in mWaypoint)
		{
			MeshRenderer renderer = waypoint.GetComponent<MeshRenderer>();
			if (renderer != null)
			{
				renderer.enabled = false;
			}
		}
	}

	private void FixedUpdate()
	{
		if (mWaypoint.Count == 0)
		{
			return;
		}

		Transform mTarget = mWaypoint[mCurrentWayPointIndex];
		Vector3 mDirection = (mTarget.position - transform.position).normalized;

		// Rotar
		Quaternion mTargetRotation = Quaternion.LookRotation(mDirection);
		transform.rotation = Quaternion.Slerp(transform.rotation, mTargetRotation, mSpeed * Time.deltaTime);

		// Mover
		transform.position += transform.forward * mSpeed * Time.deltaTime;

		if (Vector3.Distance(transform.position, mTarget.position) < 5.0f)
		{
			mCurrentWayPointIndex = (mCurrentWayPointIndex + 1) % mWaypoint.Count; 
		}
	}
}
