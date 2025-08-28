using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemWaypointController : MonoBehaviour
{
	public TMPro.TextMeshProUGUI mItemWaypointText; 
    public Slider mItemWaypointSlider; 
    public int mItemWaypointToCollect; 
    public int mItemWaypointCollected; 

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

	// Metodo trigger para recolectar items waypoints
	/*private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "ItemWaypoint")
		{
			ItemWaypointCounter(); // Llamado al metodo contador
			LevelData.WaypointsCollectedInLevel = mItemWaypointCollected; // Actualiza los datos en LevelData
			Debug.Log("Waypoints recolectados: " + LevelData.WaypointsCollectedInLevel); // Verifica el valor
			ItemWaypointTextCounter();
			Destroy(other.gameObject);
			Debug.Log("Waypoint collected"); // Mensaje de depuración
		}
	}*/
}
