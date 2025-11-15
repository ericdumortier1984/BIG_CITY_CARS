/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapIndicator : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Transform carPlayer;
	[SerializeField] private RectTransform maskRect;      // Contenedor UI del minimapa (RawImage / Mask)
	[SerializeField] private GameObject markerPrefab;     // Prefab UI de cada marcador (Image, pivot center)
	[SerializeField] private Camera mapCamera;            // Cámara del minimapa (debe ser ORTHOGRAPHIC)

	[Header("Settings")]
	[SerializeField] private float edgePadding = 10f;     // Margen dentro del rectángulo (píxeles)
	[SerializeField] private float mapHeight = 20f;       // Altura física de la cámara minimapa sobre el mundo
	[SerializeField] private float arrowSpriteOffset = 90f; // Ajuste si tu sprite flecha apunta "hacia arriba"
	[SerializeField] private bool drawDebugLines = false; // Para debugging en escena

	private readonly List<Transform> targets = new List<Transform>();
	private readonly List<RectTransform> markers = new List<RectTransform>();

	private float halfWidthPx;
	private float halfHeightPx;
	private float unitsToPixelsX; // cuántos píxeles UI por 1 unidad mundo en X
	private float unitsToPixelsY; // cuántos píxeles UI por 1 unidad mundo en Y

	private void Start()
	{
		if (mapCamera == null)
		{
			Debug.LogError("MiniMapIndicator: assign mapCamera (orthographic) in inspector.");
			enabled = false;
			return;
		}
		if (!mapCamera.orthographic)
		{
			Debug.LogWarning("MiniMapIndicator: recommended to use an orthographic mapCamera for correct mapping.");
		}

		// Tamaño util en píxeles del rect transform (restando padding)
		halfWidthPx = (maskRect.rect.width * 0.5f) - edgePadding;
		halfHeightPx = (maskRect.rect.height * 0.5f) - edgePadding;

		// Calcular escala unidades_mundo -> píxeles_UI a partir de la cámara ortográfica
		// vertical visible en el mundo = orthoSize * 2
		// horizontal visible = vertical * aspect
		float halfWorldHeight = mapCamera.orthographicSize;           // en unidades mundo
		float halfWorldWidth = halfWorldHeight * mapCamera.aspect;    // en unidades mundo

		// píxeles por unidad
		unitsToPixelsX = halfWidthPx / halfWorldWidth;
		unitsToPixelsY = halfHeightPx / halfWorldHeight;
	}

	public void AddTarget(Transform target)
	{
		targets.Add(target);
		GameObject marker = Instantiate(markerPrefab, maskRect);
		RectTransform rt = marker.GetComponent<RectTransform>();
		rt.pivot = new Vector2(0.5f, 0.5f);
		markers.Add(rt);
	}

	private void LateUpdate()
	{
		if (carPlayer == null) return;

		// Mantener la cámara del minimapa centrada en el jugador (si la cámara está moviéndose con este transform)
		transform.position = new Vector3(carPlayer.position.x, mapHeight, carPlayer.position.z);

		// Ángulo Y de la cámara (si el minimapa rota con la cámara)
		float camAngleRad = mapCamera.transform.eulerAngles.y * Mathf.Deg2Rad;
		float sin = Mathf.Sin(camAngleRad);
		float cos = Mathf.Cos(camAngleRad);

		for (int i = 0; i < targets.Count; i++)
		{
			if (targets[i] == null)
			{
				// limpiar
				if (markers[i] != null) Destroy(markers[i].gameObject);
				markers.RemoveAt(i);
				targets.RemoveAt(i);
				i--;
				continue;
			}

			Vector3 worldOffset = targets[i].position - carPlayer.position;
			worldOffset.y = 0f;

			// Rotación clásica: (x', y') = (x cos - z sin, x sin + z cos)
			Vector2 rotated = new Vector2(
				worldOffset.x * sin + worldOffset.z * cos,
				worldOffset.x * cos - worldOffset.z * sin
			);

			// Convertir unidades mundo -> píxeles UI usando escalas separadas
			Vector2 mapPosPx = new Vector2(rotated.x * unitsToPixelsX, rotated.y * unitsToPixelsY);

			// DEBUG: mostrar línea en la escena desde el jugador hasta el objetivo
			if (drawDebugLines)
			{
				Debug.DrawLine(carPlayer.position + Vector3.up * 0.2f, targets[i].position + Vector3.up * 0.2f, Color.green);
				// También mostrar la "dirección rotada" en el mundo para comparar:
				Vector3 dirWorld = new Vector3(rotated.x, 0f, rotated.y);
				Debug.DrawRay(carPlayer.position, dirWorld.normalized * 2f, Color.cyan);
			}

			// Clamp rectangular FORZANDO al borde (no "acercarlo")
			bool isOutside = false;
			Vector2 displayPos = mapPosPx;

			if (Mathf.Abs(mapPosPx.x) > halfWidthPx || Mathf.Abs(mapPosPx.y) > halfHeightPx)
			{
				isOutside = true;

				// obtener ángulo hacia el borde en coordenadas UI (atan2(y,x))
				float angleToEdge = Mathf.Atan2(mapPosPx.y, mapPosPx.x);

				// colocar en el borde rect (x limitado por halfWidthPx, y limitado por halfHeightPx)
				// usamos cos/sin mezclados con halfWidthPx/halfHeightPx para mantener relación correcta
				displayPos = new Vector2(Mathf.Cos(angleToEdge) * halfWidthPx, Mathf.Sin(angleToEdge) * halfHeightPx);

				// Nota: este método coloca el marcador en el lado exacto del rectángulo borde.
			}

			// Asignar posición en el canvas (anchoredPosition está en píxeles)
			markers[i].anchoredPosition = displayPos;

			// ROTACIÓN: para las flechas offscreen usamos la dirección original en UI coords
			if (isOutside)
			{
				// calculamos ángulo en grados con base en mapPosPx (no en displayPos)
				float angleDeg = Mathf.Atan2(mapPosPx.y, mapPosPx.x) * Mathf.Rad2Deg;
				float finalRotation = angleDeg - arrowSpriteOffset;
				markers[i].rotation = Quaternion.Euler(0f, 0f, finalRotation);
			}
			else
			{
				markers[i].rotation = Quaternion.identity;
			}

			// Color/visual
			Image img = markers[i].GetComponent<Image>();
			if (img != null) img.color = isOutside ? Color.red : Color.white;
		}
	}
}*/



