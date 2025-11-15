using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetMinimap : MonoBehaviour
{
	[SerializeField] private MiniMapIndicator miniMap;

	private void Start()
	{
		if (miniMap != null)
		{
			miniMap.AddTarget(this.transform);
		}
	}
}
