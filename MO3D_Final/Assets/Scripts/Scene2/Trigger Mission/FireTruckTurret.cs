using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTruckTurret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretPivot;
    [SerializeField] private GameObject waterStreamPrefab;
    [SerializeField] private Transform outPoint;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxDistance;

	[Header("Turret Limits Movement")]
	[SerializeField] private float minX;
	[SerializeField] private float maxX;
	[SerializeField] private float minY;
	[SerializeField] private float maxY;

	[Header("UI")]
    [SerializeField] private RectTransform crosshair;

    private bool canShoot = false;
	private Camera mainCam;

	private void Awake()
	{
		mainCam = Camera.main;

		if (crosshair != null)
		{
			crosshair.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (!canShoot) return;

		AimTurret();
		UpdateCrosshairPosition();
		
		if (Input.GetMouseButtonDown(0))
		{
			ShootWater();
		}
	}

	private void AimTurret()
	{
		// CREO EL RAYO DESDE LA CAMARA HASTA LA POSICION DEL MOUSE
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		// VARIABLES DEL IMPACTO Y EL TARGET
		RaycastHit hit;
		Vector3 targetPoint;

		// LANZO EL RAYO PARA VER SI IMPACTA CON EL TARGET EN LOS VALORES INDICADOS
		if (Physics.Raycast(ray, out hit, maxDistance))
		{
			targetPoint = hit.point;
		}
		else
		{
			targetPoint = ray.origin + ray.direction * maxDistance;
		}

		// DIRECCION DESDE EL CANION AL TARGET, TUVE QUE INVERTIRLA
		Vector3 dir = -(targetPoint - turretPivot.position);

		// CALCULO LA ROTACION EN ESA DIRECCION Y TUVE QUE AJUSTAR EL EJE Y 90 GRADOS
		Quaternion targetRot = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 90, 0); ;

		// CONVIERTO LA ROTACION A ANGULOS EULER (X, Y, Z)
		Vector3 euler = targetRot.eulerAngles;

		// AJUSTO ANGULOS MAYORES A 180 GRADOS A USAR VALORES NEGATIVOS PORQUE ES MAS FACIL DE LIMITAR
		if (euler.x > 180) euler.x -= 360;
		if (euler.y > 180) euler.y -= 360;

		// LIMITO LA ROTACION EN DOS EJES DENTRO DE UN RANGO
		euler.x = Mathf.Clamp(euler.x, minX, maxX);  
		euler.y = Mathf.Clamp(euler.y, minY, maxY);

		// INTERPOLACION LINEAL PARA ROTACION SUAVE
		turretPivot.rotation = Quaternion.Lerp(turretPivot.rotation, Quaternion.Euler(euler.x, euler.y, 0f), Time.deltaTime * rotationSpeed);
	}

	private void UpdateCrosshairPosition()
	{
		if (crosshair != null)
		{
			Vector3 mousePos = Input.mousePosition;
			crosshair.position = mousePos;
		}
	}

	private void ShootWater()
	{
		if (waterStreamPrefab == null || outPoint == null) { return; }

		GameObject water = Instantiate(waterStreamPrefab, outPoint.position, outPoint.rotation);
		ParticleSystem waterParticle = water.GetComponent<ParticleSystem>();

		if (waterParticle != null)
		{
			waterParticle.Play();
		}

		Destroy(water, 2f);
	}

	public void EnableTurret(bool active)
	{
		canShoot = active;

		if (crosshair != null)
		{
			crosshair.gameObject.SetActive(active);
		}
	}
}
