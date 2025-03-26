using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemWaypointController : MonoBehaviour
{
	public TMPro.TextMeshProUGUI mItemWaypointText; // Referencia al texto items waypoints del canvas
    public Slider mItemWaypointSlider; // Referencia al slider 
    public int mItemWaypointToCollect; // Referencia a la cantidad de items waypoints
    private int mItemWaypointCollected = 0; // Referencia a la cantidad de items waypoints recolectados

	private void Start() 
	{
		// Seteo de valores iniciales del slider
		mItemWaypointSlider.maxValue = mItemWaypointToCollect; // Maximo valor permitido
		mItemWaypointSlider.value = mItemWaypointCollected; // Valor actual

		ItemWaypointTextCounter();
	}

	// Metodo contador para cantidad items waypoints recolectados en slider
	public void ItemWaypointCounter()
	{
		mItemWaypointCollected++; // Incremento del contador
		mItemWaypointCollected = Mathf.Clamp(mItemWaypointCollected, 0, mItemWaypointToCollect); // No excede limite recolectado
		mItemWaypointSlider.value = mItemWaypointCollected; // Actualiza slider
	}

	// Metodo para actualizar cantidad de items waypoints recolectados en texto
	public void ItemWaypointTextCounter()
	{
		mItemWaypointText.text = mItemWaypointCollected.ToString() + "/" + mItemWaypointToCollect.ToString(); // Actualiza el texto
	}

	// Metodo trigger para recolectar items waypoints
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "ItemWaypoint")
		{
			ItemWaypointCounter(); // Llamado al metodo contador
		}
	}
}