using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapIndicator : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Transform carPlayer;
	[SerializeField] private RectTransform maskRect;
	[SerializeField] private GameObject markerPrefab;
	[SerializeField] private Camera mapCamera;

	[Header("Settings")]
	[SerializeField] private float mapScale = 1.0f;
	[SerializeField] private float edgePadding = 20.0f;
	[SerializeField] private float mapHeight = 45.0f;

	private readonly List<Transform> targets = new List<Transform>();
	private readonly List<RectTransform> markers = new List<RectTransform>();
	private float mapRadius;

	private void Start()
	{
		mapRadius = maskRect.rect.width * 0.5f;
	}
	public void AddTarget(Transform target)
	{
		targets.Add(target);
		GameObject marker = Instantiate(markerPrefab, maskRect);
		markers.Add(marker.GetComponent<RectTransform>());
	}
	private void Update()
	{
		Vector3 tp;
		tp.x = carPlayer.transform.position.x;
		tp.y = mapHeight;
		tp.z = carPlayer.transform.position.z;
		this.transform.position = tp;

		for (int i = 0; i < targets.Count; i++)
		{
			if (targets[i] == null)
			{
				// limpiar
				if (markers[i] != null) Destroy(markers[i].gameObject);
				markers.RemoveAt(i);
				targets.RemoveAt(i);
				i--;
				continue;
			}
		}


		for (int i = 0; i < targets.Count; i++)
		{
			if (targets[i] == null) { continue; }
			Vector3 worldOffset = targets[i].position - carPlayer.position;
			worldOffset.y = 0.0f;
			float angle = -mapCamera.transform.eulerAngles.y * Mathf.Deg2Rad;
			float sin = Mathf.Sin(angle); float cos = Mathf.Cos(angle);
			Vector2 rotated = new Vector2(worldOffset.x * cos - worldOffset.z * sin, worldOffset.x * sin + worldOffset.z * cos);
			Vector2 mapPos = rotated * mapScale;
			float distance = mapPos.magnitude;
			bool isOutside = distance > (mapRadius - edgePadding);
			if (isOutside)
			{
				mapPos = mapPos.normalized * (mapRadius - edgePadding);
			}

			markers[i].anchoredPosition = mapPos;

			float dirAngle = Mathf.Atan2(mapPos.x, mapPos.y) * Mathf.Rad2Deg;
			markers[i].rotation = Quaternion.Euler(0, 0, -dirAngle); // OBJECTS OUTSIDE MINI MAP

			Image img = markers[i].GetComponent<Image>();
			if (img != null)
			{
				img.color = isOutside ? Color.red : Color.white;  // COLOR CHANGE																 
			}
		}
	}
}





