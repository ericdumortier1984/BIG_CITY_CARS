using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType
{
    Collect,
    Deliver,
}

[CreateAssetMenu(fileName = "New Mission", menuName = "Mission Data")]

public class MissionData : ScriptableObject
{
    [Header("Description")]
    [SerializeField] private string missionName;
    [SerializeField] private MissionType missionType;

    public string MissionName { get { return missionName; } }
    public MissionType MissionType {get{ return missionType; }}

    [Header("Collect")]
    [SerializeField] private GameObject collectableItem;
    [SerializeField] private int collectableAmount;

    public GameObject CollectableItem { get { return collectableItem; } }
    public int CollectableAmount { get { return collectableAmount; } }

    [Header("Deliver")]
    [SerializeField] private GameObject deliveryItem;
    [SerializeField] private Transform deliveryLocation;
    [SerializeField] private int deliveryAmount;

    public GameObject DeliveryItem { get { return deliveryItem; } }
    public Transform DeliveryLocation { get { return deliveryLocation; } }
    public int DeliveryAmount { get { return deliveryAmount; } }
}
