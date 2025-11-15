using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorageRotationMiniMap : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private GameObject carPlayer;
	[SerializeField] private float positionY;

	private void LateUpdate()
	{
		transform.position = new Vector3(carPlayer.transform.position.x, positionY, carPlayer.transform.position.z);
	}
}
