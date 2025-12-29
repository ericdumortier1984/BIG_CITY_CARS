using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodID : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int foodID;

    public int ID => foodID; // Llamo desde Fast Food script
}
