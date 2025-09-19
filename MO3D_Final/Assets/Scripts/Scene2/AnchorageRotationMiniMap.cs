using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorageRotationMiniMap : MonoBehaviour
{
	[SerializeField] private GameObject carPlayer;

	private void LateUpdate()
	{
		transform.position = new Vector3(carPlayer.transform.position.x, 20.0f, carPlayer.transform.position.z);
	}
}
