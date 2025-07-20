using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarStats : MonoBehaviour
{
	// Script que uso para guardar las estadistica de los autos
	[SerializeField] private string mCarName;
	[SerializeField] private string mCarPriceText;
	[SerializeField] private int mCarPrice;
	[SerializeField] private float mCarSpeed;
	[SerializeField] private float mCarAcceleration;
	[SerializeField] private float mCarBreaking;
	[SerializeField] private float mCarHandling;
	[SerializeField] private bool mIsCarBlocked = false;

	public string CarName { get => mCarName; set => mCarName = value; }
	public string CarPriceText { get => mCarPriceText; set => mCarPriceText = value; }
	public int CarPrice { get => mCarPrice; set => mCarPrice = value; }
	public float CarSpeed { get => mCarSpeed; set => mCarSpeed = value; }
	public float CarAcceleration { get => mCarAcceleration; set => mCarAcceleration = value; }
	public float CarBreaking { get => mCarBreaking; set => mCarBreaking = value; }
	public float CarHandling { get => mCarHandling; set => mCarHandling = value; }
	public bool IsCarBlocked { get => mIsCarBlocked; set => mIsCarBlocked = value; }
}
