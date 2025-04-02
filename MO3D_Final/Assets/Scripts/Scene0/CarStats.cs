using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "CarStats", menuName = "CarStats")]
public class CarStats : MonoBehaviour
{
	// Script que uso para guardar las estadistica de los autos
	[SerializeField] private string mCarName;
	[SerializeField] private string mCarPrice;
	[SerializeField] private float mCarSpeed;
	[SerializeField] private float mCarAcceleration;
	[SerializeField] private float mCarBreaking;
	[SerializeField] private float mCarHandling;

	public string CarName { get => mCarName; set => mCarName = value; }
	public string CarPrice { get => mCarPrice; set => mCarPrice = value; }
	public float CarSpeed { get => mCarSpeed; set => mCarSpeed = value; }
	public float CarAcceleration { get => mCarAcceleration; set => mCarAcceleration = value; }
	public float CarBreaking { get => mCarBreaking; set => mCarBreaking = value; }
	public float CarHandling { get => mCarHandling; set => mCarHandling = value; }

}
