using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTruckController : MonoBehaviour
{
    [Header("Rigidbody")]
    [SerializeField] private Rigidbody truckRb;

    [Header("Speed settings")]
    [SerializeField] private float normalSpeed;
    [SerializeField] private float loadedSpeed;
	[SerializeField] private float minSpeed;
	[SerializeField] private float maxSpeed;

	[Header("FXs")]
	[SerializeField] private List<ParticleSystem> smokeParticles;

    private bool isLoaded = false;

	private void Start()
	{
        SetLoaded(false);
	}

	private void Update()
	{
		UpdateSmoke();
	}


	public void SetLoaded(bool value)
    {
        isLoaded = value;
		if (isLoaded)
		{
			truckRb.drag = loadedSpeed;
		}
		else
		{
			truckRb.drag = normalSpeed;
		}
	}

	private void UpdateSmoke()
	{
		float verticalInput = Input.GetAxis("Vertical");
		if (isLoaded && verticalInput >= minSpeed &&
			verticalInput <= maxSpeed)
		{
			foreach (ParticleSystem smoke in smokeParticles)
			{
				if (!smoke.isPlaying)
				{
					smoke.Play();
				}
			}
		}
		else 
		{
			foreach (ParticleSystem smoke in smokeParticles)
			{
				if (!smoke.isPlaying)
				{
					smoke.Stop();
				}
			}
		}
	}
}