/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapIndicator : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Transform carPlayer;
	[SerializeField] private RectTransform maskRect;
	[SerializeField] private GameObject markerPrefab;
	[SerializeField] private Camera mapCamera;

	[Header("Settings")]
	[SerializeField] private float mapScale = 1.0f;
	[SerializeField] private float edgePadding = 20.0f;
	[SerializeField] private float mapHeight = 45.0f;

	private readonly List<Transform> targets = new List<Transform>();
	private readonly List<RectTransform> markers = new List<RectTransform>();

	private float halfWidth;
	private float halfHeight;

	private void Start()
	{
		halfWidth = maskRect.rect.width * 0.5f - edgePadding;
		halfHeight = maskRect.rect.height * 0.5f - edgePadding;
	}

	public void AddTarget(Transform target)
	{
		if (target == null) return;
		targets.Add(target);
		GameObject marker = Instantiate(markerPrefab, maskRect);
		markers.Add(marker.GetComponent<RectTransform>());
	}

	private void Update()
	{
		Vector3 tp = new Vector3(carPlayer.position.x, mapHeight, carPlayer.position.z);
		transform.position = tp;

		for (int i = 0; i < targets.Count; i++)
		{
			if (targets[i] == null)
			{
				// limpiar
				if (markers[i] != null) Destroy(markers[i].gameObject);
				markers.RemoveAt(i);
				targets.RemoveAt(i);
				i--;
				continue;
			}
		}

			for (int i = 0; i < targets.Count; i++)
		{
			if (targets[i] == null) continue;

			Vector3 offset = targets[i].position - carPlayer.position;
			offset.y = 0f;

			float angle = mapCamera.transform.eulerAngles.y * Mathf.Deg2Rad;
			float sin = Mathf.Sin(angle);
			float cos = Mathf.Cos(angle);

			Vector2 rotated = new Vector2(
				offset.x * sin + offset.z * cos,
				offset.x * cos - offset.z * sin
			);

			Vector2 mapPos = rotated * mapScale;

			// --- Mantener dentro del cuadrado ---
			bool isOutside = false;

			if (Mathf.Abs(mapPos.x) > halfWidth || Mathf.Abs(mapPos.y) > halfHeight)
			{
				isOutside = true;
				Vector2 clamped = mapPos;

				// Limitar por borde
				float ratioX = halfWidth / Mathf.Abs(mapPos.x);
				float ratioY = halfHeight / Mathf.Abs(mapPos.y);
				float ratio = Mathf.Min(ratioX, ratioY);

				clamped *= ratio;
				mapPos = clamped;
			}

			markers[i].anchoredPosition = mapPos;

			float dirAngle = Mathf.Atan2(mapPos.x, mapPos.y) * Mathf.Rad2Deg;
			markers[i].rotation = Quaternion.Euler(0, 0, -dirAngle);

			// COLOR según rango
			Image img = markers[i].GetComponent<Image>();
			if (img != null)
				img.color = isOutside ? Color.red : Color.white;
		}
	}
}*/

