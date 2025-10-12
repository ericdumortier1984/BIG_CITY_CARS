using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RouteDrawer : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Transform carPlayer;
	[SerializeField] private Transform target;
	[SerializeField] private Camera minimapCamera;
	[SerializeField] private float heightOffset;

	private LineRenderer lineRenderer;

	private void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		if (lineRenderer.positionCount == 0) { return; }

		// ACTUALIZAR LA LINEA
		Vector3 startPos = carPlayer.position + Vector3.up * heightOffset;
		Vector3 endPos = target.position + Vector3.up * heightOffset;

		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, startPos);
		lineRenderer.SetPosition(1, endPos);
	}

	public void SetTarget(Transform newTarget)
	{
		target = newTarget;
	}

	public void ClearRoute()
	{
		lineRenderer.positionCount = 0;
	}
}
