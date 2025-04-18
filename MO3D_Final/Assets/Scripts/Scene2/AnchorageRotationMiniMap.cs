using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorageRotationMiniMap : MonoBehaviour
{
	public GameObject mCarPlayer;
	private void LateUpdate()
	{
		transform.position = new Vector3(mCarPlayer.transform.position.x, 20.0f, mCarPlayer.transform.position.z);
	}
}
