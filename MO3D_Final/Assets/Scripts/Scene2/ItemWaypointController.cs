using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemWaypointController : MonoBehaviour
{
	[Header("Waypoints Settings")]
	[SerializeField] private TMPro.TextMeshProUGUI mItemWaypointText; 
    [SerializeField] private Slider mItemWaypointSlider; 
    [SerializeField] private int mItemWaypointToCollect; 
    private int mItemWaypointCollected; 

	public int ItemWaypointCollected { get { return mItemWaypointCollected; } set { mItemWaypointCollected = value; } }

	private void Start() 
	{
		mItemWaypointSlider.maxValue = mItemWaypointToCollect; 
		mItemWaypointSlider.value = mItemWaypointCollected; 
		mItemWaypointSlider.interactable = false;

		ItemWaypointTextCounter();
	}

	public void ItemWaypointCounter()
	{
		mItemWaypointCollected++; 
		mItemWaypointCollected = Mathf.Clamp(mItemWaypointCollected, 0, mItemWaypointToCollect); 
		mItemWaypointSlider.value = mItemWaypointCollected; 
	}

	public void ItemWaypointTextCounter()
	{
		mItemWaypointText.text = mItemWaypointCollected.ToString() + " / " + mItemWaypointToCollect.ToString();
	}
}
